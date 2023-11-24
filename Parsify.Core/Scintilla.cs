﻿using Kbg.NppPluginNET.PluginInfrastructure;
using Parsify.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Parsify.Core
{
    public class Scintilla
    {
        private ScintillaGateway _gateway;

        public Scintilla()
        {
            var ptr = PluginBase.GetCurrentScintilla();
            _gateway = new ScintillaGateway( ptr );
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
        public void SelectFieldValue( int lineNo, int index, int length )
        {
            _gateway.ClearSelections();

            int lineStartIndex = _gateway.PositionFromLine( lineNo - 1 );

            // caret, anchor
            _gateway.SetSelection( lineStartIndex + index, lineStartIndex + length + index );
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
    }
}
