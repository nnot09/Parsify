using Parsify.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
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

        [XmlElement( "Define" )]
        public List<ParsifyLine> TextLineDefinitions { get; set; }

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

            mod.TextLineDefinitions.Add( new ParsifyLine() { Name = "HEAD", Fields = new List<BaseField>() } );
            mod.TextLineDefinitions.Add( new ParsifyLine() { Name = "POS", Fields = new List<BaseField>() } );

            mod.TextLineDefinitions[ 0 ].Fields.Add( new Plain() { Name = "NOTE", Index = 4, Length = 10, DataType = "string" } );
            mod.TextLineDefinitions[ 0 ].Fields.Add( new Plain() { Name = "FLAG", Index = 14, Length = 2, DataType = "string" } );

            mod.TextLineDefinitions[ 1 ].Fields.Add( new Plain() { Name = "QUANTITY", Index = 3, Length = 2, DataType = "int" } );

            return mod;
        }

        private static ParsifyModule DebugGetDefaultCsv()
        {
            var mod = new ParsifyModule()
            {
                Name = "TestCsvFormat",
                Version = "2.5",
                TextFormat = TextFormat.Csv,
                TextLineDefinitions = new List<ParsifyLine>()
            };

            mod.TextLineDefinitions.Add( new ParsifyLine() { Name = "Order", Fields = new List<BaseField>(), CsvSplitDelimeter = "," } );

            mod.TextLineDefinitions[ 0 ].Fields.Add( new Csv() { Name = "OrderId", DataType = "string" } );
            mod.TextLineDefinitions[ 0 ].Fields.Add( new Csv() { Name = "OrderDescription" } );

            return mod;
        }

        public override string ToString()
            => $"{this.Name} ({this.Version})";
    }
}