namespace Parsify.Forms
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdForeground = new System.Windows.Forms.RadioButton();
            this.rdBackground = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
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
            this.btnCancel.Location = new System.Drawing.Point(118, 185);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(199, 185);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnConfirm.TabIndex = 5;
            this.btnConfirm.Text = "OK";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdForeground);
            this.groupBox1.Controls.Add(this.rdBackground);
            this.groupBox1.Location = new System.Drawing.Point(28, 75);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(246, 97);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Highlighter Mode";
            // 
            // rdForeground
            // 
            this.rdForeground.AutoSize = true;
            this.rdForeground.Location = new System.Drawing.Point(16, 57);
            this.rdForeground.Name = "rdForeground";
            this.rdForeground.Size = new System.Drawing.Size(79, 17);
            this.rdForeground.TabIndex = 1;
            this.rdForeground.TabStop = true;
            this.rdForeground.Text = "Foreground";
            this.rdForeground.UseVisualStyleBackColor = true;
            // 
            // rdBackground
            // 
            this.rdBackground.AutoSize = true;
            this.rdBackground.Location = new System.Drawing.Point(16, 34);
            this.rdBackground.Name = "rdBackground";
            this.rdBackground.Size = new System.Drawing.Size(126, 17);
            this.rdBackground.TabIndex = 0;
            this.rdBackground.TabStop = true;
            this.rdBackground.Text = "Background (Default)";
            this.rdBackground.UseVisualStyleBackColor = true;
            // 
            // frmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 220);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnBrowseNewPath);
            this.Controls.Add(this.txtDirectoryPath);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "App configuration";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDirectoryPath;
        private System.Windows.Forms.Button btnBrowseNewPath;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdForeground;
        private System.Windows.Forms.RadioButton rdBackground;
    }
}