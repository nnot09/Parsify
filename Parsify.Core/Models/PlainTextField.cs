using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsify.Core.Models.Values
{
    public class PlainTextField : BaseField
    {
        public int Index { get; set; }
        public int Length { get; set; }

        public override string ToString()
            => $"{Name}: {Value ?? "(null)"}";
    }
}
