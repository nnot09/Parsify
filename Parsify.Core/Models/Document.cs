using Parsify.Core.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Parsify.Core.Models.Values
{
    public class Document
    {
        public string FilePath { get; set; }
        public string FileName => Path.GetFileName( FilePath );
        public string FormatName { get; set; }
        public string Version { get; set; }
        public TextFormat TextFormat { get; set; }
        public bool HasHeader { get; set; }
        public string CsvSplitDelimeter { get; set; }
        public string CsvCommentLineIdentifier { get; set; }
        public string CsvEscapeCharacter { get; set; }
        public List<BaseLine> Lines { get; set; }

        public Document()
        {
            Lines = new List<BaseLine>();
        }

        public override string ToString()
            => $"{FormatName} ({Version})";
    }
}
