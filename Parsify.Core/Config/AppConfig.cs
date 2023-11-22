using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Parsify.Core.Config
{
    public class AppConfig
    {
        [XmlIgnore]
        public static string AppData = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData );

        [XmlIgnore]
        public static string AppConfigFileName = "Parsify.xml";

        [XmlIgnore]
        public static string AppConfigFullPath = Path.Combine( AppData, AppConfigFileName );

        [XmlElement( "AutoDetect" )]
        public bool AutoDetectTextFormat { get; set; }

        [XmlElement( "ModulesFilePath" )]
        public string ModulesDirectoryPath { get; set; }

        public static AppConfig LoadOrCreate()
        {
            if ( !File.Exists( AppConfigFullPath ) )
                return GenerateDefault();

            return Read();
        }

        public void Save()
        {
            // TODO: Appends extra char after save for some reason. seems to not be an encoding problem.
            try
            {
                XmlSerializer serializer = new XmlSerializer( typeof( AppConfig ) );

                using ( FileStream fs = new FileStream( AppConfig.AppConfigFullPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None ) )
                using ( StreamWriter writer = new StreamWriter( fs ) )
                using ( XmlWriter xml = XmlWriter.Create( writer, new XmlWriterSettings() { Indent = true } ) )
                {
                    serializer.Serialize( xml, this );
                }
            }
            catch ( Exception ex )
            {
                // TODO
                MessageBox.Show( "Save: " + ex.ToString() );
            }
        }

        private static AppConfig Read()
        {
            XmlSerializer serializer = new XmlSerializer( typeof( AppConfig ) );

            try
            {
                using ( FileStream fs = new FileStream( AppConfig.AppConfigFullPath, FileMode.Open, FileAccess.Read, FileShare.Read ) )
                using ( StreamReader reader = new StreamReader( fs ) )
                using ( XmlReader xml = XmlReader.Create( reader ) )
                {
                    var appConfig = ( AppConfig )serializer.Deserialize( xml );
                    return appConfig;
                }
            }
            catch ( Exception ex )
            {
#if DEBUG
                MessageBox.Show(
                    "Parsify error when trying to read app configuration: " + ex.ToString(),
                    "Parsify Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error );

                return GenerateDefault();
#else
                MessageBox.Show(
                    "Parsify app configuration seems to be corrupted. Restoring defaults.",
                    "Parsify Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error );

                return GenerateDefault();
#endif
            }
        }

        private static AppConfig GenerateDefault()
        {
            try
            {
                if ( File.Exists( AppConfigFullPath ) )
                    File.Delete( AppConfigFullPath );
            }
            catch 
            {
                MessageBox.Show( $"Parsify restoration has failed. Please delete \"{AppConfigFullPath}\" and restart Notepad++. Then try again." );
            }

            var appConfig = GetDefault();
            Directory.CreateDirectory( appConfig.ModulesDirectoryPath );
            appConfig.Save();

            return appConfig;
        }
        private static AppConfig GetDefault()
            => new AppConfig()
            {
                AutoDetectTextFormat = false,
                ModulesDirectoryPath = @"C:\Parsify\Modules"
            };
    }
}
