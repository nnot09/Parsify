using Parsify.Core.Config;
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
        private readonly AppConfig _configuration;

        public frmConfig( AppConfig configuration )
        {
            InitializeComponent();

            this._configuration = configuration;

            this.txtDirectoryPath.Text = _configuration.ModulesDirectoryPath;
            this.checkAutoDetect.Checked = _configuration.AutoDetectTextFormat;
        }

        private void btnConfirm_Click( object sender, EventArgs e )
        {
            if ( this.txtDirectoryPath == null || !Directory.Exists(this.txtDirectoryPath.Text) )
            {
                MessageBox.Show( "Given directory is invalid or does not exist." );
                return;
            }

            this._configuration.ModulesDirectoryPath = this.txtDirectoryPath.Text.Trim();
            this._configuration.AutoDetectTextFormat = this.checkAutoDetect.Checked;

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
    }
}
