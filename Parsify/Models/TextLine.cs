using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsify.Models
{
    public class TextLine
    {
        public string LineIdentifier { get; set; }
        public List<DataField> Fields { get; set; }
        public int DocumentLineNumber { get; set; }

        public TextLine()
        {
            Fields = new List<DataField>();
        }

        public override string ToString()
            => LineIdentifier;
    }
}
