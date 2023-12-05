using Microsoft.VisualBasic.FileIO;
using Parsify.Core.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parsify.Core.Core
{
    public class CsvReader : IDisposable
    {
        private StringReader _reader;
        private TextFieldParser _textFieldParser;

        public int CurrentLineNo { get; private set; }
        public string CommentToken { get; }

        public CsvReader( string compiledLines, string splitDelimeter, string commentToken )
        {
            _reader = new StringReader( compiledLines );
            _textFieldParser = new TextFieldParser( _reader )
            {
                Delimiters = new[] { splitDelimeter },
                HasFieldsEnclosedInQuotes = true,
                TrimWhiteSpace = true,
                // CommentTokens = new[] { commentToken ?? string.Empty }
            };
            this.CommentToken = commentToken ?? string.Empty;
        }

        public IEnumerable<(string[] Fields, int LineNo)> ReadFields()
        {
            this.CurrentLineNo = 0;

            while ( !_textFieldParser.EndOfData )
            {
                this.CurrentLineNo++;

                // Note: This is a dirty trick to avoid running into -1 with LineNumber when ReadFields advances to next line.
                // If the next line is not existent, the LineNumber property will be -1.
                // Maybe I did some logical mistake at some point, I didn't have any coffee when I was writing this.
                // TODO In-depth analysis of this issue.

                if ( IsComment() )
                {
                    var data = _textFieldParser.ReadFields();
                    yield return (null, this.CurrentLineNo);
                }
                else
                {
                    yield return (_textFieldParser.ReadFields(), this.CurrentLineNo);
                }
            }
        }

        private bool IsComment()
        {
            string begin = _textFieldParser.PeekChars( this.CommentToken.Length );
            return begin.StartsWith( this.CommentToken );
        }

        public void Dispose()
        {
            if ( _textFieldParser != null ) _textFieldParser.Dispose();
            if ( _reader != null ) _reader.Dispose();
        }
    }
}
