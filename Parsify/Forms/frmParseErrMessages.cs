using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kbg.NppPluginNET
{
    public partial class frmParseErrMessages : Form
    {
        public frmParseErrMessages(string errMsg)
        {
            InitializeComponent();
            this.txtErrMsg.Text = errMsg;
        }
    }
}
