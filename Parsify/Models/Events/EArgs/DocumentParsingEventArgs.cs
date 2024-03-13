using Parsify.XmlModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parsify.Models.Events.EArgs
{
    public class DocumentParsingEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
        public string FilePath { get; set; }
        public ParsifyModule Parser { get; set; }
    }
}
