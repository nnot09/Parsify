namespace Parsify.Core.Models
{
    public class Area
    {
        public int Start { get; set; }
        public int End { get; set; }

        public Area()
        {
        }
        public Area( int start, int end )
        {
            Start = start;
            End = end;
        }
    }
}
