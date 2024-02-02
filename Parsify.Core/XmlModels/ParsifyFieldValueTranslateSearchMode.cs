using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsify.Core.XmlModels
{
    public enum ParsifyFieldValueTranslateSearchMode
    {
        Default,    // Equals
        Contains,
        StartsWith,
        EndsWith,
        Regex
    }
}
