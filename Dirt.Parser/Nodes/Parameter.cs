using Dirt.Lexer;

namespace Dirt.Parser.Nodes
{
    public class Parameter
    {
        public string Name { get; set; } = "";

        public TokenType Type { get; set; }

        public Parameter(string name, TokenType type)
        {
            this.Name = name;
            this.Type = type;
        }
    }
}
