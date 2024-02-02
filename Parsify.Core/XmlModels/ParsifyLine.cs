using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace Parsify.Core.XmlModels
{
    public class ParsifyLine
    {
        [XmlAttribute( "StartsWith" )]
        public string StartsWithIdentifier { get; set; }

        [XmlElement( "Field" )]
        public List<ParsifyField> Fields { get; set; }
    }
}
