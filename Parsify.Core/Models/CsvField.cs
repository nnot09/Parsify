using Parsify.Core.Models.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Parsify.Core.Models
{
    public class CsvField : BaseField
    {
        public int DataIndex { get; set; }

        public int Length { get; set; }

        public override string ToString()
            => $"{Name}: {Value}";
    }
}
