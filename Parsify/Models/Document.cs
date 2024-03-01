using Parsify.Models.Events.EArgs;
using Parsify.XmlModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Parsify.Models
{
    public class Document
    {
        public static Document Current { get; private set; }

        public event EventHandler<DocumentParsingEventArgs> DocumentParsingEvent;
        public event EventHandler<DocumentParsedEventArgs> DocumentParsedEvent;
        public event EventHandler<DocumentParserChangingEventArgs> DocumentChangingEvent;
        public event EventHandler<DocumentParserChangedEventArgs> DocumentChangedEvent;
        public event EventHandler<DocumentUnloadingEventArgs> DocumentUnloadingEvent;
        public event EventHandler<DocumentUnloadedEventArgs> DocumentUnloadedEvent;

        public string FilePath { get; set; }
        public string FileName => Path.GetFileName( FilePath );
        public string FormatName { get; set; }
        public string Version { get; set; }
        public ParsifyModule Parser { get; set; }
        public List<TextLine> Lines { get; set; }

        public Document()
        {
            Lines = new List<TextLine>();
        }

        public override string ToString()
            => $"{FormatName} ({Version})";
    }
}
