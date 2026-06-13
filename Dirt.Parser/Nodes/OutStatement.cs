namespace Dirt.Parser.Nodes
{
    public class OutStatement : AstNode
    {
        public AstNode Value { get; set; }

        public OutStatement(AstNode value)
        {
            Value=value;
        }
    }
}
