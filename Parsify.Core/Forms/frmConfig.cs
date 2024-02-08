using Kbg.NppPluginNET;
using Parsify.Core.Config;
using Parsify.Core.Other;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parsify.Core.Forms
{
    public partial class frmConfig : Form
    {
        public frmConfig()
        {
            InitializeComponent();

            this.txtDirectoryPath.Text = Main.Configuration.ModulesDirectoryPath;
            this.comboHighlightingMode.SelectedIndex = (int)Main.Configuration.HighlightingMode;
            this.colorDiag.Color = Color.FromArgb( (int)Main.Configuration.Color );
            this.panColor.BackColor = this.colorDiag.Color;
            this.numTransparency.Value = Main.Configuration.Transparency;
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

        private void btnConfirm_Click( object sender, EventArgs e )
        {
            if ( this.txtDirectoryPath == null || !Directory.Exists( this.txtDirectoryPath.Text ) )
            {
                MessageBox.Show( "Given directory is invalid or does not exist." );
                return;
            }

            Main.Configuration.ModulesDirectoryPath = this.txtDirectoryPath.Text.Trim();
            Main.Configuration.Transparency = (int)this.numTransparency.Value;
            Main.Configuration.Color = (uint)this.panColor.BackColor.ToArgb();
            Main.Configuration.HighlightingMode = (AppHighlightingMode)this.comboHighlightingMode.SelectedIndex;

            this.DialogResult = DialogResult.OK;
        }

        private void btnBrowseNewPath_Click( object sender, EventArgs e )
        {
            using ( FolderBrowserDialog fbd = new FolderBrowserDialog() )
            {
                if ( fbd.ShowDialog() == DialogResult.OK )
                    this.txtDirectoryPath.Text = fbd.SelectedPath;
            }
        }

        private void comboHighlightingMode_SelectedIndexChanged( object sender, EventArgs e )
        {
            this.panColorConfiguration.Enabled = comboHighlightingMode.SelectedIndex == 1;
        }

        private void panColor_MouseClick( object sender, MouseEventArgs e )
        {
            if ( colorDiag.ShowDialog() == DialogResult.OK )
            {
                this.panColor.BackColor = colorDiag.Color;
            }
        }
    }
}
