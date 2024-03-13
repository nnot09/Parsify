using Parsify.XmlModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parsify.Models.Events.EArgs
{
    public class DocumentParserChangedEventArgs : EventArgs
    {
        public Document Document { get; set; }
        public ParsifyModule OldParser { get; set; }
        public ParsifyModule NewParser { get; set; }
    }
}
