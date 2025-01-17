﻿using Parsify.Models;
using Parsify.Other;
using Parsify.XmlModels;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Parsify.Core
{
    public class DocumentParser
    {
        public Document Document { get; private set; }
        public bool Success { get; private set; }
        public int NumberOfErrors { get; private set; }

        private StringBuilder _errors;

        public DocumentParser( Scintilla scintilla, ParsifyModule module )
        {
            _errors = new StringBuilder();

            Document = new Document
            {
                FilePath = scintilla.GetFilePath(),
                FormatName = module.Name,
                Version = module.Version,
                Parser = module
            };

            ParseText( module, scintilla );
        }

        private void ParseText( ParsifyModule module, Scintilla scintilla )
        {
            foreach ( var documentLine in scintilla.GetLines( trimCrLf: true ) )
            {
                var moduleLine = scintilla.GetLineDefinition( documentLine.Line, module.LineDefinitions );

                // Current document line isn't defined in our module XML
                if ( moduleLine == null )
                {
                    _errors.AppendLine( $"Line {documentLine.LineNo} is not defined." );
                    NumberOfErrors++;
                    continue;
                }

                // Line is defined since we're here
                TextLine line = new TextLine()
                {
                    DocumentLineNumber = documentLine.LineNo,
                    LineIdentifier = moduleLine.StartsWithIdentifier
                };

                Document.Lines.Add( line );

                int readLength = 0;
                foreach ( var moduleLineField in moduleLine.Fields )
                {
                    DataField field = new DataField()
                    {
                        Name = moduleLineField.Name,
                        Index = moduleLineField.Position - 1,
                        Length = moduleLineField.Length,
                        Parent = line,
                    };

                    field.Value = Extensions.GetField( documentLine.Line, field.Index, field.Length, documentLine.LineNo, field.Name, out string error );
                    if ( error != null )
                    {
                        if ( moduleLineField.Optional )
                        {
                            field.Value = "[missing]";
                        }
                        else
                        {
                            _errors.AppendLine( error );
                            NumberOfErrors++;
                        }
                    }
                    else
                    {
                        field.Success = true;
                    }

                    readLength += field.Length;

                    foreach ( var translatedFieldValueDef in moduleLineField.Translations )
                    {
                        bool expression = false;

                        if ( translatedFieldValueDef.IgnoreCase )
                        {
                            switch ( translatedFieldValueDef.SearchMode )
                            {
                                default:
                                case ParsifyFieldValueTranslateSearchMode.Default:
                                    expression = field.Value.Equals( translatedFieldValueDef.Value, System.StringComparison.OrdinalIgnoreCase );
                                    break;
                                case ParsifyFieldValueTranslateSearchMode.Contains:
                                    expression = field.Value.ToLower().Contains( translatedFieldValueDef.Value.ToLower() );
                                    break;
                                case ParsifyFieldValueTranslateSearchMode.StartsWith:
                                    expression = field.Value.StartsWith( translatedFieldValueDef.Value, System.StringComparison.OrdinalIgnoreCase );
                                    break;
                                case ParsifyFieldValueTranslateSearchMode.EndsWith:
                                    expression = field.Value.EndsWith( translatedFieldValueDef.Value, System.StringComparison.OrdinalIgnoreCase );
                                    break;
                                case ParsifyFieldValueTranslateSearchMode.Regex:
                                    expression = new Regex( translatedFieldValueDef.Value, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant )
                                                            .Match( field.Value ).Success;
                                    break;
                            }
                        }
                        else
                        {
                            switch ( translatedFieldValueDef.SearchMode )
                            {
                                default:
                                case ParsifyFieldValueTranslateSearchMode.Default:
                                    expression = field.Value.Equals( translatedFieldValueDef.Value );
                                    break;
                                case ParsifyFieldValueTranslateSearchMode.Contains:
                                    expression = field.Value.Contains( translatedFieldValueDef.Value );
                                    break;
                                case ParsifyFieldValueTranslateSearchMode.StartsWith:
                                    expression = field.Value.StartsWith( translatedFieldValueDef.Value );
                                    break;
                                case ParsifyFieldValueTranslateSearchMode.EndsWith:
                                    expression = field.Value.EndsWith( translatedFieldValueDef.Value );
                                    break;
                                case ParsifyFieldValueTranslateSearchMode.Regex:
                                    expression = new Regex( translatedFieldValueDef.Value, RegexOptions.Singleline | RegexOptions.CultureInvariant )
                                                            .Match( field.Value ).Success;
                                    break;
                            }
                        }

                        if ( translatedFieldValueDef.InvertCondition )
                            expression = !expression;

                        if ( expression )
                            field.CustomDisplayValue = translatedFieldValueDef.DisplayValue;
                    }

                    line.Fields.Add( field );
                }

                if ( documentLine.Length > readLength + line.LineIdentifier.Length )
                {
                    int unknownStartIndex = readLength + line.LineIdentifier.Length;
                    _errors.AppendLine( $"Undefined value at line {documentLine.LineNo} (Position {unknownStartIndex + 1}): {documentLine.Line?.Substring( unknownStartIndex ) ?? "(null)"}" );
                    NumberOfErrors++;
                }
            }

            // Find lines that are not listed in our document
            var definedLinesNotFound = module.LineDefinitions
                .Where( l => !l.Optional )
                .Where( l => !Document.Lines.Any( foundLines => foundLines.LineIdentifier == l.StartsWithIdentifier ) );
            
            foreach ( var notfound in definedLinesNotFound )
            {
                _errors.AppendLine( $"Line definition '{notfound.StartsWithIdentifier}' is missing in current document." );
                NumberOfErrors++;
            }

            Success = true;
        }

        public string GetErrors()
            => NumberOfErrors > 0 ? _errors.ToString() : string.Empty;
    }
}
