using Parsify.Core.Models;
using Parsify.Core.Models.Values;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parsify.Core.Forms.NodeControls
{
    internal class NodeField : TreeNode
    {
        public BaseField DocumentField { get; }

        public NodeField( BaseField field )
            : base( field.Name ) // https://www.youtube.com/watch?v=otCpCn0l4Wo
        {
            this.DocumentField = field;
        }
    }
}
