namespace Kbg.NppPluginNET
{
    partial class frmCoreWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panToolbar = new System.Windows.Forms.Panel();
            this.btnHighlightSwitch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboTextFormats = new System.Windows.Forms.ComboBox();
            this.btnUpdateModules = new System.Windows.Forms.Button();
            this.panContent = new System.Windows.Forms.Panel();
            this.treeDataView = new Parsify.Forms.NodeControls.FieldTreeView();
            this.treeNodeContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxMenuItemShowOnlyLines = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxMenuItemShowOnlyColumnType = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxMenuItemMarkAllLines = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxMenuItemMarkAllIdenticalLines = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxMenuItemMarkSpecificOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxMenuItemMarkSpecificOptionValue = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxMenuItemMarkSpecificOptionAllValues = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.footerlbTotalLinesCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.footerlbSelectedCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.footerlbParsifyErrorsCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.panToolbar.SuspendLayout();
            this.panContent.SuspendLayout();
            this.treeNodeContext.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panToolbar
            // 
            this.panToolbar.Controls.Add(this.btnHighlightSwitch);
            this.panToolbar.Controls.Add(this.label1);
            this.panToolbar.Controls.Add(this.comboTextFormats);
            this.panToolbar.Controls.Add(this.btnUpdateModules);
            this.panToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panToolbar.Location = new System.Drawing.Point(0, 0);
            this.panToolbar.Name = "panToolbar";
            this.panToolbar.Size = new System.Drawing.Size(566, 33);
            this.panToolbar.TabIndex = 0;
            // 
            // btnHighlightSwitch
            // 
            this.btnHighlightSwitch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHighlightSwitch.Enabled = false;
            this.btnHighlightSwitch.Location = new System.Drawing.Point(501, 5);
            this.btnHighlightSwitch.Name = "btnHighlightSwitch";
            this.btnHighlightSwitch.Size = new System.Drawing.Size(58, 21);
            this.btnHighlightSwitch.TabIndex = 3;
            this.btnHighlightSwitch.Text = "Highlight";
            this.btnHighlightSwitch.UseVisualStyleBackColor = true;
            this.btnHighlightSwitch.Click += new System.EventHandler(this.btnHighlightSwitch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Choose";
            // 
            // comboTextFormats
            // 
            this.comboTextFormats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboTextFormats.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTextFormats.FormattingEnabled = true;
            this.comboTextFormats.Location = new System.Drawing.Point(58, 6);
            this.comboTextFormats.Name = "comboTextFormats";
            this.comboTextFormats.Size = new System.Drawing.Size(373, 21);
            this.comboTextFormats.TabIndex = 1;
            this.comboTextFormats.SelectedIndexChanged += new System.EventHandler(this.comboTextFormats_SelectedIndexChanged);
            // 
            // btnUpdateModules
            // 
            this.btnUpdateModules.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateModules.Location = new System.Drawing.Point(437, 5);
            this.btnUpdateModules.Name = "btnUpdateModules";
            this.btnUpdateModules.Size = new System.Drawing.Size(58, 21);
            this.btnUpdateModules.TabIndex = 0;
            this.btnUpdateModules.Text = "Update";
            this.btnUpdateModules.UseVisualStyleBackColor = true;
            this.btnUpdateModules.Click += new System.EventHandler(this.btnUpdateModules_Click);
            // 
            // panContent
            // 
            this.panContent.Controls.Add(this.treeDataView);
            this.panContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panContent.Location = new System.Drawing.Point(0, 33);
            this.panContent.Name = "panContent";
            this.panContent.Size = new System.Drawing.Size(566, 405);
            this.panContent.TabIndex = 1;
            // 
            // treeDataView
            // 
            this.treeDataView.ContextMenuStrip = this.treeNodeContext;
            this.treeDataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeDataView.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeDataView.Location = new System.Drawing.Point(0, 0);
            this.treeDataView.Name = "treeDataView";
            this.treeDataView.ShowNodeToolTips = true;
            this.treeDataView.Size = new System.Drawing.Size(566, 405);
            this.treeDataView.TabIndex = 0;
            this.treeDataView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeDataView_AfterSelect);
            // 
            // treeNodeContext
            // 
            this.treeNodeContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxMenuItemShowOnlyLines,
            this.ctxMenuItemShowOnlyColumnType,
            this.ctxMenuItemMarkAllLines,
            this.ctxMenuItemMarkAllIdenticalLines,
            this.ctxMenuItemMarkSpecificOptions});
            this.treeNodeContext.Name = "treeNodeContext";
            this.treeNodeContext.Size = new System.Drawing.Size(230, 114);
            // 
            // ctxMenuItemShowOnlyLines
            // 
            this.ctxMenuItemShowOnlyLines.Name = "ctxMenuItemShowOnlyLines";
            this.ctxMenuItemShowOnlyLines.Size = new System.Drawing.Size(229, 22);
            this.ctxMenuItemShowOnlyLines.Text = "Show only selected line type";
            this.ctxMenuItemShowOnlyLines.Click += new System.EventHandler(this.ctxMenuItemShowOnlyLines_Click);
            // 
            // ctxMenuItemShowOnlyColumnType
            // 
            this.ctxMenuItemShowOnlyColumnType.Enabled = false;
            this.ctxMenuItemShowOnlyColumnType.Name = "ctxMenuItemShowOnlyColumnType";
            this.ctxMenuItemShowOnlyColumnType.Size = new System.Drawing.Size(229, 22);
            this.ctxMenuItemShowOnlyColumnType.Text = "Show only column type";
            // 
            // ctxMenuItemMarkAllLines
            // 
            this.ctxMenuItemMarkAllLines.Name = "ctxMenuItemMarkAllLines";
            this.ctxMenuItemMarkAllLines.Size = new System.Drawing.Size(229, 22);
            this.ctxMenuItemMarkAllLines.Text = "Mark all lines from same type";
            this.ctxMenuItemMarkAllLines.Click += new System.EventHandler(this.ctxMenuItemMarkAllLines_Click);
            // 
            // ctxMenuItemMarkAllIdenticalLines
            // 
            this.ctxMenuItemMarkAllIdenticalLines.Name = "ctxMenuItemMarkAllIdenticalLines";
            this.ctxMenuItemMarkAllIdenticalLines.Size = new System.Drawing.Size(229, 22);
            this.ctxMenuItemMarkAllIdenticalLines.Text = "Mark all identical lines";
            // 
            // ctxMenuItemMarkSpecificOptions
            // 
            this.ctxMenuItemMarkSpecificOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxMenuItemMarkSpecificOptionValue,
            this.ctxMenuItemMarkSpecificOptionAllValues});
            this.ctxMenuItemMarkSpecificOptions.Name = "ctxMenuItemMarkSpecificOptions";
            this.ctxMenuItemMarkSpecificOptions.Size = new System.Drawing.Size(229, 22);
            this.ctxMenuItemMarkSpecificOptions.Text = "Mark";
            // 
            // ctxMenuItemMarkSpecificOptionValue
            // 
            this.ctxMenuItemMarkSpecificOptionValue.Name = "ctxMenuItemMarkSpecificOptionValue";
            this.ctxMenuItemMarkSpecificOptionValue.Size = new System.Drawing.Size(256, 22);
            this.ctxMenuItemMarkSpecificOptionValue.Text = "Value";
            this.ctxMenuItemMarkSpecificOptionValue.Click += new System.EventHandler(this.ctxMenuItemMarkSpecificOptionValue_Click);
            // 
            // ctxMenuItemMarkSpecificOptionAllValues
            // 
            this.ctxMenuItemMarkSpecificOptionAllValues.Name = "ctxMenuItemMarkSpecificOptionAllValues";
            this.ctxMenuItemMarkSpecificOptionAllValues.Size = new System.Drawing.Size(256, 22);
            this.ctxMenuItemMarkSpecificOptionAllValues.Text = "All values of selected type/column";
            this.ctxMenuItemMarkSpecificOptionAllValues.Click += new System.EventHandler(this.ctxMenuItemMarkSpecificOptionAllValues_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.footerlbTotalLinesCount,
            this.footerlbSelectedCount,
            this.footerlbParsifyErrorsCount});
            this.statusStrip1.Location = new System.Drawing.Point(0, 416);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(566, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // footerlbTotalLinesCount
            // 
            this.footerlbTotalLinesCount.Name = "footerlbTotalLinesCount";
            this.footerlbTotalLinesCount.Size = new System.Drawing.Size(86, 17);
            this.footerlbTotalLinesCount.Text = "Total Lines: n/a";
            // 
            // footerlbSelectedCount
            // 
            this.footerlbSelectedCount.Name = "footerlbSelectedCount";
            this.footerlbSelectedCount.Size = new System.Drawing.Size(217, 17);
            this.footerlbSelectedCount.Spring = true;
            this.footerlbSelectedCount.Text = "Selected Count: n/a";
            // 
            // footerlbParsifyErrorsCount
            // 
            this.footerlbParsifyErrorsCount.IsLink = true;
            this.footerlbParsifyErrorsCount.Name = "footerlbParsifyErrorsCount";
            this.footerlbParsifyErrorsCount.Size = new System.Drawing.Size(217, 17);
            this.footerlbParsifyErrorsCount.Spring = true;
            this.footerlbParsifyErrorsCount.Text = "Parsify: 0 Errors";
            this.footerlbParsifyErrorsCount.Click += new System.EventHandler(this.footerlbParsifyErrorsCount_Click);
            // 
            // frmCoreWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 438);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panContent);
            this.Controls.Add(this.panToolbar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmCoreWindow";
            this.Text = "Parsify";
            this.panToolbar.ResumeLayout(false);
            this.panToolbar.PerformLayout();
            this.panContent.ResumeLayout(false);
            this.treeNodeContext.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panToolbar;
        private System.Windows.Forms.Panel panContent;
        private System.Windows.Forms.Button btnUpdateModules;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboTextFormats;
        Parsify.Forms.NodeControls.FieldTreeView treeDataView;
        private System.Windows.Forms.ContextMenuStrip treeNodeContext;
        private System.Windows.Forms.ToolStripMenuItem ctxMenuItemShowOnlyLines;
        private System.Windows.Forms.ToolStripMenuItem ctxMenuItemMarkAllLines;
        private System.Windows.Forms.ToolStripMenuItem ctxMenuItemMarkSpecificOptions;
        private System.Windows.Forms.ToolStripMenuItem ctxMenuItemMarkSpecificOptionValue;
        private System.Windows.Forms.ToolStripMenuItem ctxMenuItemMarkSpecificOptionAllValues;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel footerlbTotalLinesCount;
        private System.Windows.Forms.ToolStripStatusLabel footerlbSelectedCount;
        private System.Windows.Forms.ToolStripStatusLabel footerlbParsifyErrorsCount;
        private System.Windows.Forms.ToolStripMenuItem ctxMenuItemMarkAllIdenticalLines;
        private System.Windows.Forms.ToolStripMenuItem ctxMenuItemShowOnlyColumnType;
        private System.Windows.Forms.Button btnHighlightSwitch;
    }
}