namespace Dirt.Parser.Nodes
{
    public class MatchArm
    {
        public AstNode Pattern {  get; set; }

        public List<AstNode> Body {  get; set; }

        public MatchArm(AstNode pattern, List<AstNode> body)
        {
            Pattern = pattern;
            Body = body;
        }
    }
}
