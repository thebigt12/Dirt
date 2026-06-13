namespace Dirt.Parser.Nodes
{
    public class DecimalLiteralNode : AstNode
    {
        public decimal Value { get; set; }

        public DecimalLiteralNode(decimal value)
        {
            this.Value = value;
        }
    }
}
