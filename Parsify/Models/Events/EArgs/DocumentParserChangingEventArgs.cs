using Parsify.XmlModels;
using System;

namespace Parsify.Models.Events.EArgs
{
    public class DocumentParserChangingEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
        public bool IsFirst { get; private set; }
        public Document Document { get; set; }
        public ParsifyModule Parser { get; set; }

        public void SetIsFirst()
            => IsFirst = Document == null;
    }
}
