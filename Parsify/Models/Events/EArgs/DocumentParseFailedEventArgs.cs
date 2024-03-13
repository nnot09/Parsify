using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parsify.Models.Events.EArgs
{
    public class DocumentParseFailedEventArgs : EventArgs
    {
        public string ErrorText { get; set; }
        public int NumberOfErrors { get; set; }
    }
}
