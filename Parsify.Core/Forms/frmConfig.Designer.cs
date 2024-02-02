namespace Parsify.Core.Forms
{
    partial class frmConfig
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtDirectoryPath = new System.Windows.Forms.TextBox();
            this.btnBrowseNewPath = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboHighlightingMode = new System.Windows.Forms.ComboBox();
            this.grpHighlighting = new System.Windows.Forms.GroupBox();
            this.panColorConfiguration = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.numTransparency = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.panColor = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.colorDiag = new System.Windows.Forms.ColorDialog();
            this.grpHighlighting.SuspendLayout();
            this.panColorConfiguration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTransparency)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Modules-Path:";
            // 
            // txtDirectoryPath
            // 
            this.txtDirectoryPath.Location = new System.Drawing.Point(28, 49);
            this.txtDirectoryPath.Name = "txtDirectoryPath";
            this.txtDirectoryPath.Size = new System.Drawing.Size(214, 20);
            this.txtDirectoryPath.TabIndex = 2;
            // 
            // btnBrowseNewPath
            // 
            this.btnBrowseNewPath.Location = new System.Drawing.Point(248, 49);
            this.btnBrowseNewPath.Name = "btnBrowseNewPath";
            this.btnBrowseNewPath.Size = new System.Drawing.Size(26, 20);
            this.btnBrowseNewPath.TabIndex = 3;
            this.btnBrowseNewPath.Text = "...";
            this.btnBrowseNewPath.UseVisualStyleBackColor = true;
            this.btnBrowseNewPath.Click += new System.EventHandler(this.btnBrowseNewPath_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(118, 264);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(199, 264);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnConfirm.TabIndex = 5;
            this.btnConfirm.Text = "OK";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Mode:";
            // 
            // comboHighlightingMode
            // 
            this.comboHighlightingMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboHighlightingMode.FormattingEnabled = true;
            this.comboHighlightingMode.Items.AddRange(new object[] {
            "Default Marker",
            "Colored Marker"});
            this.comboHighlightingMode.Location = new System.Drawing.Point(16, 44);
            this.comboHighlightingMode.Name = "comboHighlightingMode";
            this.comboHighlightingMode.Size = new System.Drawing.Size(211, 21);
            this.comboHighlightingMode.TabIndex = 7;
            this.comboHighlightingMode.SelectedIndexChanged += new System.EventHandler(this.comboHighlightingMode_SelectedIndexChanged);
            // 
            // grpHighlighting
            // 
            this.grpHighlighting.Controls.Add(this.panColorConfiguration);
            this.grpHighlighting.Controls.Add(this.comboHighlightingMode);
            this.grpHighlighting.Controls.Add(this.label2);
            this.grpHighlighting.Location = new System.Drawing.Point(28, 75);
            this.grpHighlighting.Name = "grpHighlighting";
            this.grpHighlighting.Size = new System.Drawing.Size(246, 169);
            this.grpHighlighting.TabIndex = 8;
            this.grpHighlighting.TabStop = false;
            this.grpHighlighting.Text = "Highlighting";
            // 
            // panColorConfiguration
            // 
            this.panColorConfiguration.Controls.Add(this.label5);
            this.panColorConfiguration.Controls.Add(this.numTransparency);
            this.panColorConfiguration.Controls.Add(this.label4);
            this.panColorConfiguration.Controls.Add(this.panColor);
            this.panColorConfiguration.Controls.Add(this.label3);
            this.panColorConfiguration.Location = new System.Drawing.Point(16, 76);
            this.panColorConfiguration.Name = "panColorConfiguration";
            this.panColorConfiguration.Size = new System.Drawing.Size(211, 71);
            this.panColorConfiguration.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(179, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "%";
            // 
            // numTransparency
            // 
            this.numTransparency.Location = new System.Drawing.Point(118, 39);
            this.numTransparency.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTransparency.Name = "numTransparency";
            this.numTransparency.Size = new System.Drawing.Size(55, 20);
            this.numTransparency.TabIndex = 3;
            this.numTransparency.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Transparency";
            // 
            // panColor
            // 
            this.panColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panColor.Location = new System.Drawing.Point(173, 9);
            this.panColor.Name = "panColor";
            this.panColor.Size = new System.Drawing.Size(25, 24);
            this.panColor.TabIndex = 1;
            this.panColor.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panColor_MouseClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Select Color";
            // 
            // frmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 307);
            this.Controls.Add(this.grpHighlighting);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnBrowseNewPath);
            this.Controls.Add(this.txtDirectoryPath);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "App configuration";
            this.grpHighlighting.ResumeLayout(false);
            this.grpHighlighting.PerformLayout();
            this.panColorConfiguration.ResumeLayout(false);
            this.panColorConfiguration.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTransparency)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDirectoryPath;
        private System.Windows.Forms.Button btnBrowseNewPath;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboHighlightingMode;
        private System.Windows.Forms.GroupBox grpHighlighting;
        private System.Windows.Forms.Panel panColorConfiguration;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numTransparency;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panColor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ColorDialog colorDiag;
    }
}