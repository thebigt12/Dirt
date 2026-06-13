namespace Dirt.Parser.Nodes
{
    public class MatchStatementNode : AstNode
    {
        public AstNode Value {  get; set; }

        public List<MatchArm> Arms { get; set; }

        public MatchStatementNode(AstNode value, List<MatchArm> arms)
        {
            Value = value;
            Arms = arms;
        }
    }
}
