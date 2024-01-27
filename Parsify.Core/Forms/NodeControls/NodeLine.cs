using Parsify.Core.Models;
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
        public TextLine DocumentLine { get; }

        public NodeLine( TextLine documentLine ) 
            : base( documentLine.ToString() )
        {
            this.DocumentLine = documentLine;
        }
    }
}
