using Kbg.NppPluginNET.PluginInfrastructure;
using Parsify.Core.Models;
using Parsify.Core.Models.Values;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Parsify.Core
{
    public class Scintilla
    {
        private NotepadPPGateway _notepad;
        private ScintillaGateway _gateway;

        public Scintilla()
        {
            var ptr = PluginBase.GetCurrentScintilla();

            _notepad = new NotepadPPGateway();
            _gateway = new ScintillaGateway( ptr );
        }

        public string GetFilePath()
            => _notepad.GetCurrentFilePath();

        public ParsifyLine GetLineDefinition( string documentLine, List<ParsifyLine> lines )
        {
            if ( string.IsNullOrWhiteSpace( documentLine ) )
                return null;

            // TODO Optimize
            var line = lines
                .Where( l => documentLine.Length > l.StartsWithIdentifier.Length )
                .SingleOrDefault( l => l.StartsWithIdentifier == documentLine.Substring( 0, l.StartsWithIdentifier.Length ) );

            return line;
        }

        public IEnumerable<(string Line, int LineNo)> GetLines()
        {
            for ( int i = 0; i < _gateway.GetLineCount(); i++ )
            {
                yield return (_gateway.GetLine( i ), i + 1); // i acts as LineNo
            }
        }

        public IEnumerable<(string Line, int LineNo)> ReadLines( string lineStartIdentifier )
        {
            for ( int i = 0; i < _gateway.GetLineCount(); i++ )
            {
                var line = _gateway.GetLine( i );

                if ( line.StartsWith( lineStartIdentifier ) )
                {
                    yield return (line, i + 1); // i acts as LineNo
                }
            }
        }

        // TODO Use marking instead of selection
        public void SelectFieldValue( Field field )
        {
            _gateway.ClearSelections();

            var area = GetSelectArea( field.Parent.DocumentLineNumber, field.Index, field.Length );

            // caret, anchor
            _gateway.SetSelection( area.Start, area.End );
        }

        // TODO Csv support
        private void SelectMultiplePlainFieldValues( IEnumerable<Line> lines, Field field )
        {
            _gateway.ClearSelections();
            _gateway.SetMultipleSelection( true );

            // TODO Optimize
            foreach ( var line in lines.Where( l => l.LineIdentifier == field.Parent.LineIdentifier ) )
            {
                var area = GetSelectArea( line.DocumentLineNumber, field.Index, field.Length );

                _gateway.AddSelection( area.Start, area.End );
            }
        }

        public void SelectLines( IEnumerable<int> lineNo )
        {
            _gateway.ClearSelections();
            _gateway.SetMultipleSelection( true );

            foreach ( var line in lineNo )
            {
                int lineStartIndex = _gateway.PositionFromLine( line - 1 );
                int lineEndIndex = _gateway.GetLineEndPosition( line - 1 );

                _gateway.AddSelection( lineStartIndex, lineEndIndex );
            }
        }

        private (int Start, int End) GetSelectArea( int lineNo, int index, int length )
        {
            int lineStartIndex = _gateway.PositionFromLine( lineNo - 1 );
            int start = lineStartIndex + index;
            int end = lineStartIndex + index + length;

            return (start, end);
        }
    }
}
