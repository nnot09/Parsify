namespace Kbg.NppPluginNET
{
    partial class frmParseErrMessages
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
            this.txtErrMsg = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtErrMsg
            // 
            this.txtErrMsg.BackColor = System.Drawing.SystemColors.Control;
            this.txtErrMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtErrMsg.Location = new System.Drawing.Point(0, 0);
            this.txtErrMsg.Multiline = true;
            this.txtErrMsg.Name = "txtErrMsg";
            this.txtErrMsg.Size = new System.Drawing.Size(634, 367);
            this.txtErrMsg.TabIndex = 0;
            // 
            // frmParseErrMessages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 367);
            this.Controls.Add(this.txtErrMsg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmParseErrMessages";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Error Messages";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtErrMsg;
    }
}