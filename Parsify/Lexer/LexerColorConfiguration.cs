using Kbg.NppPluginNET;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Parsify.Lexer
{
    internal static class LexerColorConfiguration
    {
        private static string _appData = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData );
        private static string _langDirectoryPath = Path.Combine( _appData, "Notepad++", "plugins", "config" );
        private static string _udlConfigPath = Path.Combine( _langDirectoryPath, "Parsify.xml" );

        public static void Initialize()
        {
            if ( !File.Exists( _udlConfigPath ) )
            {
                Directory.CreateDirectory( _langDirectoryPath );
                GenerateLanguage( Main.Configuration.HighlightingMode );
            }
        }

        private static void GenerateLanguage( AppHighlightingMode mode )
        {
            string[] tags = { "instre1", "instre2", "type1", "type2", "type3", "type4", "type5", "type6" };

            using ( XmlWriter writer = XmlWriter.Create( _udlConfigPath, new XmlWriterSettings() { Indent = true, Encoding = new UTF8Encoding( false ), OmitXmlDeclaration = false } ) )
            {
                writer.WriteStartDocument();
                writer.WriteStartElement( "NotepadPlus" );

                writer.WriteStartElement( "Languages" );
                writer.WriteStartElement( "Language" );
                writer.WriteAttributeString( "name", "Parsify" );
                writer.WriteAttributeString( "ext", string.Empty );

                foreach ( string tag in tags )
                {
                    writer.WriteStartElement( "Keywords" );
                    writer.WriteAttributeString( "name", tag );
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteStartElement( "LexerStyles" );
                writer.WriteStartElement( "LexerType" );
                writer.WriteAttributeString( "name", "Parsify" );
                writer.WriteAttributeString( "desc", "Parsify - Text parser" );
                writer.WriteAttributeString( "excluded", "no" );

                ColorPalette colorPalette = HighlightingConfiguration.GetColorPalette( mode );

                for ( int i = 0; i < HighlightingConfiguration.NUMBER_OF_COLORS_PER_SET; i++ )
                {
                    writer.WriteStartElement( "WordsStyle" );
                    writer.WriteAttributeString( "styleID", i.ToString() );
                    writer.WriteAttributeString( "name", i == 0 ? "Default" : "ColumnColor" + i.ToString() );
                    writer.WriteAttributeString( "fgColor", colorPalette.ForegroundColors[ i ] );
                    writer.WriteAttributeString( "bgColor", colorPalette.BackgroundColors[ i ] );
                    writer.WriteAttributeString( "fontName", "" );
                    writer.WriteAttributeString( "fontStyle", "0" );// colorPalette.BoldFlag );
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.Flush();
                writer.Close();
            }
        }

        private static void RemoveLanguage()
        {
            if ( !File.Exists( _udlConfigPath ) )
            {
                try
                {
                    File.Delete( _udlConfigPath );
                }
                catch
                {
                }
            }
        }

        public static void Refresh()
        {
            RemoveLanguage();
            GenerateLanguage( Main.Configuration.HighlightingMode );
        }
    }
}
