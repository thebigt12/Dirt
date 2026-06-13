namespace Dirt.Parser.Nodes
{
    public class StringLiteralNode : AstNode
    {
        public string Value { get; set; } = "";

        public StringLiteralNode(string value)
        {
            this.Value = value;
        }
    }
}
