namespace Dirt.Parser.Nodes
{
    public class StopStatementNode : AstNode
    {
        public AstNode? Value { get; set; }
        
        public StopStatementNode(AstNode? value)
        {
           Value = value;
        }
    }
}
