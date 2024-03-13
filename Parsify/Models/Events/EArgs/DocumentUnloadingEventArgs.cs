using System;

namespace Parsify.Models.Events.EArgs
{
    public class DocumentUnloadingEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
        public Document Document { get; set; }
    }
}
