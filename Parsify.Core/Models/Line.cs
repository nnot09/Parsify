using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsify.Core.Models.Values
{
    public class Line
    {
        public string LineIdentifier {  get; set; }
        public int DocumentLineNumber { get; set; }
        public List<Field> Fields { get; set; }
        
        public Line()
        {
             Fields = new List<Field>();
        }

        // TODO LineNo as prefix?
        public override string ToString()
            => LineIdentifier; 
    }
}
