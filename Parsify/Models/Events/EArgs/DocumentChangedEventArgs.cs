using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parsify.Models.Events.EArgs
{
    public class DocumentChangedEventArgs : EventArgs
    {
        public Document NewDocument { get; set; }
        public Document OldDocument { get; set; }
    }
}
