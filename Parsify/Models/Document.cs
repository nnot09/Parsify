using Parsify.Models.Events.EArgs;
using Parsify.Other;
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
        private string _filePath;
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                Hash = this.CreateHash();
            }
        }
        public string FileName => Path.GetFileName( FilePath );
        public string FormatName { get; set; }
        public string Version { get; set; }
        public ParsifyModule Parser { get; set; }
        public List<TextLine> Lines { get; set; }
        public byte[] Hash { get; private set; }

        public Document()
        {
            Lines = new List<TextLine>();
        }

        public override string ToString()
            => $"{FormatName} ({Version})";
    }
}
