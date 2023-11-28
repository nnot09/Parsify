using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsify.Core.Models.Values // TODO ns
{
    public class PlainTextLine : BaseLine
    {
        public string LineIdentifier {  get; set; }

        public override string ToString()
            => LineIdentifier; 
    }
}
