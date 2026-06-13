using Dirt.Lexer;

namespace Dirt.Parser.Nodes
{
    public class BinaryExpression : AstNode
    {
        public AstNode Left {  get; set; }
        public TokenType Op { get; set; }

        public AstNode Right { get; set; }

        public BinaryExpression  (AstNode left, TokenType op, AstNode right)
        {
            Left = left;
            Op = op;
            Right = right;
        }
    }
}
