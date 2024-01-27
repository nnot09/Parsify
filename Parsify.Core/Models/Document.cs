using Parsify.Core.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Parsify.Core.Models
{
    public class Document
    {
        public string FilePath { get; set; }
        public string FileName => Path.GetFileName( FilePath );
        public string FormatName { get; set; }
        public string Version { get; set; }
        public List<TextLine> Lines { get; set; }

        public Document()
        {
            Lines = new List<TextLine>();
        }

        public override string ToString()
            => $"{FormatName} ({Version})";
    }
}
