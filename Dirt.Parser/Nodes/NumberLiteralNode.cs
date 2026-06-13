namespace Dirt.Parser.Nodes
{
    public class NumberLiteralNode : AstNode
    {
        public int Value { get; set; }

        public NumberLiteralNode(int value)
        {
            this.Value = value;
        }
    }
}
