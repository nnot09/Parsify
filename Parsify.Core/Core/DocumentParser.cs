using Parsify.Core.Config;
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
                    ParseCsv( scintilla );
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

        private void ParseCsv( Scintilla scintilla )
        {
            string[] headerComponents = null;
            bool skipFirstLine = Document.HasHeader;
            bool skippedFirstLine = false;

            foreach ( var documentLine in scintilla.GetLines(trimCrLf: true) )
            {
                if ( Document.CommentLineIdentifier != null && documentLine.Line.StartsWith( Document.CommentLineIdentifier ) )
                    continue;

                if ( skipFirstLine && !skippedFirstLine )
                {
                    CsvLine header = new CsvLine()
                    {
                        DocumentLineNumber = documentLine.LineNo,
                        IsHeader = true
                    };

                    // TODO Csv escape stuff
                    headerComponents = documentLine.Line.Split( new[] { Document.CsvSplitDelimeter }, StringSplitOptions.RemoveEmptyEntries );

                    Document.Lines.Add( header );

                    skippedFirstLine = true;

                    continue;
                }

                CsvLine line = new CsvLine()
                {
                    DocumentLineNumber = documentLine.LineNo,
                };

                var fields = GetCsvFields( headerComponents, documentLine.Line, Document.CsvSplitDelimeter );

                // yes i know 
                fields.ForEach( f => f.Parent = line );

                line.Fields.AddRange(fields);

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

        private void StrongCsvValidation( string line, bool isHeader )
        {

        }

        public string GetErrors()
            => NumberOfErrors > 0 ? _errors.ToString() : string.Empty;
    }
}
