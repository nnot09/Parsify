﻿using Parsify.Core.Config;
using Parsify.Core.Forms;
using Parsify.Core.Models;
using Parsify.Core.Models.Values;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Parsify.Core.Core
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
                TextFormat = module.TextFormat,
                CsvSplitDelimeter = module.CsvSplitDelimeter,
                HasHeader = module.HasTableHeader,
                CommentLineIdentifier = module.CommentLineIdentifier,
            };

            switch ( Document.TextFormat )
            {
                case TextFormat.Csv:
                    ParseCsv( module, scintilla );
                    return;

                case TextFormat.Plain:
                    ParseText( module, scintilla );
                    return;

                default:
                    throw new NotImplementedException( "unknown format: " + Document.TextFormat );
            }
        }

        private void ParseText( ParsifyModule module, Scintilla scintilla )
        {
            foreach ( var documentLine in scintilla.GetLines() )
            {
                var moduleLine = scintilla.GetLineDefinition( documentLine.Line, module.TextLineDefinitions );

                // Current document line isn't defined in our module XML
                if ( moduleLine == null )
                {
                    _errors.AppendLine( $"LineNo {documentLine.LineNo} seems to be undefined." );
                    NumberOfErrors++;
                    continue;
                }

                // Line is defined since we're here
                PlainTextLine line = new PlainTextLine()
                {
                    DocumentLineNumber = documentLine.LineNo,
                    LineIdentifier = moduleLine.StartsWithIdentifier
                };

                Document.Lines.Add( line );

                foreach ( var moduleLineFields in moduleLine.Fields.Cast<ParsifyPlain>() )
                {
                    PlainTextField field = new PlainTextField()
                    {
                        Name = moduleLineFields.Name,
                        Index = moduleLineFields.Index,
                        Length = moduleLineFields.Length,
                        Parent = line
                    };

                    field.Value = Extensions.GetField( documentLine.Line, field.Index, field.Length );

                    line.Fields.Add( field );
                }
            }

            Success = true;
        }

        private void ParseCsv( ParsifyModule module, Scintilla scintilla )
        {
            List<string> headerComponents = null;
            bool skippedFirstLine = false;

            foreach ( var documentLine in scintilla.GetLines( trimCrLf: true ) )
            {
                if ( Document.CommentLineIdentifier != null && documentLine.Line.StartsWith( Document.CommentLineIdentifier ) )
                    continue;

                if ( Document.HasHeader && !skippedFirstLine )
                {
                    CsvLine header = new CsvLine()
                    {
                        DocumentLineNumber = documentLine.LineNo,
                        IsHeader = true
                    };

                    // TODO Csv escape stuff
                    headerComponents = documentLine.Line
                        .Split( new[] { Document.CsvSplitDelimeter }, StringSplitOptions.RemoveEmptyEntries )
                        .ToList();

                    Document.Lines.Add( header );

                    skippedFirstLine = true;

                    continue;
                }
                else if ( !Document.HasHeader && !skippedFirstLine )
                {
                    // get column definition from xml
                    var headerLine = module.TextLineDefinitions.FirstOrDefault();

                    if ( headerLine == null )
                        throw new Exception( "Header columns are not defined and there are no present header columns in current document, according to your XML definition." );

                    headerComponents = new List<string>();

                    foreach ( var columnName in headerLine.Fields )
                        headerComponents.Add( columnName.Name );

                    // validate
                    var documentColumns = documentLine.Line
                        .Split( new[] { Document.CsvSplitDelimeter }, StringSplitOptions.RemoveEmptyEntries );

                    ValidateColumns( headerComponents, documentColumns );
                }

                CsvLine line = new CsvLine()
                {
                    DocumentLineNumber = documentLine.LineNo,
                };

                var fields = GetCsvFields( headerComponents, documentLine.Line, Document.CsvSplitDelimeter );

                // yes i know 
                fields.ForEach( f => f.Parent = line );

                line.Fields.AddRange( fields );

                Document.Lines.Add( line );
            }

            Success = true;
        }

        private List<CsvField> GetCsvFields( IEnumerable<string> headerComponents, string currentLine, string splitDelimeter )
        {
            List<CsvField> fields = new List<CsvField>();

            // TODO Csv escape stuff
            var values = currentLine.Split( new[] { splitDelimeter }, StringSplitOptions.None );

            for ( int i = 0; i < values.Length; i++ )
            {
                var field = new CsvField()
                {
                    DataIndex = i,
                    Name = headerComponents.ElementAtOrDefault( i ) ?? $"Unknown{i + 1}",
                    Value = values[ i ],
                };

                fields.Add( field );
            }

            return fields;
        }

        private void ValidateColumns( List<string> headerComponents, string[] documentColumns )
        {
            if ( headerComponents.Count != documentColumns.Length )
                throw new Exception( "Header columns do not match the XML definition." );

            for ( int i = 0; i < headerComponents.Count; i++ )
            {
                var headerComponent = headerComponents.ElementAt( i );
                var documentColumn = documentColumns[ i ];

                if (  headerComponent != documentColumn )
                    throw new Exception( $"Header columns mismatch: \"{headerComponent}\" on \"{documentColumn}\"" );
            }
        }

        public string GetErrors()
            => NumberOfErrors > 0 ? _errors.ToString() : string.Empty;
    }
}
