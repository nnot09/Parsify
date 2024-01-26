using Parsify.Core.Config;
using Parsify.Core.Forms;
using Parsify.Core.Models;
using Parsify.Core.Models.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using TinyCsvParser.NetFramework20.Readers;

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

        public string GetErrors()
            => NumberOfErrors > 0 ? _errors.ToString() : string.Empty;
    }
}
