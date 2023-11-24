using Kbg.NppPluginNET.PluginInfrastructure;
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

        public IEnumerable<string> GetLines()
        {
            for ( int i = 0; i < _gateway.GetLineCount(); i++ )
            {
                yield return _gateway.GetLine( i );
            }
        }

        public IEnumerable<string> ReadLines( string lineStartIdentifier )
        {
            for ( int i = 0; i < _gateway.GetLineCount(); i++ )
            {
                var line = _gateway.GetLine( i );

                if ( line.StartsWith( lineStartIdentifier ) )
                {
                    yield return line;
                }
            }
        }
    }
}
