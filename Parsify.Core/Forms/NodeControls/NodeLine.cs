using Parsify.Core.Models;
using Parsify.Core.Models.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parsify.Core.Forms.NodeControls
{
    internal class NodeLine : TreeNode
    {
        public BaseLine DocumentLine { get; }

        public NodeLine( BaseLine documentLine ) 
            : base( documentLine.ToString() )
        {
            this.DocumentLine = documentLine;
        }
    }
}
