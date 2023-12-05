using Microsoft.VisualBasic.FileIO;
using Parsify.Core.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parsify.Core.Core
{
    public class DocumentCsvHelper : IDisposable
    {
        private StringReader _reader;
        private TextFieldParser _textFieldParser;

        public DocumentCsvHelper( string compiledLines, string splitDelimeter, string commentToken )
        {
            _reader = new StringReader( compiledLines );
            _textFieldParser = new TextFieldParser( _reader )
            {
                Delimiters = new[] { splitDelimeter },
                HasFieldsEnclosedInQuotes = true,
                TrimWhiteSpace = true,
                CommentTokens = new[] { commentToken ?? string.Empty }
            };
        }

        public IEnumerable<(string[] Fields, int LineNo)> ReadFields()
        {
            do
            {
                int lineNumber = (int)_textFieldParser.LineNumber;
                yield return (_textFieldParser.ReadFields(), lineNumber);

            } while ( _textFieldParser.EndOfData );
        }

        public void Dispose()
        {
            if ( _textFieldParser != null ) _textFieldParser.Dispose();
            if ( _reader != null ) _reader.Dispose();
        }
    }
}
