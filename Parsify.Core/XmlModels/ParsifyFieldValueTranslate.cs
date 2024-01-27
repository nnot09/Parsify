using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Parsify.Core.XmlModels
{
    public class ParsifyFieldValueTranslate
    {
        [XmlAttribute( "Value" )]
        public string Value { get; set; }

        [XmlAttribute( "Display" )]
        public string DisplayValue { get; set; }

        [XmlAttribute( "IgnoreCase" )]
        public bool IgnoreCase { get; set; }
    }
}
