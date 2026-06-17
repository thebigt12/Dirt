using Dirt.Lexer;
using Dirt.Parser.Nodes;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

namespace Dirt.Parser
{
    public class Parser
    {
        private List<Token> _tokens;

        private int _index;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _index = 0;
        }

        private Token Peek()
        {
            return _tokens[_index];
        }

        private Token PeekNext()
        {
            return _tokens[_index + 1];
        }

        private Token Consume()
        {
            Token returner = _tokens[_index];
            _index++;
            return returner;
        }

        private Token Expect(TokenType type)
        {
            if (Peek().TokenType != type)
            {
                throw new Exception($"Expected {type} but got {Peek().TokenType} on line {Peek().Line}");
            }
            return Consume();
        }

        public List<AstNode> Parse()
        {
            List<AstNode> list = new List<AstNode>();
            while (Peek().TokenType != TokenType.EOF)
            {
                if (Peek().TokenType == TokenType.Newline)
                {
                    Consume();
                    continue;
                }
                list.Add(ParseStatement());
            }
            return list;
        }
        //Syntax
        private AstNode ParseStatement()
        {
            Token current = Peek();
            switch (current.TokenType)
            {
                case TokenType.Identifier:
                    if (PeekNext().TokenType == TokenType.LBracket)
                        return ParseFunctionDeclaration();
                    else if (PeekNext().TokenType == TokenType.Colon)
                        return ParseVarDeclaration();
                    else if(PeekNext().TokenType == TokenType.Equals) {
                        return ParseAssignment();
                    }
                        return ParseExpression();
                case TokenType.Output:
                    return ParseOutStatement();
                case TokenType.When:
                    return ParseWhenStatement();
                case TokenType.Repeat:
                    return ParseRepeatStatement();
                case TokenType.Match:
                    return ParseMatchStatement();
            }
            throw new Exception($"Unexpected STATEMENT on {current.Line}");
        }

        private List<AstNode> ParseBlock()
        {
            Expect(TokenType.LBrace);
            List<AstNode> list = new List<AstNode>();
            while(Peek().TokenType != TokenType.RBrace && Peek().TokenType != TokenType.EOF)
            {
                if(Peek().TokenType == TokenType.Newline)
                {
                    Consume();
                    continue;
                }
                list.Add(ParseStatement());
            }
            Expect(TokenType.RBrace);
            return list;
        }

        //Precedence
        private AstNode ParsePrimary()
        {
            Token current = Consume();
            switch (current.TokenType)
            {
                case TokenType.StringLiteral:
                    return new StringLiteralNode(current.Value);
                case TokenType.NumberLiteral:
                    return new NumberLiteralNode(int.Parse(current.Value));
                case TokenType.DecimalLiteral:
                    return new DecimalLiteralNode(decimal.Parse(current.Value, System.Globalization.NumberStyles.Any));
                case TokenType.True:
                    return new BoolLiteralNode(true);
                case TokenType.False:
                    return new BoolLiteralNode(false);
                case TokenType.LBracket:
                    AstNode inner = ParseExpression();
                    Expect(TokenType.RBracket);
                    return inner;
                case TokenType.Identifier:
                    if(Peek().TokenType == TokenType.LBracket)
                    {
                        Consume();
                        List<AstNode> args = new List<AstNode>();
                        while(Peek().TokenType != TokenType.RBracket)
                        {
                            args.Add(ParseExpression());
                            if (Peek().TokenType == TokenType.Comma)
                            {
                                Consume();
                            }
                        }
                        Expect(TokenType.RBracket);
                        return new FunctionCall(current.Value, args);
                    }
                    return new IdentifierNode(current.Value);
                default:
                    throw new Exception($"Unexpected Token on line {current.Line}");
            }
        }

        private AstNode ParseMultiplicative()
        {
            AstNode left = ParsePrimary();
            while(Peek().TokenType == TokenType.Star || Peek().TokenType == TokenType.Slash)
            {
                Token op = Consume();
                AstNode right = ParsePrimary();
                left = new BinaryExpression(left, op.TokenType, right);
            }
            return left;
        }

        private AstNode ParseAddition()
        {
            AstNode left = ParseMultiplicative();
            while (Peek().TokenType == TokenType.Plus || Peek().TokenType == TokenType.Minus)
            {
                Token op = Consume();
                AstNode right = ParseMultiplicative();
                left = new BinaryExpression(left, op.TokenType, right);
            }
            return left;
        }

