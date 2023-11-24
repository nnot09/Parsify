using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace Parsify.Core.Models
{
    public class ParsifyLine
    {
        [XmlAttribute( "Identifier" )]
        public string StartsWithIdentifier { get; set; }

        [XmlElement("Plain", typeof( ParsifyPlain ))]
        [XmlElement("Csv", typeof( ParsifyCsv ))]
        public List<ParsifyBaseField> Fields { get; set; }

        [XmlElement("CsvSplitDelimeter")]
        public string CsvSplitDelimeter { get; set; }
    }
}
