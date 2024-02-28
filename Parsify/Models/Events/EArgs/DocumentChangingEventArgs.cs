using System;

namespace Parsify.Models.Events.EArgs
{
    public class DocumentChangingEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
        public Document Current { get; set; }
    }
}
