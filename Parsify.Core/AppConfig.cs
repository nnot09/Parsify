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

        [XmlElement( "HighlightingMode" )]
        public AppHighlightingMode HighlightingMode { get; set; }

        [XmlElement("HighlightingColor")]
        public uint Color { get; set; }

        [XmlElement("HighlightingTransparency")]
        public int Transparency { get; set; }

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
                MessageBox.Show( $"Failed to save app configuration at \"{AppConfig.AppConfigFullPath}\".\r\n" +
                    $"Please try to restart Notepad++ and try again.\r\n" +
                    $"Make sure you have necessary permissions to write on that path.\r\n" +
                    $"If the file already exists, delete it and try again.\r\n" +
                    $"Close any applications that might block this config file.\r\n" +
                    $"If this problem persists, please create an issue on the Github repository where Parsify is maintained.\r\n" +
                    "-----------------\r\n" +
                    $"In-Depth reason:\r\n{ex.ToString()}", "Parsify Start", MessageBoxButtons.OK, MessageBoxIcon.Information ); ;
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
                    var appConfig = (AppConfig)serializer.Deserialize( xml );
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
                MessageBox.Show(
                    $"Parsify restoration has failed. Please delete \"{AppConfigFullPath}\" and restart Notepad++. Then try again.",
                    "Parsify",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning );
            }

            var appConfig = GetDefault();
            Directory.CreateDirectory( appConfig.ModulesDirectoryPath );
            appConfig.Save();

            return appConfig;
        }
        private static AppConfig GetDefault()
            => new AppConfig()
            {
                ModulesDirectoryPath = @"C:\Parsify\Modules",
                Color = 0xff2d8fcc,
                HighlightingMode = AppHighlightingMode.Default,
                Transparency = 50
            };
    }
}
