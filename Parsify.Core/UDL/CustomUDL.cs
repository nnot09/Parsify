using Kbg.NppPluginNET;
using Parsify.Core.Config;
using Parsify.Core.UDL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Xml;

namespace Parsify.Core
{
    internal static class CustomUDL
    {
        private static string _appData = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData );
        private static string _langDirectoryPath = Path.Combine( _appData, "Notepad++", "plugins", "config" );
        private static string _udlConfigPath = Path.Combine( _langDirectoryPath, "Parsify.xml" );

        

        public static void Initialize()
        {
            if ( !File.Exists( _udlConfigPath ) )
            {
                Directory.CreateDirectory( _langDirectoryPath );
                GenerateUdl( Main.Configuration.HighlightingMode );
            }
        }

        private static void GenerateUdl( AppHighlightingMode mode )
        {
            string[] presets = new string[] { "background higlighting mode", "foreground highlighting mode" };
            int numberOfTags = 8;

            using ( XmlWriter writer = XmlWriter.Create( _udlConfigPath, new XmlWriterSettings() { Indent = true, Encoding = new UTF8Encoding( false ) } ) )
            {
                writer.WriteStartDocument();
                writer.WriteStartElement( "NotepadPlus" );

                writer.WriteStartElement( "Languages" );
                writer.WriteStartElement( "Language" );
                writer.WriteAttributeString( "name", "Parsify" );
                writer.WriteAttributeString( "ext", string.Empty );

                for ( int i = 0; i < numberOfTags; i++ )
                {
                    writer.WriteStartElement( "Keywords" );
                    writer.WriteAttributeString( "name", "keyword" + i + 1 );
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteStartElement( "LexerStyles" );
                writer.WriteStartElement( "LexerType" );
                writer.WriteAttributeString( "name", "Parsify" );
                writer.WriteAttributeString( "desc", "Parsify - Text parser" );
                writer.WriteAttributeString( "excluded", "no" );

                (string[] Background, string[] Foreground, string BoldFlag) colorPalette = HighlightingConfiguration.GetColorPalette( mode );
                //for ( int ps = 0; ps < presets.Length; ps++ )
                {
                    // comment preset name
                    // writer.WriteComment( presets[ ps ] );

                    for ( int i = 0; i < HighlightingConfiguration.NUMBER_OF_COLORS_PER_SET; i++ )
                    {
                        writer.WriteStartElement( "WordsStyle" );
                        writer.WriteAttributeString( "styleID", i.ToString() );
                        writer.WriteAttributeString( "name", ( i == 0 ? "Default" : "ColumnColor" + i.ToString() ) );
                        writer.WriteAttributeString( "fgColor", colorPalette.Foreground[ i ] );
                        writer.WriteAttributeString( "bgColor", colorPalette.Background[ i ] );
                        writer.WriteAttributeString( "fontName", "" );
                        writer.WriteAttributeString( "fontStyle", "0" );// colorPalette.BoldFlag );
                        writer.WriteEndElement();
                    }
                };

                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.Flush();
                writer.Close();
            }
        }
    }
}
