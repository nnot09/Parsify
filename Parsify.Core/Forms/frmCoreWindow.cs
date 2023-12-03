using Kbg.NppPluginNET.PluginInfrastructure;
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
using System.Security.Policy;
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
        private bool _toggleShowLines;
        private int _errorsCount;

        public frmCoreWindow()
        {
            InitializeComponent();

            this._moduleDefinitions = new List<ParsifyModule>();
            this.Configuration = AppConfig.LoadOrCreate();
            this._scintilla = new Scintilla();

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

        private void ApplySelectedModule( ParsifyModule module )
        {
            var documentParser = new DocumentParser( _scintilla, module );

            if ( !documentParser.Success )
            {
                MessageBox.Show( $"Document parsing failed:\r\n{documentParser.GetErrors()}", "Partially parsed", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            if ( documentParser.NumberOfErrors > 0 )
                MessageBox.Show( $"Text document could not be parsed entirely:\r\n{documentParser.GetErrors()}", "Partially parsed", MessageBoxButtons.OK, MessageBoxIcon.Warning );

            this.CurrentDocument = documentParser.Document;
            this._errorsCount = documentParser.NumberOfErrors;

            GenerateTree();
            UpdateStatusBar();
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
                    this.treeDataView.UpdateNodeTooltip( fieldNode );
                }

                this.treeDataView.Nodes.Add( lineNode );
            }
        }

        private void UpdateStatusBar()
        {
            footerlbTotalLinesCount.Text = $"Total Lines: {CurrentDocument.Lines.Count}";

            if ( this.treeDataView.SelectedNode != null )
            {
                if ( this.treeDataView.SelectedNode is NodeField field )
                {
                    int sameValuesCount = 0;

                    if ( CurrentDocument.TextFormat == TextFormat.Csv )
                        sameValuesCount = GetSameValuesCountCsv( field );
                    else if ( CurrentDocument.TextFormat == TextFormat.Plain )
                        sameValuesCount = GetSameValuesCountPlainTextFormat( field );

                    footerlbSelectedCount.Text = $"Selected same values: {sameValuesCount}";
                }
                else if ( this.treeDataView.SelectedNode is NodeLine line )
                {
                    int sameLinesCount = 0;

                    if ( CurrentDocument.TextFormat == TextFormat.Csv )
                    {
                        // Since it's Csv the best and logical choice would be to count lines with identical field values
                        sameLinesCount = GetIdenticalLinesCount( line );

                        footerlbSelectedCount.Text = $"Identical Lines Count: {sameLinesCount}";
                    }
                    else if ( CurrentDocument.TextFormat == TextFormat.Plain )
                    {
                        sameLinesCount = GetPlainTextSelectedCount( line );

                        footerlbSelectedCount.Text = $"{( line.DocumentLine as PlainTextLine ).LineIdentifier} Count: {sameLinesCount}";
                    }
                }
            }

            footerlbParsifyErrorsCount.Text = $"Parsify: {_errorsCount} Errors";
        }

        private int GetPlainTextSelectedCount( NodeLine line )
        {
            string identifier = ( line.DocumentLine as PlainTextLine ).LineIdentifier;

            return CurrentDocument.Lines
                        .Cast<PlainTextLine>()
                        .Where( l => l.LineIdentifier == identifier )
                        .Count();
        }

        private int GetSameValuesCountPlainTextFormat( NodeField field )
        {
            string identifier = ( field.DocumentField.Parent as PlainTextLine ).LineIdentifier;

            var sameValuesCount = CurrentDocument.Lines
                .Cast<PlainTextLine>()
                .Where( l => l.LineIdentifier == identifier )
                .SelectMany( l => l.Fields )
                .Where( f => f.Name == field.DocumentField.Name && f.Value == field.DocumentField.Value )
                .Count();

            return sameValuesCount;
        }

        private int GetIdenticalLinesCount( NodeLine line )
        {
            if ( ( line.DocumentLine as CsvLine ).IsHeader )
                return 1;

            int identicalLinesCount = 1;

            for ( int lineIndex = 0; lineIndex < CurrentDocument.Lines.Count; lineIndex++ )
            {
                bool isIdentical = true;
                var currentLine = CurrentDocument.Lines[ lineIndex ];

                if ( ( currentLine as CsvLine ).IsHeader )
                    continue;

                if ( line.DocumentLine.DocumentLineNumber == currentLine.DocumentLineNumber )
                    continue;

                if ( currentLine.Fields.Count != line.DocumentLine.Fields.Count )
                    continue;

                for ( int fieldIndex = 0; fieldIndex < currentLine.Fields.Count; fieldIndex++ )
                {
                    var currentField = currentLine.Fields[ fieldIndex ];
                    var dataField = line.DocumentLine.Fields.ElementAtOrDefault( fieldIndex );


                    if ( dataField == null )
                    {
                        isIdentical = false;
                        break;
                    }

                    if ( currentField.Name != dataField.Name || currentField.Value != dataField.Value )
                    {
                        isIdentical = false;
                        break;
                    }
                }

                if ( isIdentical )
                {
                    identicalLinesCount++;
                }
            }

            return identicalLinesCount;
        }

        private int GetSameValuesCountCsv( NodeField field )
        {
            return CurrentDocument.Lines
                .SelectMany( l => l.Fields )
                .Where( f => f.Name == field.DocumentField.Name && f.Value == field.DocumentField.Value )
                .Count();
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

        private void comboTextFormats_SelectedIndexChanged( object sender, EventArgs e )
        {
            this.treeDataView.Nodes.Clear();

            if ( comboTextFormats.SelectedItem == null )
                return;

            ApplySelectedModule( comboTextFormats.SelectedItem as ParsifyModule );
        }

        private void ctxMenuItemMarkSpecificOptionAllValues_Click( object sender, EventArgs e )
        {
            if ( this.treeDataView.SelectedNode == null )
            {
                return;
            }

            if ( this.treeDataView.SelectedNode is NodeField fieldNode )
            {
                if ( CurrentDocument.TextFormat == TextFormat.Csv )
                {
                    _scintilla.SelectMultipleCsvFieldValues(
                        CurrentDocument.Lines.Cast<CsvLine>(),
                        fieldNode.DocumentField
                    );
                }
                else if ( CurrentDocument.TextFormat == TextFormat.Plain )
                {
                    _scintilla.SelectMultiplePlainFieldValues(
                        CurrentDocument.Lines.Cast<PlainTextLine>(),
                        fieldNode.DocumentField
                    );
                }
            }
        }

        private void ctxMenuItemMarkSpecificOptionValue_Click( object sender, EventArgs e )
        {
            if ( this.treeDataView.SelectedNode == null )
            {
                return;
            }

            if ( this.treeDataView.SelectedNode is NodeField fieldNode )
            {
                if ( CurrentDocument.TextFormat == TextFormat.Csv )
                {
                    _scintilla.SelectFieldValue( fieldNode.DocumentField );
                }
                else if ( CurrentDocument.TextFormat == TextFormat.Plain )
                {
                    _scintilla.SelectFieldValue( fieldNode.DocumentField );
                }
            }
        }

        // TODO ShowOnlyColumns variant for Csv
        private void ctxMenuItemShowOnlyLines_Click( object sender, EventArgs e )
        {
            if ( this.treeDataView.SelectedNode == null )
            {
                return;
            }

            if ( CurrentDocument.TextFormat != TextFormat.Plain )
                return;

            if ( this.treeDataView.SelectedNode is NodeLine lineNode )
            {
                if ( _toggleShowLines )
                {
                    ctxMenuItemShowOnlyLines.Text = "Show only selected line type";

                    _scintilla.UnhideAll( CurrentDocument.Lines.Cast<PlainTextLine>() );
                }
                else
                {
                    ctxMenuItemShowOnlyLines.Text = "Show all lines";

                    // TODO More performant way
                    var lineNoList = CurrentDocument.Lines
                        .Cast<PlainTextLine>()
                        .Where( l => l.LineIdentifier != ( lineNode.DocumentLine as PlainTextLine ).LineIdentifier )
                        .Select( l => l.DocumentLineNumber );

                    _scintilla.HideLines( lineNoList );
                }

                _toggleShowLines = !_toggleShowLines;
            }
        }

        // TODO MarkAllColumns variant for Csv
        private void ctxMenuItemMarkAllLines_Click( object sender, EventArgs e )
        {
            if ( this.treeDataView.SelectedNode == null )
            {
                return;
            }

            if ( CurrentDocument.TextFormat != TextFormat.Plain )
                return;

            if ( this.treeDataView.SelectedNode is NodeLine lineNode )
            {
                // TODO More performant way
                var lineNoList = CurrentDocument.Lines
                    .Cast<PlainTextLine>()
                    .Where( l => l.LineIdentifier == ( lineNode.DocumentLine as PlainTextLine ).LineIdentifier )
                    .Select( l => l.DocumentLineNumber );

                _scintilla.SelectLines( lineNoList );
            }
        }

        private void treeDataView_AfterSelect( object sender, TreeViewEventArgs e )
        {
            if ( e.Node == null ) return;
         
            UpdateStatusBar();

            if ( e.Node is NodeField field )
            {
                if ( CurrentDocument.TextFormat == TextFormat.Csv )
                {
                    _scintilla.SelectFieldValue( field.DocumentField );
                }
                else if ( CurrentDocument.TextFormat == TextFormat.Plain )
                {
                    _scintilla.SelectFieldValue( field.DocumentField );
                }

                ctxMenuItemShowOnlyLines.Visible = false;
                ctxMenuItemMarkAllLines.Visible = false;
                ctxMenuItemMarkSpecificOptions.Visible = true;
            }
            else if ( e.Node is NodeLine line )
            {
                if ( CurrentDocument.TextFormat == TextFormat.Csv )
                {
                    // Most logical step for Csv would be to mark the entire line.
                    _scintilla.SelectSingleLine( line.DocumentLine.DocumentLineNumber );
                }
                else if ( CurrentDocument.TextFormat == TextFormat.Plain ) // TODO More performant way
                {
                    var lineNoList = CurrentDocument.Lines
                        .Cast<PlainTextLine>()
                        .Where( l => l.LineIdentifier == ( line.DocumentLine as PlainTextLine ).LineIdentifier )
                        .Select( l => l.DocumentLineNumber );


                    _scintilla.SelectLines( lineNoList );
                }

                ctxMenuItemShowOnlyLines.Visible = true;
                ctxMenuItemMarkAllLines.Visible = true;
                ctxMenuItemMarkSpecificOptions.Visible = false;
            }
        }
    }
}
