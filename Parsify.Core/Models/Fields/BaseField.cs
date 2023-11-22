﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Parsify.Core.Models
{
    [XmlInclude( typeof( Plain ) )]
    [XmlInclude( typeof( Csv ) )]
    public class BaseField
    {
        [XmlAttribute( "Type" )]
        public string DataType { get; set; } = "string";

        [XmlAttribute( "Name" )]
        public string Name { get; set; } = "unknown";

        [XmlIgnore]
        public string Value { get; set; }
    }
}
