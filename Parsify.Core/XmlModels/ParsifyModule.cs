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

        [XmlElement( "Define" )]
        public List<ParsifyLine> LineDefinitions { get; set; }

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
                    return (ParsifyModule)serializer.Deserialize( xml );
                }
            }
            catch ( Exception ex )
            {
                MessageBox.Show(
                    $"Parsify error when trying to read app configuration \"{path}\". The XML-Definition might be corrupted.\r\n" +
                    $"In-depth reason:\r\n{ex.ToString()}",
                    "Parsify Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error );

                return null;
            }
        }

#if DEBUG
        public static void DebugCreateDefault( string name )
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer( typeof( ParsifyModule ) );

                using ( FileStream fs = new FileStream( "C:\\" + name, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None ) )
                using ( StreamWriter writer = new StreamWriter( fs ) )
                using ( XmlWriter xml = XmlWriter.Create( writer, new XmlWriterSettings() { Indent = true } ) )
                {
                    serializer.Serialize( xml, DebugGetDefault() );
                }
            }
            catch ( Exception ex )
            {
                MessageBox.Show( $"Error at DebugCreateDefault({name}): {ex.ToString()}" );
            }
        }

        private static ParsifyModule DebugGetDefault()
        {
            var mod = new ParsifyModule()
            {
                Name = "TestTextFormat",
                Version = "1.6",
                LineDefinitions = new List<ParsifyLine>()
            };

            mod.LineDefinitions.Add( new ParsifyLine() { StartsWithIdentifier = "HEAD", Fields = new List<ParsifyBaseField>() } );
            mod.LineDefinitions.Add( new ParsifyLine() { StartsWithIdentifier = "POS", Fields = new List<ParsifyBaseField>() } );

            mod.LineDefinitions[ 0 ].Fields.Add( new ParsifyPlain() { Name = "NOTE", Index = 4, Length = 10 } );
            mod.LineDefinitions[ 0 ].Fields.Add( new ParsifyPlain() { Name = "FLAG", Index = 14, Length = 2 } );

            mod.LineDefinitions[ 1 ].Fields.Add( new ParsifyPlain() { Name = "QUANTITY", Index = 3, Length = 2, DataType = "int" } );

            return mod;
        }
#endif
    }
}