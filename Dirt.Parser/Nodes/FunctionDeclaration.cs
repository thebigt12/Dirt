using Dirt.Lexer;

namespace Dirt.Parser.Nodes
{
    public class FunctionDeclaration : AstNode
    {
        public string Name { get; set; } = "";

        public List<Parameter> Parameters { get; set; }

        public TokenType? ReturnType { get; set; }

        public List<AstNode> Body { get; set; }

        public FunctionDeclaration(string name, List<Parameter> parameters, TokenType? returnType, List<AstNode> body)
        {
            Name = name;
            Parameters = parameters;
            ReturnType = returnType;
            Body = body;
        }
    }
}
