namespace Dirt.Parser.Nodes
{
    public class Assignment : AstNode
    {
       public string Name { get; set; }="";

        public AstNode Value { get; set; }

        public Assignment(string name, AstNode value)
        {
            Name = name;

            Value = value;
        }
    }
}
