using Parsify;
using Parsify.Core;
using Parsify.Forms;
using Parsify.Forms.NodeControls;
using Parsify.Models;
using Parsify.Other;
using Parsify.PluginInfrastructure;
using Parsify.XmlModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kbg.NppPluginNET
{
    public partial class frmCoreWindow : Form
    {
        private List<ParsifyModule> _moduleDefinitions;
        private bool _toggleShowLines;

        public frmCoreWindow()
        {
            InitializeComponent();
            Main.DocumentFactory.DocumentParseFailedEvent += this.DocumentFactory_DocumentParseFailedEvent;
            Main.DocumentFactory.DocumentInitializedEvent += this.DocumentFactory_DocumentInitializedEvent;
            Main.DocumentFactory.DocumentParserChangedEvent += this.DocumentFactory_DocumentParserChangedEvent;
            Main.DocumentFactory.DocumentChanged += this.DocumentFactory_DocumentChanged;

            this._moduleDefinitions = new List<ParsifyModule>();
#if DEBUG
            ParsifyModule.DebugCreateDefault( "text.xml" );
#endif
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

        private void DocumentFactory_DocumentChanged( object sender, EventArgs e )
        {
            footerlbTotalLinesCount.Text = $"Total Lines: n/a";
            footerlbSelectedCount.Text = string.Empty;

            this.treeDataView.Nodes.Clear();
            this.comboTextFormats.SelectedItem = null;
            this.comboTextFormats.SelectedItem = Main.DocumentFactory.DocumentParserCache.GetCachedModuleOrNull( Main.Scintilla.CurrentDocumentHash() );
        }

        private void DocumentFactory_DocumentParserChangedEvent( object sender, Parsify.Models.Events.EArgs.DocumentParserChangedEventArgs e )
        {
            GenerateTree();
            UpdateStatusBar();
        }

        private void DocumentFactory_DocumentInitializedEvent( object sender, Parsify.Models.Events.EArgs.DocumentInitializedEventArgs e )
        {
            GenerateTree();
            UpdateStatusBar();
        }

        private void DocumentFactory_DocumentParseFailedEvent( object sender, Parsify.Models.Events.EArgs.DocumentParseFailedEventArgs e )
        {
            MessageBox.Show( $"Document parsing failed:\r\n{e.ErrorText}", "Partially parsed", MessageBoxButtons.OK, MessageBoxIcon.Error );
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

        private void UpdateModulesList()
        {
            StringBuilder errorModules = new StringBuilder();

            _moduleDefinitions.Clear();

            foreach ( var module in Directory.GetFiles( Main.Configuration.ModulesDirectoryPath, "*.xml", SearchOption.TopDirectoryOnly ) )
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
            ParsifyModule currentlySelected = null;
            if ( this.comboTextFormats.SelectedItem != null )
                currentlySelected = (ParsifyModule)this.comboTextFormats.SelectedItem;

            this.comboTextFormats.Items.Clear();

            foreach ( var module in _moduleDefinitions )
                this.comboTextFormats.Items.Add( module );

            if ( currentlySelected != null )
            {
                this.comboTextFormats.SelectedItem = this.comboTextFormats.Items
                    .Cast<ParsifyModule>()
                    .FirstOrDefault( mod => mod.Name == currentlySelected.Name && mod.Version == currentlySelected.Version );
            }
        }

        private void GenerateTree()
        {
            this.treeDataView.Nodes.Clear();
            this.treeDataView.Nodes.Add( Main.DocumentFactory.Active.ToString() );

            foreach ( var line in Main.DocumentFactory.Active.Lines )
            {
                var lineNode = new NodeLine( line );

                foreach ( var field in line.Fields )
                {
                    var fieldNode = new NodeField( field );
                    lineNode.Nodes.Add( fieldNode );
                    this.treeDataView.UpdateNodeTooltip( fieldNode );
                }

                this.treeDataView.Nodes.Add( lineNode );
            }
        }

        private void UpdateStatusBar()
        {
            footerlbTotalLinesCount.Text = $"Total Lines: {Main.DocumentFactory.Active.Lines.Count}";

            if ( this.treeDataView.SelectedNode != null )
            {
                if ( this.treeDataView.SelectedNode is NodeField field )
                {
                    int sameValuesCount = GetSameValuesCountPlainTextFormat( field );
                    footerlbSelectedCount.Text = $"Selected same values: {sameValuesCount}";
                }
                else if ( this.treeDataView.SelectedNode is NodeLine line )
                {
                    int sameLinesCount = GetPlainTextSelectedCount( line );
                    footerlbSelectedCount.Text = $"{( line.DocumentLine as TextLine ).LineIdentifier} Count: {sameLinesCount}";
                }
            }

            footerlbParsifyErrorsCount.Text = $"Parsify: {Main.DocumentFactory.DocumentReader.NumberOfErrors} Errors";
        }

        private int GetPlainTextSelectedCount( NodeLine line )
        {
            string identifier = ( line.DocumentLine as TextLine ).LineIdentifier;

            return Main.DocumentFactory.Active.Lines
                        .Cast<TextLine>()
                        .Where( l => l.LineIdentifier == identifier )
                        .Count();
        }

        private int GetSameValuesCountPlainTextFormat( NodeField field )
        {
            string identifier = ( field.DocumentField.Parent as TextLine ).LineIdentifier;

            var sameValuesCount = Main.DocumentFactory.Active.Lines
                .Cast<TextLine>()
                .Where( l => l.LineIdentifier == identifier )
                .SelectMany( l => l.Fields )
                .Where( f => f.Name == field.DocumentField.Name && f.Value == field.DocumentField.Value )
                .Count();

            return sameValuesCount;
        }

        private void btnOpenConfig_Click( object sender, EventArgs e )
        {

        }

        private void btnUpdateModules_Click( object sender, EventArgs e )
        {
            this.btnUpdateModules.Enabled = false;

            try
            {
                UpdateModulesList();
            }
            finally
            {
                this.btnUpdateModules.Enabled = true;
            }
        }

        private void comboTextFormats_SelectedIndexChanged( object sender, EventArgs e )
        {
            this.treeDataView.Nodes.Clear();

            if ( comboTextFormats.SelectedItem == null )
            {
                btnHighlightSwitch.Enabled = false;
                return;
            }

            btnHighlightSwitch.Enabled = true;

            Main.DocumentFactory.UpdateParser( comboTextFormats.SelectedItem as ParsifyModule );
        }

        private void ctxMenuItemMarkSpecificOptionAllValues_Click( object sender, EventArgs e )
        {
            if ( this.treeDataView.SelectedNode == null )
            {
                return;
            }

            if ( this.treeDataView.SelectedNode is NodeField fieldNode )
            {
                Main.Scintilla.SelectMultiplePlainFieldValues(
                    Main.DocumentFactory.Active.Lines.Cast<TextLine>(),
                    fieldNode.DocumentField
                );
            }
        }

        private void ctxMenuItemMarkSpecificOptionValue_Click( object sender, EventArgs e )
        {
            if ( this.treeDataView.SelectedNode == null )
            {
                return;
            }

            if ( this.treeDataView.SelectedNode is NodeField fieldNode )
                Main.Scintilla.SelectFieldValue( fieldNode.DocumentField );
        }

        private void ctxMenuItemShowOnlyLines_Click( object sender, EventArgs e )
        {
            if ( this.treeDataView.SelectedNode == null )
            {
                return;
            }

            if ( this.treeDataView.SelectedNode is NodeLine lineNode )
            {
                if ( _toggleShowLines )
                {
                    ctxMenuItemShowOnlyLines.Text = "Show only selected line identifier";

                    Main.Scintilla.UnhideAll( Main.DocumentFactory.Active.Lines.Cast<TextLine>() );
                }
                else
                {
                    ctxMenuItemShowOnlyLines.Text = "Show all lines";

                    // TODO More performant way
                    var lineNoList = Main.DocumentFactory.Active.Lines
                        .Cast<TextLine>()
                        .Where( l => l.LineIdentifier != ( lineNode.DocumentLine as TextLine ).LineIdentifier )
                        .Select( l => l.DocumentLineNumber );

                    Main.Scintilla.HideLines( lineNoList );
                }

                _toggleShowLines = !_toggleShowLines;
            }
        }

        private void ctxMenuItemMarkAllLines_Click( object sender, EventArgs e )
        {
            if ( this.treeDataView.SelectedNode == null )
            {
                return;
            }

            if ( this.treeDataView.SelectedNode is NodeLine lineNode )
            {
                // TODO More performant way
                var lineNoList = Main.DocumentFactory.Active.Lines
                    .Cast<TextLine>()
                    .Where( l => l.LineIdentifier == ( lineNode.DocumentLine as TextLine ).LineIdentifier )
                    .Select( l => l.DocumentLineNumber );

                Main.Scintilla.SelectLines( lineNoList );
            }
        }

        private void treeDataView_AfterSelect( object sender, TreeViewEventArgs e )
        {
            if ( e.Node == null ) return;

            UpdateStatusBar();

            if ( e.Node is NodeField field )
            {
                if ( !field.DocumentField.Success )
                {
                    Main.Scintilla.ClearSelect();
                    return;
                }

                Main.Scintilla.SelectFieldValue( field.DocumentField );

                ctxMenuItemShowOnlyColumnType.Visible = false;
                ctxMenuItemShowOnlyLines.Visible = false;
                ctxMenuItemMarkAllLines.Visible = false;
                ctxMenuItemMarkAllIdenticalLines.Visible = false;
                ctxMenuItemMarkSpecificOptions.Visible = true;
            }
            else if ( e.Node is NodeLine line )
            {
                var lineNoList = Main.DocumentFactory.Active.Lines
                    .Cast<TextLine>()
                    .Where( l => l.LineIdentifier == ( line.DocumentLine as TextLine ).LineIdentifier )
                    .Select( l => l.DocumentLineNumber );


                Main.Scintilla.SelectLines( lineNoList );

                ctxMenuItemShowOnlyLines.Visible = true;
                ctxMenuItemMarkAllLines.Visible = true;
                ctxMenuItemMarkAllIdenticalLines.Visible = true;
                ctxMenuItemMarkSpecificOptions.Visible = false;
                ctxMenuItemShowOnlyColumnType.Visible = false;
            }
        }

        private void btnHighlightSwitch_Click( object sender, EventArgs e )
        {
            Main.Scintilla.SwitchLanguage();
        }

        private void footerlbParsifyErrorsCount_Click( object sender, EventArgs e )
        {
            if ( Main.DocumentFactory.DocumentReader.NumberOfErrors == 0 )
                return;

            using ( frmParseErrMessages errMsgWnd = new frmParseErrMessages( Main.DocumentFactory.DocumentReader.GetErrors() ) )
                errMsgWnd.ShowDialog();
        }
    }
}
