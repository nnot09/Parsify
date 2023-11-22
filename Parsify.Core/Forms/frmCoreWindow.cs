using Parsify.Core.Config;
using Parsify.Core.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Kbg.NppPluginNET
{
    public partial class frmCoreWindow : Form
    {
        public AppConfig Configuration { get; set; }

        private List<ParsifyModule> _moduleDefinitions;

        public frmCoreWindow()
        {
            InitializeComponent();

            this._moduleDefinitions = new List<ParsifyModule>();
            this.Configuration = AppConfig.LoadOrCreate();

            ParsifyModule.DebugCreateDefault( "text.xml", Parsify.Core.Models.TextFormat.Plain );
            ParsifyModule.DebugCreateDefault( "csv.xml", Parsify.Core.Models.TextFormat.Csv );

            this.Enabled = false;

            try
            {
                UpdateModulesList();
            }
            finally
            {
                this.Enabled = true;
            }
        }

        private void btnOpenConfig_Click( object sender, EventArgs e )
        {
            using ( frmConfig configWindow = new frmConfig( this.Configuration ) )
            {
                if ( configWindow.ShowDialog() == DialogResult.OK )
                {
                    this.Configuration.Save();
                }
            }
        }

        private void btnUpdateModules_Click( object sender, EventArgs e )
        {
            this.Enabled = false;

            try
            {
                UpdateModulesList();
            }
            finally
            {
                this.Enabled = true;
            }
        }

        private void UpdateModulesList()
        {
            StringBuilder errorModules = new StringBuilder();

            // Not sure about thread safety here (idk how NPP handles this). this might be reduntant
            // ok lock() doesnt work
            _moduleDefinitions.Clear();

            foreach ( var module in Directory.GetFiles( this.Configuration.ModulesDirectoryPath, "*.xml", SearchOption.TopDirectoryOnly ) )
            {
                var parsifyModuleDef = ParsifyModule.Load( module );

                if ( parsifyModuleDef == null )
                {
                    errorModules.AppendLine( module );
                    continue;
                }

                _moduleDefinitions.Add( parsifyModuleDef );
            }

            SyncModulesList();
        }

        private void SyncModulesList()
        {
            // TODO Don't forget currently selected format so we can re-select when available

            this.comboTextFormats.Items.Clear();

            foreach ( var module in _moduleDefinitions )
            {
                this.comboTextFormats.Items.Add( module );
            }
        }

        private void PrintModule( ParsifyModule module )
        {
            this.treeDataView.Nodes.Add( $"{module.Name} ({module.Version})" );

            foreach ( var line in module.TextLineDefinitions )
            {
                var lineNode = this.treeDataView.Nodes.Add( line.Name );

                foreach ( var field in line.Fields )
                {
                    var fieldNode = lineNode.Nodes.Add( field.Name + ": unknown" );
                }
            }
        }

        private void comboTextFormats_SelectedIndexChanged( object sender, EventArgs e )
        {
            if ( comboTextFormats.SelectedItem == null )
            {
                this.treeDataView.Nodes.Clear();
                return;
            }

            PrintModule( comboTextFormats.SelectedItem as ParsifyModule );
        }
    }
}
