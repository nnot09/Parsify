using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsify.Core.Models
{
    public class BaseField
    {
        public BaseLine Parent { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
