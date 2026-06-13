namespace Dirt.Parser.Nodes
{
    public class IdentifierNode : AstNode
    {
        public string Name { get; set; } = "";

        public IdentifierNode(string name)
        {
            Name = name;
        }
    }
}
