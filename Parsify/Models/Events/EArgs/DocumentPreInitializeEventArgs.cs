using System;

namespace Parsify.Models.Events.EArgs
{
    public class DocumentPreInitializeEventArgs : EventArgs
    {
        /// <summary>
        /// Note: Cancellation of this event affects only the lexer update. you better don't cancel here.
        /// </summary>
        public bool Cancel { get; set; }
        public Document Document { get; set; }
    }
}
