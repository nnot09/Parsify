﻿using Kbg.NppPluginNET.PluginInfrastructure;
using Parsify.Core;
using Parsify.Core.Config;
using Parsify.Core.Core;
using Parsify.Core.Forms;
using Parsify.Core.Forms.NodeControls;
using Parsify.Core.Models;
using Parsify.Core.Models.Values;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Kbg.NppPluginNET
{
    public partial class frmCoreWindow : Form
    {
        public Document CurrentDocument { get; set; }
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

            if ( errorModules.Length > 0 )
                MessageBox.Show( $"Failed to load some XML modules:\r\n{errorModules}", "XML-Modules", MessageBoxButtons.OK, MessageBoxIcon.Warning );
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

        private void ApplySelectedModule( ParsifyModule module )
        {
            var documentParser = new DocumentParser( _scintilla, module );

            if ( !documentParser.Success )
            {
                MessageBox.Show( $"Document parsing failed:\r\n{documentParser.GetErrors()}", "Partially parsed", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            if ( documentParser.HasErrors )
                MessageBox.Show( $"Text document could not be parsed entirely:\r\n{documentParser.GetErrors()}", "Partially parsed", MessageBoxButtons.OK, MessageBoxIcon.Warning );

            this.CurrentDocument = documentParser.Document;
            
            GenerateTree();
        }

        private void GenerateTree()
        {
            this.treeDataView.Nodes.Add( this.CurrentDocument.ToString() );

            foreach ( var line in this.CurrentDocument.Lines )
            {
                var lineNode = new NodeLine( line );

                foreach ( var field in line.Fields )
                {
                    var fieldNode = new NodeField( field );
                    lineNode.Nodes.Add( fieldNode );
                }

                this.treeDataView.Nodes.Add( lineNode );
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

        private void comboTextFormats_SelectedIndexChanged( object sender, EventArgs e )
        {
            this.treeDataView.Nodes.Clear();

            if ( comboTextFormats.SelectedItem == null )
                return;

            ApplySelectedModule( comboTextFormats.SelectedItem as ParsifyModule );
        }

        private void treeDataView_NodeMouseClick( object sender, TreeNodeMouseClickEventArgs e )
        {
            if ( e.Node is NodeField field )
            {
                _scintilla.SelectFieldValue( field.DocumentField );

                ctxMenuItemShowOnlyLines.Visible = false;
                ctxMenuItemMarkAllLines.Visible = false;
                ctxMenuItemMarkSpecificOptions.Visible = true;
            }
            else if ( e.Node is NodeLine line )
            {
                // TODO More performant way
                var lineNoList = CurrentDocument.Lines
                    .Where( l => l.LineIdentifier == line.DocumentLine.LineIdentifier )
                    .Select( l => l.DocumentLineNumber );

                _scintilla.SelectLines( lineNoList );

                ctxMenuItemShowOnlyLines.Visible = true;
                ctxMenuItemMarkAllLines.Visible = true;
                ctxMenuItemMarkSpecificOptions.Visible = false;
            }
        }

        private void ctxMenuItemMarkSpecificOptionAllValues_Click( object sender, EventArgs e )
        {
            if ( this.treeDataView.SelectedNode == null )
            {
                return;
            }

            if ( this.treeDataView.SelectedNode is NodeField fieldNode )
            {
                //foreach ( var documentLines in _scintilla.ReadLines( fieldNode.Line.StartsWithIdentifier ) )
                //{

                //}
            }
        }
    }
}
