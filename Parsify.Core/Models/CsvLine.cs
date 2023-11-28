using Parsify.Core.Models.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Parsify.Core.Models
{
    public class CsvLine : BaseLine
    {
        public override string ToString()
            => $"Line {base.DocumentLineNumber}";
    }
}
