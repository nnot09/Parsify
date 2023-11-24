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
        public ParsifyLine ParsifyLine { get; }
        public int DocumentLineNo { get; set; }

        public NodeLine( ParsifyLine line, int lineNo ) 
            : base ( line.Name )
        {
            this.ParsifyLine = line;
            this.DocumentLineNo = lineNo;
        }
    }
}