        private AstNode ParseExpression()
        {
            AstNode left = ParseAddition();
            while (Peek().TokenType == TokenType.EqualsEquals || Peek().TokenType == TokenType.NotEquals || Peek().TokenType == TokenType.Greater || Peek().TokenType == TokenType.Smaller || Peek().TokenType == TokenType.SmallerEquals || Peek().TokenType == TokenType.GreaterEquals)
            {
                Token op = Consume();
                AstNode right = ParseAddition();
                left = new BinaryExpression(left, op.TokenType, right);
            }
            return left;
        }

        //functionalities
        private VarDeclaration ParseVarDeclaration()
        {
            //x: int = 5
            string name = Expect(TokenType.Identifier).Value;
            Expect(TokenType.Colon);
            Token token = Consume();
            switch (token.TokenType)
            {
                case TokenType.IntegerType:
                case TokenType.BoolType:
                case TokenType.DecimalType:
                case TokenType.StringType:
                    break;
                default:
                    throw new Exception($"Unexpected TokenType on line {token.Line}");
            }
            TokenType type = token.TokenType;
            Expect(TokenType.Equals);
            return new VarDeclaration(name, type, ParseExpression());
        }

        private AstNode ParseAssignment()
        {
            //x = 5;
            string name = Expect(TokenType.Identifier).Value;
            Expect(TokenType.Equals);
            AstNode value = ParseExpression();
            return new Assignment(name, value);
        }
        private AstNode ParseOutStatement()
        {
            //out("hello")
            Expect(TokenType.Output);
            Expect(TokenType.LBracket);
            AstNode value = ParseExpression();
            Expect(TokenType.RBracket);
            return new OutStatement(value);
        }

        private AstNode ParseWhenStatement()
        {
            //when(x<y){}
            Expect(TokenType.When);
            Expect(TokenType.LBracket);
            AstNode condition = ParseExpression();
            Expect(TokenType.RBracket);
            bool isRepeat = false;
            if(Peek().TokenType == TokenType.Repeat)
            {
                Consume();
                isRepeat = true;
            }
            List<AstNode> body = ParseBlock();
            List<AstNode> otherwise = null!;
            if(Peek().TokenType == TokenType.Otherwise)
            {
                Consume();
                otherwise = ParseBlock();
            }
            return new WhenStatementNode(condition, body, otherwise, isRepeat);
        }

        private AstNode ParseRepeatStatement()
        {
            //repeat x (){}
            Expect(TokenType.Repeat);
            AstNode? count = Peek().TokenType == TokenType.LBrace ? null : ParseExpression();
            List<AstNode> body = ParseBlock();
            return new RepeatStatementNode(count, body);
        }

        private AstNode ParseMatchStatement()
        {
            //match(x){}
            Expect(TokenType.Match);
            Expect(TokenType.LBracket);
            AstNode value = ParseExpression();
            Expect(TokenType.RBracket);
            Expect(TokenType.LBrace);
            List<MatchArm> list = new List<MatchArm>();
            while(Peek().TokenType != TokenType.RBrace)
            {
                if(Peek().TokenType == TokenType.Newline)
                {
                    Consume();
                    continue;
                }
                list.Add(ParseMatchArm());
            }
            Expect(TokenType.RBrace);
            return new MatchStatementNode(value, list);
        }
        private MatchArm ParseMatchArm()
        {
            //x -> out || x.. out || _ -> 
            AstNode pattern;
            if(Peek().TokenType == TokenType.Underscore)
            {
                Consume();
                pattern = new WildcardPattern();
            }
            else
            {
                AstNode from = ParseExpression();
                if(Peek().TokenType == TokenType.DoubleDot)
                {
                    Consume();
                    AstNode to = ParseExpression();
                    pattern = new RangePattern(from, to);
                }
                else
                {
                    pattern = from;
                }
            }
            Expect(TokenType.Arrow);
            List<AstNode> list = ParseBlock();
            return new MatchArm(pattern, list);
        }
        private AstNode ParseFunctionDeclaration()
        {
            //x(a,b): int{}
            string name = Expect(TokenType.Identifier).Value;
            Expect(TokenType.LBracket);
            List<Parameter> list = new List<Parameter>();
            while(Peek().TokenType != TokenType.RBracket) {
                string pname = Expect(TokenType.Identifier).Value;
                Expect(TokenType.Colon);
                TokenType ptype = Consume().TokenType;
                list.Add(new Parameter(pname, ptype));
                if(Peek().TokenType == TokenType.Comma)
                {
                    Consume();
                }
            }
            Expect(TokenType.RBracket);
            TokenType? type = null;
            if(Peek().TokenType == TokenType.Colon) {
                Consume();
                type = Consume().TokenType;
            }
            return new FunctionDeclaration(name, list, type, ParseBlock());
        }
    }
}