namespace Dirt.Parser.Nodes
{
    public class RepeatStatementNode : AstNode
    {
        public AstNode? Count { get; set; }

        public List<AstNode> Body { get; set; }

        public RepeatStatementNode(AstNode? count, List<AstNode> body)
        {
            Count = count;
            Body = body;
        }

    }
}
