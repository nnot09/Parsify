namespace Parsify.Core.Models
{
    public class LineInfo
    {
        public string Line { get; set; }
        public int LineNo { get; set; }
        public int Length => Line?.TrimEnd( new[] { '\r', '\n' } ).Length ?? 0;

        public LineInfo()
        {
        }
        public LineInfo( string line, int lineNo )
        {
            Line = line;
            LineNo = lineNo;
        }
    }
}
