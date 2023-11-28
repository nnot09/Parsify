using Parsify.Core.Models.Values;
using System.Collections.Generic;

namespace Parsify.Core.Models
{
    public class BaseLine
    {
        public List<BaseField> Fields { get; set; }
        public int DocumentLineNumber { get; set; }

        public BaseLine()
        {
            Fields = new List<BaseField>();
        }
    }
}