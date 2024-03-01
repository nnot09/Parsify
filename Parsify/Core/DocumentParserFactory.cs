using Parsify.Models;
using Parsify.XmlModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parsify.Core
{
    public class DocumentParserCache
        : Dictionary<ParsifyModule, List<Document>>
    {
        
    }
}
