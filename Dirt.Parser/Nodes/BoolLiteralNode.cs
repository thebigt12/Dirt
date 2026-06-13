namespace Dirt.Parser.Nodes

{
    public class BoolLiteralNode : AstNode
    {
        public bool Value { get; set; }

        public BoolLiteralNode(bool value)
        {
            this.Value = value;
        }
    }
}
