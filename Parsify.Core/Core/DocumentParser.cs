using Parsify.Core.Config;
using Parsify.Core.Forms;
using Parsify.Core.Models;
using Parsify.Core.Models.Values;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsify.Core.Core
{
    internal class DocumentParser
    {
        public Document Document { get; set; }
        public bool Success { get; set; }
        public bool HasErrors { get; set; }

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
            };

            foreach ( var documentLine in scintilla.GetLines() )
            {
                var moduleLine = scintilla.GetLineDefinition( documentLine.Line, module.TextLineDefinitions );

                // Current document line isn't defined in our module XML
                if ( moduleLine == null )
                {
                    _errors.AppendLine( $"LineNo {documentLine.LineNo} seems to be undefined." );
                    HasErrors = true;
                    continue;
                }

                // Line is defined since we're here
                Line line = new Line()
                {
                    DocumentLineNumber = documentLine.LineNo,
                    LineIdentifier = moduleLine.StartsWithIdentifier
                };

                Document.Lines.Add( line );

                foreach ( var moduleLineFields in moduleLine.Fields.Cast<ParsifyPlain>() )
                {
                    Field field = new Field()
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

        public string GetErrors()
            => _errors.Length > 0 ? _errors.ToString() : string.Empty;
    }
}
