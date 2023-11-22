using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Parsify.Core.Models
{
    [XmlRoot("Text")]
    public class Plain : BaseField
    {
        [XmlAttribute("Index")]
        public int Index { get; set; }

        [XmlAttribute("Length")]
        public int Length { get; set; }
    }
}
