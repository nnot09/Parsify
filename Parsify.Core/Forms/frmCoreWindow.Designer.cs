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
            this.btnOpenConfig = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboTextFormats = new System.Windows.Forms.ComboBox();
            this.btnUpdateModules = new System.Windows.Forms.Button();
            this.panContent = new System.Windows.Forms.Panel();
            this.treeDataView = new System.Windows.Forms.TreeView();
            this.treeNodeContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxMenuItemShowOnlyLines = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxMenuItemMarkAllLines = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxMenuItemMarkSpecificOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxMenuItemMarkSpecificOptionValue = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxMenuItemMarkSpecificOptionAllValues = new System.Windows.Forms.ToolStripMenuItem();
            this.panToolbar.SuspendLayout();
            this.panContent.SuspendLayout();
            this.treeNodeContext.SuspendLayout();
            this.SuspendLayout();
            // 
            // panToolbar
            // 
            this.panToolbar.Controls.Add(this.btnOpenConfig);
            this.panToolbar.Controls.Add(this.label1);
            this.panToolbar.Controls.Add(this.comboTextFormats);
            this.panToolbar.Controls.Add(this.btnUpdateModules);
            this.panToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panToolbar.Location = new System.Drawing.Point(0, 0);
            this.panToolbar.Name = "panToolbar";
            this.panToolbar.Size = new System.Drawing.Size(581, 33);
            this.panToolbar.TabIndex = 0;
            // 
            // btnOpenConfig
            // 
            this.btnOpenConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenConfig.Location = new System.Drawing.Point(519, 6);
            this.btnOpenConfig.Name = "btnOpenConfig";
            this.btnOpenConfig.Size = new System.Drawing.Size(58, 21);
            this.btnOpenConfig.TabIndex = 3;
            this.btnOpenConfig.Text = "Config";
            this.btnOpenConfig.UseVisualStyleBackColor = true;
            this.btnOpenConfig.Click += new System.EventHandler(this.btnOpenConfig_Click);
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
            this.comboTextFormats.Size = new System.Drawing.Size(391, 21);
            this.comboTextFormats.TabIndex = 1;
            this.comboTextFormats.SelectedIndexChanged += new System.EventHandler(this.comboTextFormats_SelectedIndexChanged);
            // 
            // btnUpdateModules
            // 
            this.btnUpdateModules.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateModules.Location = new System.Drawing.Point(455, 6);
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
            this.panContent.Size = new System.Drawing.Size(581, 405);
            this.panContent.TabIndex = 1;
            // 
            // treeDataView
            // 
            this.treeDataView.ContextMenuStrip = this.treeNodeContext;
            this.treeDataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeDataView.Location = new System.Drawing.Point(0, 0);
            this.treeDataView.Name = "treeDataView";
            this.treeDataView.Size = new System.Drawing.Size(581, 405);
            this.treeDataView.TabIndex = 0;
            this.treeDataView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeDataView_NodeMouseClick);
            // 
            // treeNodeContext
            // 
            this.treeNodeContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxMenuItemShowOnlyLines,
            this.ctxMenuItemMarkAllLines,
            this.ctxMenuItemMarkSpecificOptions});
            this.treeNodeContext.Name = "treeNodeContext";
            this.treeNodeContext.Size = new System.Drawing.Size(184, 92);
            // 
            // ctxMenuItemShowOnlyLines
            // 
            this.ctxMenuItemShowOnlyLines.Name = "ctxMenuItemShowOnlyLines";
            this.ctxMenuItemShowOnlyLines.Size = new System.Drawing.Size(183, 22);
            this.ctxMenuItemShowOnlyLines.Text = "Show only \"{0}\" lines";
            // 
            // ctxMenuItemMarkAllLines
            // 
            this.ctxMenuItemMarkAllLines.Name = "ctxMenuItemMarkAllLines";
            this.ctxMenuItemMarkAllLines.Size = new System.Drawing.Size(183, 22);
            this.ctxMenuItemMarkAllLines.Text = "Mark all \"{0}\" lines";
            // 
            // ctxMenuItemMarkSpecificOptions
            // 
            this.ctxMenuItemMarkSpecificOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxMenuItemMarkSpecificOptionValue,
            this.ctxMenuItemMarkSpecificOptionAllValues});
            this.ctxMenuItemMarkSpecificOptions.Name = "ctxMenuItemMarkSpecificOptions";
            this.ctxMenuItemMarkSpecificOptions.Size = new System.Drawing.Size(183, 22);
            this.ctxMenuItemMarkSpecificOptions.Text = "Mark";
            // 
            // ctxMenuItemMarkSpecificOptionValue
            // 
            this.ctxMenuItemMarkSpecificOptionValue.Name = "ctxMenuItemMarkSpecificOptionValue";
            this.ctxMenuItemMarkSpecificOptionValue.Size = new System.Drawing.Size(180, 22);
            this.ctxMenuItemMarkSpecificOptionValue.Text = "Value";
            // 
            // ctxMenuItemMarkSpecificOptionAllValues
            // 
            this.ctxMenuItemMarkSpecificOptionAllValues.Name = "ctxMenuItemMarkSpecificOptionAllValues";
            this.ctxMenuItemMarkSpecificOptionAllValues.Size = new System.Drawing.Size(180, 22);
            this.ctxMenuItemMarkSpecificOptionAllValues.Text = "All values of \"{0}\"";
            // 
            // frmCoreWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 438);
            this.Controls.Add(this.panContent);
            this.Controls.Add(this.panToolbar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmCoreWindow";
            this.Text = "Parsify";
            this.panToolbar.ResumeLayout(false);
            this.panToolbar.PerformLayout();
            this.panContent.ResumeLayout(false);
            this.treeNodeContext.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panToolbar;
        private System.Windows.Forms.Panel panContent;
        private System.Windows.Forms.Button btnUpdateModules;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboTextFormats;
        private System.Windows.Forms.Button btnOpenConfig;
        private System.Windows.Forms.TreeView treeDataView;
        private System.Windows.Forms.ContextMenuStrip treeNodeContext;
        private System.Windows.Forms.ToolStripMenuItem ctxMenuItemShowOnlyLines;
        private System.Windows.Forms.ToolStripMenuItem ctxMenuItemMarkAllLines;
        private System.Windows.Forms.ToolStripMenuItem ctxMenuItemMarkSpecificOptions;
        private System.Windows.Forms.ToolStripMenuItem ctxMenuItemMarkSpecificOptionValue;
        private System.Windows.Forms.ToolStripMenuItem ctxMenuItemMarkSpecificOptionAllValues;
    }
}