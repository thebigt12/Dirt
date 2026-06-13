namespace Dirt.Parser.Nodes
{
    public class FunctionCall : AstNode
    {
        public string Name { get; set; } = "";

            public List<AstNode> Args { get; set; }

        public FunctionCall(string name, List<AstNode> args) {
            this.Name = name;
            this.Args = args;
        }
    }
}
