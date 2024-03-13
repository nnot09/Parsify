using Parsify.XmlModels;
using System;

namespace Parsify.Models.Events.EArgs
{
    public class DocumentParsedEventArgs : EventArgs
    {
        public Document Document { get; set; }
        public ParsifyModule Parser { get; set; }
    }
}
