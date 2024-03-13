using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Parsify.XmlModels
{
    public class ParsifyFieldValueTranslate
    {
        [XmlAttribute( "Value" )]
        public string Value { get; set; }

        [XmlAttribute( "Display" )]
        public string DisplayValue { get; set; }

        [XmlAttribute( "IgnoreCase" )]
        public bool IgnoreCase { get; set; }

        [XmlAttribute("InvertCondition")]
        public bool InvertCondition { get; set;}

        [XmlAttribute( "SearchMode" )]
        public ParsifyFieldValueTranslateSearchMode SearchMode { get; set; } = ParsifyFieldValueTranslateSearchMode.Default;
    }
}
