using System;

namespace Parsify.Models.Events.EArgs
{
    public class DocumentUnloadedEventArgs : EventArgs
    {
        public Document UnloadedDocument { get; set; }
    }
}
