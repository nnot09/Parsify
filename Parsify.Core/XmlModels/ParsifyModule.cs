using Parsify.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Parsify.Core.Config
{
    public class ParsifyModule
    {
        [XmlElement( "Name" )]
        public string Name { get; set; }

        [XmlElement( "Version" )]
        public string Version { get; set; }

        [XmlElement( "TextFormat" )]
        public TextFormat TextFormat { get; set; }

        [XmlElement( "CsvSplitDelimeter" )]
        public string CsvSplitDelimeter { get; set; }

        [XmlElement("HasCsvTableHeader")]
        public bool HasTableHeader { get; set; }

        [XmlElement("CommentLineIdentifier")]
        public string CommentLineIdentifier { get; set; }

        [XmlElement( "Define" )]
        public List<ParsifyLine> TextLineDefinitions { get; set; }

        public override string ToString()
            => $"{Name} ({Version})";

        public static ParsifyModule Load( string path )
        {
            XmlSerializer serializer = new XmlSerializer( typeof( ParsifyModule ) );

            try
            {
                using ( FileStream fs = new FileStream( path, FileMode.Open, FileAccess.Read, FileShare.Read ) )
                using ( StreamReader reader = new StreamReader( fs ) )
                using ( XmlReader xml = XmlReader.Create( reader ) )
                {
                    return ( ParsifyModule )serializer.Deserialize( xml );
                }
            }
            catch ( Exception ex )
            {
                MessageBox.Show(
                    "Parsify error when trying to read app configuration: " + ex.ToString(),
                    "Parsify Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error );

                return null;
            }
        }

        // TODO Remove
        public static void DebugCreateDefault( string name, TextFormat type )
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer( typeof( ParsifyModule ) );

                using ( FileStream fs = new FileStream( "C:\\" + name, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None ) )
                using ( StreamWriter writer = new StreamWriter( fs ) )
                using ( XmlWriter xml = XmlWriter.Create( writer, new XmlWriterSettings() { Indent = true } ) )
                {
                    if ( type == TextFormat.Plain )
                        serializer.Serialize( xml, DebugGetDefault() );
                    else
                        serializer.Serialize( xml, DebugGetDefaultCsv() );
                }
            }
            catch (Exception ex)
            {
            }
        }

        // TODO Remove
        private static ParsifyModule DebugGetDefault()
        {
            var mod = new ParsifyModule()
            {
                Name = "TestTextFormat",
                Version = "1.6",
                TextFormat = TextFormat.Plain,
                TextLineDefinitions = new List<ParsifyLine>()
            };

            mod.TextLineDefinitions.Add( new ParsifyLine() { StartsWithIdentifier = "HEAD", Fields = new List<ParsifyBaseField>() } );
            mod.TextLineDefinitions.Add( new ParsifyLine() { StartsWithIdentifier = "POS", Fields = new List<ParsifyBaseField>() } );

            mod.TextLineDefinitions[ 0 ].Fields.Add( new ParsifyPlain() { Name = "NOTE", Index = 4, Length = 10 } );
            mod.TextLineDefinitions[ 0 ].Fields.Add( new ParsifyPlain() { Name = "FLAG", Index = 14, Length = 2 } );

            mod.TextLineDefinitions[ 1 ].Fields.Add( new ParsifyPlain() { Name = "QUANTITY", Index = 3, Length = 2, DataType = "int" } );

            return mod;
        }

        private static ParsifyModule DebugGetDefaultCsv()
        {
            var mod = new ParsifyModule()
            {
                Name = "TestCsvFormat",
                Version = "2.5",
                TextFormat = TextFormat.Csv,
                TextLineDefinitions = new List<ParsifyLine>(),
                CsvSplitDelimeter = ";",
                HasTableHeader = true, 
                CommentLineIdentifier = "<"
            };

            mod.TextLineDefinitions.Add( new ParsifyLine() { StartsWithIdentifier = "Order", Fields = new List<ParsifyBaseField>() } );

            mod.TextLineDefinitions[ 0 ].Fields.Add( new ParsifyCsv() { Name = "OrderId", DataType = "string" } );
            mod.TextLineDefinitions[ 0 ].Fields.Add( new ParsifyCsv() { Name = "OrderDescription" } );

            return mod;
        }
    }
}