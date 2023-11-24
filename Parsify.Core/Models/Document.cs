using Parsify.Core.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsify.Core.Models.Values
{
    public class Document
    {
        public string FilePath { get; set; }
        public string FileName => Path.GetFileName(FilePath);
        public string FormatName { get; set; }
        public string Version { get; set; }
        public TextFormat TextFormat { get; set; }
        public List<Line> Lines { get; set; }

        public Document()
        {
             Lines = new List<Line>();
        }

        public override string ToString()
            => $"{FormatName} ({Version})";
    }
}
