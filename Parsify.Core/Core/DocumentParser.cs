using Parsify.Core.Config;
using Parsify.Core.Forms;
using Parsify.Core.Models;
using Parsify.Core.Models.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
                CsvCommentLineIdentifier = module.CsvCommentLineIdentifier,
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
                    _errors.AppendLine( $"Format \"{Document.TextFormat}\" is currently not supported." );
                    return;
            }
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
                PlainTextLine line = new PlainTextLine()
                {
                    DocumentLineNumber = documentLine.LineNo,
                    LineIdentifier = moduleLine.StartsWithIdentifier
                };

                Document.Lines.Add( line );

                foreach ( var moduleLineFields in moduleLine.Fields.Cast<ParsifyPlain>() )
                {
                    DataField field = new DataField()
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
            List<string> headerNames = new List<string>();
            bool skippedHeader = false;

            using ( var csvDocument = new CsvReader(
                scintilla.CompileLines( trimCrLf: true ),
                Document.CsvSplitDelimeter,
                Document.CsvCommentLineIdentifier ) )
            {
                foreach ( var documentLine in csvDocument.ReadFields() )
                {
                    if ( documentLine.Fields == null )
                        continue; // Comment line

                    if ( !skippedHeader )
                    {
                        CsvLine header = new CsvLine()
                        {
                            DocumentLineNumber = documentLine.LineNo,
                            IsHeader = true
                        };

                        Document.Lines.Add( header );

                        if ( !Document.HasHeader )
                        {
                            // get column definition from xml
                            var headerLine = module.LineDefinitions.FirstOrDefault();

                            if ( headerLine == null )
                                throw new Exception( "Header columns are not defined and there are no present header columns in current document, according to your XML definition." );

                            foreach ( var columnName in headerLine.Fields )
                                headerNames.Add( columnName.Name );

                            ValidateColumns( headerNames, documentLine.Fields );
                        }
                        else
                        {
                            headerNames.AddRange( documentLine.Fields );
                        }

                        skippedHeader = true;
                        continue;
                    }

                    var line = new CsvLine()
                    {
                        DocumentLineNumber = documentLine.LineNo,
                    };

                    var fields = GetCsvFields2( headerNames, documentLine.Fields, Document.CsvSplitDelimeter );

                    // yes i know 
                    fields.ForEach( f => f.Parent = line );

                    line.Fields.AddRange( fields );

                    Document.Lines.Add( line );
                }
            }
            Success = true;
        }

        private List<DataField> GetCsvFields2( IEnumerable<string> headerComponents, string[] fields, string splitDelimeter )
        {
            List<DataField> dataFields = new List<DataField>();

            int addedLength = 0;
            for ( int i = 0; i < fields.Length; i++ )
            {
                var dataField = new DataField()
                {
                    Index = ( i * splitDelimeter.Length ) + addedLength,
                    Name = headerComponents.ElementAtOrDefault( i ) ?? $"Unknown{i + 1}",
                    Value = fields[ i ],
                    Length = fields[ i ].Length
                };

                dataFields.Add( dataField );

                addedLength += fields[ i ].Length;
            }

            return dataFields;
        }

        private void ValidateColumns( List<string> headerComponents, string[] documentColumns )
        {
            if ( headerComponents.Count != documentColumns.Length )
                throw new Exception( "Header columns do not match the XML definition." );

            for ( int i = 0; i < headerComponents.Count; i++ )
            {
                var headerComponent = headerComponents[ i ];
                var documentColumn = documentColumns[ i ];

                if ( headerComponent != documentColumn )
                    throw new Exception( $"Header columns mismatch: \"{headerComponent}\" on \"{documentColumn}\"" );
            }
        }

        public string GetErrors()
            => NumberOfErrors > 0 ? _errors.ToString() : string.Empty;
    }
}
