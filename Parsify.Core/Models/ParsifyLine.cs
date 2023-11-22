using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Parsify.Core.Models
{
    public class ParsifyLine
    {
        [XmlAttribute( "Name" )]
        public string Name { get; set; }

        [XmlElement( "Field" )]
        public List<BaseField> Fields { get; set; }

        [XmlElement("CsvSplitDelimeter")]
        public string CsvSplitDelimeter { get; set; }
    }
}
