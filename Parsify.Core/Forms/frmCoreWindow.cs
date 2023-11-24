using Kbg.NppPluginNET.PluginInfrastructure;
using Parsify.Core;
using Parsify.Core.Config;
using Parsify.Core.Forms;
using Parsify.Core.Forms.NodeControls;
using Parsify.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
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
        private Scintilla _scintilla;

        public frmCoreWindow()
        {
            InitializeComponent();

            this._moduleDefinitions = new List<ParsifyModule>();
            this.Configuration = AppConfig.LoadOrCreate();
            this._scintilla = new Scintilla();

            ParsifyModule.DebugCreateDefault( "text.xml", Parsify.Core.Models.TextFormat.Plain );
            //ParsifyModule.DebugCreateDefault( "csv.xml", Parsify.Core.Models.TextFormat.Csv );

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

        protected override void OnHandleCreated( EventArgs e )
        {
            /*
             * Notepad++ may freezes if the plugins control is docked out and loosing focus
             *  - https://github.com/notepad-plus-plus/notepad-plus-plus/issues/8723
             *  - https://forums.codeguru.com/showthread.php?412421-endless-messages-WM_GETDLGCODE-when-focus-is-lost
             *  - http://robs-got-a-blog.blogspot.com/2011/09/net-custom-control-locks-up-when-losing.html
             *  - https://social.msdn.microsoft.com/Forums/en-US/dca98588-039a-44f8-b15c-86fd9ac415c6/how-can-i-resolve-datagridview-and-cdialog-bug?forum=winformsdatacontrols
             *  Fix: Set WS_EX_CONTROLPARENT to elemens which may are in focus
             */

            this.UpdateWindowStyles();

            base.OnHandleCreated( e );
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

        private void ParseFile( ParsifyModule module )
        {
            this.treeDataView.Nodes.Add( $"{module.Name} ({module.Version})" );

            foreach ( var def in module.TextLineDefinitions )
            {
                foreach ( var line in _scintilla.ReadLines( def.Name ) )
                {
                    var lineNode = new NodeLine( def, line.LineNo );
                    this.treeDataView.Nodes.Add( lineNode );

                    foreach ( var field in def.Fields )
                    {
                        if ( field is Plain plain )
                            field.Value = Extensions.GetField( line.Line, plain.Index, plain.Length );
                        else
                            throw new NotImplementedException( "csv" );

                        var fieldNode = new NodeField( def, field );
                        lineNode.Nodes.Add( fieldNode );
                    }
                }
            }
        }

        private void comboTextFormats_SelectedIndexChanged( object sender, EventArgs e )
        {
            this.treeDataView.Nodes.Clear();

            if ( comboTextFormats.SelectedItem == null )
                return;

            ParseFile( comboTextFormats.SelectedItem as ParsifyModule );
        }

        private void treeDataView_NodeMouseClick( object sender, TreeNodeMouseClickEventArgs e )
        {
            if ( e.Node is NodeField field )
            {
                if ( field.Field is Plain plain )
                {
                    _scintilla.SelectFieldValue( ( field.Parent as NodeLine ).DocumentLineNo, plain.Index, plain.Length );
                }

                ctxMenuItemShowOnlyLines.Visible = false;
                ctxMenuItemMarkAllLines.Visible = false;
                ctxMenuItemMarkSpecificOptions.Visible = true;
            }
            else if ( e.Node is NodeLine line )
            {
                // TODO More performant way
                var lineNoList = _scintilla.ReadLines( line.ParsifyLine.Name ).Select( l => l.LineNo );

                _scintilla.SelectLines( lineNoList );

                ctxMenuItemShowOnlyLines.Visible = true;
                ctxMenuItemMarkAllLines.Visible = true;
                ctxMenuItemMarkSpecificOptions.Visible = false;
            }
        }
    }
}
