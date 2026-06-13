namespace Dirt.Parser.Nodes
{
    public class WhenStatementNode : AstNode
    {
        public AstNode Condition { get; set; }

        public List<AstNode> Body { get; set; }
        public List<AstNode> Otherwise {  get; set; }

        public bool IsRepeat { get; set; }

        public WhenStatementNode(AstNode condition, List<AstNode> body, List<AstNode> otherwise, bool isRepeat)
        {
            Condition = condition;
            Body = body;
            Otherwise = otherwise;
            IsRepeat = isRepeat;
        }
    }
}
