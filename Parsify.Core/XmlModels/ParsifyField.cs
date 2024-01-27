using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Parsify.Core.XmlModels
{
    public class ParsifyField
    {
        [XmlAttribute( "Type" )]
        public string DataType { get; set; } = "string";

        [XmlAttribute( "Name" )]
        public string Name { get; set; } = "unknown";

        [XmlAttribute( "Position" )]
        public int Position { get; set; }

        [XmlAttribute( "Length" )]
        public int Length { get; set; }

        [XmlElement( "Translate" )]
        public List<ParsifyFieldValueTranslate> Translations { get; set; }
    }
}
