using Kbg.NppPluginNET.PluginInfrastructure;
using Parsify.Core.Models;
using Parsify.Models;
using Parsify.XmlModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

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

        private int _currentLanguage;
        public int CurrentLanguage
        {
            get => _currentLanguage;
            private set => _currentLanguage = value;
        }

        public string GetFilePath()
            => _notepad.GetCurrentFilePath();

        public byte[] CurrentDocumentHash()
        {
            if ( _gateway.GetLength() == 0 )
                return null;

            using (SHA1 sha1 =  SHA1.Create())
            {
                byte[] docBytes = File.ReadAllBytes( GetFilePath() );

                return sha1.ComputeHash( docBytes );
            }
        }

        public ParsifyLine GetLineDefinition( string documentLine, List<ParsifyLine> lines )
        {
            if ( string.IsNullOrWhiteSpace( documentLine ) )
                return null;

            // TODO Optimize
            var line = lines
                .Where( l => documentLine.Length > l.StartsWithIdentifier.Length )
                .SingleOrDefault( l => l.StartsWithIdentifier == documentLine.Substring( 0, l.StartsWithIdentifier.Length ).TrimStart() );

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

        public void ClearSelect()
        {
            _gateway.ClearSelections();
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

        public void GatewaySetProperty( string propertyName, string value )
        {
            _gateway.SetProperty( propertyName, value );
            _gateway.SetIdleStyling( IdleStyling.ALL );
        }

        public void UpdateLexerStyle( ParsifyModule module )
        {
            PluginInfrastructure.Lexer.Lines.Clear();
            PluginInfrastructure.Lexer.Lines.AddRange( module.LineDefinitions );

            _gateway.SetProperty( "nnot09", "0" );
            _gateway.SetIdleStyling( IdleStyling.ALL );
            // _gateway.SetFocus( true );

            PluginInfrastructure.Lexer.RefreshLexerState = !PluginInfrastructure.Lexer.RefreshLexerState;
        }

        int defaultLang = 0;
        public void SwitchLanguage()
        {
            Win32.SendMessage( PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_GETCURRENTLANGTYPE, 0, out int currentLanguageId );

            int parsifyLexerId = GetLexerId( "Parsify" );
            int newLangId = 0;

#if DEBUG
            Debug.WriteLine( $"[{DateTime.Now}] Current Language Id: {currentLanguageId}" );
            Debug.WriteLine( $"[{DateTime.Now}] Parsify Language Id: {parsifyLexerId}" );
#endif

            if ( currentLanguageId == parsifyLexerId )
            {
#if DEBUG
                Debug.WriteLine( $"[{DateTime.Now}] Activating Default Language." );
#endif
                newLangId = defaultLang;
            }
            else
            {
#if DEBUG
                Debug.WriteLine( $"[{DateTime.Now}] Activating Parsify Language." );
#endif

                defaultLang = currentLanguageId;
                newLangId = parsifyLexerId;
            }

            Win32.SendMessage( PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_SETCURRENTLANGTYPE, 0, newLangId );
            CurrentLanguage = newLangId;
        }

        public void SetParsifyLanguage()
        {
            int parsifyLexerId = GetLexerId( "Parsify" );
            Win32.SendMessage( PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_SETCURRENTLANGTYPE, 0, parsifyLexerId );
            CurrentLanguage = parsifyLexerId;
        }

        public void SetDefaultLanguage()
        {
            Win32.SendMessage( PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_SETCURRENTLANGTYPE, 0, 0 /*Default*/ );
            CurrentLanguage = 0;
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
}
