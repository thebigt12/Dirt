using Dirt.Lexer;
using Dirt.Parser.Nodes;

namespace Dirt.Interpreter
{
    public class Interpreter
    {
        private Dictionary<string, object> _environment = new Dictionary<string, object>();

        public void Run(List<AstNode> nodes)
        {
            foreach (AstNode node in nodes)
                Execute(node);
        }

        private object Evaluate(AstNode node)
        {
            switch (node)
            {
                case BinaryExpression b:
                    int left = (int)Evaluate(b.Left);
                    int right = (int)Evaluate(b.Right);
                    switch (b.Op)
                    {
                        case TokenType.Star: return left * right;
                        case TokenType.Slash: return left / right;
                        case TokenType.Plus: return left + right;
                        case TokenType.Minus: return left - right;
                        case TokenType.Greater: return left > right;
                        case TokenType.Smaller: return left < right;
                        case TokenType.GreaterEquals: return left >= right;
                        case TokenType.SmallerEquals: return left <= right;
                        case TokenType.EqualsEquals: return left == right;
                        case TokenType.NotEquals: return left != right;
                        default:
                            throw new Exception($"Unknown Operator {b.Op}");
                    }
                case NumberLiteralNode n:
                    return n.Value;
                case DecimalLiteralNode d:
                    return d.Value;
                case StringLiteralNode s:
                    return s.Value;
                case BoolLiteralNode b:
                    return b.Value;
                case IdentifierNode i:
                    if (!_environment.ContainsKey(i.Name))
                    {
                        throw new Exception("Undeclared Variable");
                    }
                    return _environment[i.Name];
                default:
                    throw new Exception($"Cannot evaluate node {node.GetType().Name}");
            }
        }
        private void Execute(AstNode node)
        {
            switch (node)
            {
                case OutStatement o:
                    object value = Evaluate(o.Value);
                    Console.WriteLine(value);
                    break;
                case VarDeclaration v:
                    _environment[v.Name] = Evaluate(v.Value);
                    break;
                case WhenStatementNode w:
                    if (w.IsRepeat)
                    {
                        while ((bool)Evaluate(w.Condition))
                        {
                            foreach(AstNode a in w.Body)
                            {
                                Execute(a);
                            }
                        }
                    }
                    else
                    {
                        if ((bool)Evaluate(w.Condition))
                        {
                            foreach (AstNode a in w.Body)
                            {
                                Execute(a);
                            }
                        }
                        else if (w.Otherwise != null)
                        {
                            foreach (AstNode a in w.Otherwise)
                            {
                                Execute(a);
                            }
                        }
                    }
                    break;
                case Assignment a:
                    if(!_environment.ContainsKey(a.Name)){
                        throw new Exception("Variable doesn't exist");
                    }
                    _environment[a.Name] = Evaluate(a.Value);
                    break;
                default:
                    throw new Exception($"Cannot execute node {node.GetType().Name}");
            }
        }
    }
}