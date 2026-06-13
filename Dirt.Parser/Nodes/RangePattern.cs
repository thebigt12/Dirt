namespace Dirt.Parser.Nodes
{
    public class RangePattern : AstNode
    {
        public AstNode From { get; set; }

        public AstNode To { get; set; }

        public RangePattern(AstNode from, AstNode to)
        {
            From = from;
            To = to;
        }
    }
}
