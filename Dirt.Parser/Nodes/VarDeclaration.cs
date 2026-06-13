using Dirt.Lexer;

namespace Dirt.Parser.Nodes
{
    public class VarDeclaration : AstNode
    {
        public string Name { get; set; } = "";

        public TokenType Type { get; set; }

        public AstNode Value { get; set; }

        public VarDeclaration(string name, TokenType type, AstNode value)
        {
            this.Name = name;
            this.Type = type;
            this.Value = value;
        }
    }
}
