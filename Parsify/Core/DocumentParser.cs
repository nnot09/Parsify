using Parsify.Models;
using Parsify.Other;
using Parsify.XmlModels;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Parsify.Core
{
    internal class DocumentParser
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
            foreach ( var documentLine in scintilla.GetLines() )
            {
                var moduleLine = scintilla.GetLineDefinition( documentLine.Line, module.LineDefinitions );

                // Current document line isn't defined in our module XML
                if ( moduleLine == null )
                {
                    _errors.AppendLine( $"LineNo {documentLine.LineNo} seems to be undefined." );
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

                foreach ( var moduleLineField in moduleLine.Fields )
                {
                    DataField field = new DataField()
                    {
                        Name = moduleLineField.Name,
                        Index = moduleLineField.Position - 1,
                        Length = moduleLineField.Length,
                        Parent = line,
                    };

                    field.Value = Extensions.GetField( documentLine.Line, field.Index, field.Length );

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
            }

            Success = true;
        }

        public string GetErrors()
            => NumberOfErrors > 0 ? _errors.ToString() : string.Empty;
    }
}
