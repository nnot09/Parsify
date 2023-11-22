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
            this.checkAutoDetect = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDirectoryPath = new System.Windows.Forms.TextBox();
            this.btnBrowseNewPath = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkAutoDetect
            // 
            this.checkAutoDetect.AutoSize = true;
            this.checkAutoDetect.Location = new System.Drawing.Point(28, 92);
            this.checkAutoDetect.Name = "checkAutoDetect";
            this.checkAutoDetect.Size = new System.Drawing.Size(180, 17);
            this.checkAutoDetect.TabIndex = 0;
            this.checkAutoDetect.Text = "Try auto-detect format on startup";
            this.checkAutoDetect.UseVisualStyleBackColor = true;
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
            this.btnCancel.Location = new System.Drawing.Point(127, 179);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(208, 179);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnConfirm.TabIndex = 5;
            this.btnConfirm.Text = "OK";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // frmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 214);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnBrowseNewPath);
            this.Controls.Add(this.txtDirectoryPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkAutoDetect);
            this.Name = "frmConfig";
            this.Text = "frmConfig";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkAutoDetect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDirectoryPath;
        private System.Windows.Forms.Button btnBrowseNewPath;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnConfirm;
    }
}