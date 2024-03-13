using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Parsify.XmlModels
{
    public class ParsifyModule
    {
        [XmlElement( "Name" )]
        public string Name { get; set; }

        [XmlElement( "Version" )]
        public string Version { get; set; }

        [XmlElement( "Line" )]
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

            mod.LineDefinitions.Add( new ParsifyLine() { StartsWithIdentifier = "HEAD", Fields = new List<ParsifyField>() } );
            mod.LineDefinitions.Add( new ParsifyLine() { StartsWithIdentifier = "POS", Fields = new List<ParsifyField>() } );

            // HEAD
            mod.LineDefinitions[ 0 ].Fields.Add( new ParsifyField() { Name = "NOTE", Position = 5, Length = 10 } );
            mod.LineDefinitions[ 0 ].Fields.Add( new ParsifyField() { Name = "FLAG", Position = 15, Length = 2 } );

            // POS
            var posQtyField = new ParsifyField() { Name = "QUANTITY", Position = 4, Length = 2, DataType = "int" };
            mod.LineDefinitions[ 1 ].Fields.Add( posQtyField );
            posQtyField.Translations = new List<ParsifyFieldValueTranslate>()
            {
                new ParsifyFieldValueTranslate()
                {
                    Value = "4",
                    DisplayValue = "answer to everything",
                    IgnoreCase = true,
                    SearchMode = ParsifyFieldValueTranslateSearchMode.Contains
                },
                new ParsifyFieldValueTranslate()
                {
                    Value = "43",
                    DisplayValue = "missed the answer",
                    SearchMode = ParsifyFieldValueTranslateSearchMode.StartsWith
                }
            };

            return mod;
        }
#endif
    }
}