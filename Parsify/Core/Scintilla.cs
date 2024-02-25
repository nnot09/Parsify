using Kbg.NppPluginNET.PluginInfrastructure;
using Parsify.Models;
using Parsify.PluginInfrastructure;
using Parsify.XmlModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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

        public IEnumerable<LineInfo> GetLines( bool trimCrLf = false )
        {
            for ( int i = 0; i < _gateway.GetLineCount(); i++ )
            {
                string currentLine = _gateway.GetLine( i );

                if ( trimCrLf )
                    currentLine = currentLine.TrimEnd( new[] { '\r', '\n' } );

                yield return new LineInfo( currentLine, i + 1 ); // i acts as LineNo
            }
        }

        // TODO Use marking instead of selection
        public void SelectFieldValue( DataField field )
        {
            _gateway.ClearSelections();

            var area = GetSelectArea( field.Parent.DocumentLineNumber, field.Index, field.Length );

            // caret, anchor
            _gateway.SetSelection( area.Start, area.End );
        }

        public void SelectMultiplePlainFieldValues( IEnumerable<TextLine> lines, DataField field )
        {
            _gateway.ClearSelections();
            _gateway.SetMultipleSelection( true );

            // TODO Optimize
            foreach ( var line in lines.Where( l => l.LineIdentifier == ( field.Parent as TextLine ).LineIdentifier ) )
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

        public void UnhideAll( IEnumerable<TextLine> lines )
        {
            foreach ( var line in lines )
            {
                _gateway.ShowLines( line.DocumentLineNumber - 1, line.DocumentLineNumber - 1 );
            }
        }

        public void ClearSelectionHiding()
        {
            _gateway.ClearSelections();
        }

        public void HideLines( IEnumerable<int> lineNumbers )
        {
            foreach ( var lineNo in lineNumbers )
            {
                _gateway.HideLines( lineNo - 1, lineNo - 1 );
            }
        }

        public void ForceStyleUpdate( ParsifyModule module )
        {
            //StringBuilder buffer = new StringBuilder();

            //foreach ( var line in module.LineDefinitions )
            //{
            //    buffer.Append( "StartsWith:" );
            //    buffer.Append( line.StartsWithIdentifier );
            //    buffer.Append( ";" );

            //    foreach ( var field in line.Fields )
            //    {
            //        buffer.Append( field.Name );
            //        buffer.Append( "," );
            //        buffer.Append( field.Position );
            //        buffer.Append( "," );
            //        buffer.Append( field.Length );
            //        buffer.Append( ";" );
            //    }

            //    buffer.Append( "#" );
            //}

            //_gateway.SetProperty( "ModuleDefinition", buffer.ToString() );

            ILexer.Lines.Clear();
            ILexer.Lines.AddRange( module.LineDefinitions );
            _gateway.SetProperty( "refreshLexer", ILexer.RefreshLexerState ? "0" : "1" );
            _gateway.SetIdleStyling( IdleStyling.ALL );
            _gateway.SetFocus( true );

            ILexer.RefreshLexerState = !ILexer.RefreshLexerState;

            SwitchLanguage();
        }
        
        int defaultLang = 0;
        public void SwitchLanguage()
        {
            Win32.SendMessage( PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_GETCURRENTLANGTYPE, 0, out int currentLanguageId );

            int parsifyLexerId = GetLexerId( "Parsify" );
            int newLangId = 0;

            if ( currentLanguageId == parsifyLexerId )
            {
                newLangId = defaultLang;
            }
            else
            {
                defaultLang = currentLanguageId;
                newLangId = parsifyLexerId;
            }

            Win32.SendMessage( PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_SETCURRENTLANGTYPE, 0, newLangId );
        }

        public int GetLexerId( string name )
        {
            string lastLanguage = string.Empty;
            for ( int languageId = 0; true; languageId++ )
            {
                StringBuilder languageSb = new StringBuilder( 2000 );
                Win32.SendMessage( PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_GETLANGUAGENAME, languageId, languageSb );
                string language = languageSb.ToString();
                if ( language == name )
                {
                    return languageId;
                }
                if ( language == lastLanguage )
                {
                    return -1;
                }
                lastLanguage = language;
            }
        }

        private Area GetSelectArea( int lineNo, int index, int length )
        {
            int lineStartIndex = _gateway.PositionFromLine( lineNo - 1 );
            int start = lineStartIndex + index;
            int end = lineStartIndex + index + length;

            return new Area( start, end );
        }
    }

    public class Area
    {
        public int Start { get; set; }
        public int End { get; set; }

        public Area()
        {
        }
        public Area( int start, int end )
        {
            Start = start;
            End = end;
        }
    }

    public class LineInfo
    {
        public string Line { get; set; }
        public int LineNo { get; set; }

        public LineInfo()
        {
        }
        public LineInfo( string line, int lineNo )
        {
            Line = line;
            LineNo = lineNo;
        }
    }
}
