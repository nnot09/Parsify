using Parsify.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parsify.Core.Forms.NodeControls
{
    internal class NodeField : TreeNode
    {
        public ParsifyLine Line { get; }
        public BaseField Field { get; }

        public NodeField( ParsifyLine line, BaseField field ) 
            : base( $"{field.Name}: {field.Value}" )
        {
            this.Line = line;
            this.Field = field;
        }
    }
}
