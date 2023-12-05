using Kbg.NppPluginNET.PluginInfrastructure;
using Parsify.Core.Models;
using Parsify.Core.Models.Values;
using System.Collections.Generic;
using System.Linq;
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

        public string CompileLines( bool trimCrLf = false )
        {
            StringBuilder compiled = new StringBuilder();

            foreach ( var line in GetLines( trimCrLf ) )
                compiled.AppendLine( line.Line );

            return compiled.ToString();
        }

        public IEnumerable<(string Line, int LineNo)> GetLines( bool trimCrLf = false )
        {
            for ( int i = 0; i < _gateway.GetLineCount(); i++ )
            {
                string currentLine = _gateway.GetLine( i );

                if ( trimCrLf )
                    currentLine = currentLine.TrimEnd( new[] { '\r', '\n' } );

                yield return (currentLine, i + 1); // i acts as LineNo
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
        public void SelectFieldValue( DataField field )
        {
            _gateway.ClearSelections();

            var area = GetSelectArea( field.Parent.DocumentLineNumber, field.Index, field.Length );

            // caret, anchor
            _gateway.SetSelection( area.Start, area.End );
        }

        public void SelectMultiplePlainFieldValues( IEnumerable<PlainTextLine> lines, DataField field )
        {
            _gateway.ClearSelections();
            _gateway.SetMultipleSelection( true );

            // TODO Optimize
            foreach ( var line in lines.Where( l => l.LineIdentifier == ( field.Parent as PlainTextLine ).LineIdentifier ) )
            {
                var area = GetSelectArea( line.DocumentLineNumber, field.Index, field.Length );

                _gateway.AddSelection( area.Start, area.End );
            }
        }

        public void SelectMultipleCsvFieldValues( IEnumerable<CsvLine> lines, DataField field )
        {
            _gateway.ClearSelections();
            _gateway.SetMultipleSelection( true );

            foreach ( var csvField in lines.Where( l => !l.IsHeader ).SelectMany( l => l.Fields ).Where( f => f.Name == field.Name ) )
            {
                var area = GetSelectArea( csvField.Parent.DocumentLineNumber, csvField.Index, csvField.Length );

                _gateway.AddSelection( area.Start, area.End );
            }
        }

        public void SelectSingleLine( int lineNo )
        {
            _gateway.ClearSelections();
            _gateway.SetMultipleSelection( false );

            int lineStartIndex = _gateway.PositionFromLine( lineNo - 1 );
            int lineEndIndex = _gateway.GetLineEndPosition( lineNo - 1 );

            _gateway.SetSelection( lineStartIndex, lineEndIndex );
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

        public void UnhideAll( IEnumerable<PlainTextLine> lines )
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

        private (int Start, int End) GetSelectArea( int lineNo, int index, int length )
        {
            int lineStartIndex = _gateway.PositionFromLine( lineNo - 1 );
            int start = lineStartIndex + index;
            int end = lineStartIndex + index + length;

            return (start, end);
        }

        //public void CsvShowOnly( Document document, DataField field )
        //{
        //    // Dirty workaround: we select other columns and hide.

        //    _gateway.ClearSelections();
        //    _gateway.SetMultipleSelection( true );

        //    foreach ( var line in document.Lines )
        //    {
        //        foreach ( var dataField in line.Fields.Where( f => f.Name != field.Name ) )
        //        {
        //            var area = GetSelectArea( line.DocumentLineNumber, dataField.Index, dataField.Length );
        //            _gateway.AddSelection( area.Start, area.End );
        //        }
        //    }
        //}
    }
}
