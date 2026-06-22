using Dirt.Lexer;
using Dirt.Parser.Nodes;
using System.Text.Json;

namespace Dirt.Interpreter
{
    public class Interpreter
    {
        private Dictionary<string, object> _environment = new Dictionary<string, object>();

        private Dictionary<string, FunctionDeclaration> _functions = new Dictionary<string, FunctionDeclaration>();

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
                case FunctionCall fc:
                    if (!_functions.ContainsKey(fc.Name))
                        throw new Exception($"Undefined function '{fc.Name}'");
                    FunctionDeclaration func = _functions[fc.Name];

                    Dictionary<string, object> newScope = new Dictionary<string, object>();
                    for (int i = 0; i < func.Parameters.Count; i++)
                    {
                        object argValue = Evaluate(fc.Args[i]);
                        newScope[func.Parameters[i].Name] = argValue;
                    }
                    Dictionary<string, object> oldScope = _environment;
                    _environment = newScope;
                    object? result = null;
                    try
                    {
                        foreach (AstNode stmt in func.Body)
                            Execute(stmt);
                    }
                    catch (StopSignal signal)
                    {
                        result = signal.Value;
                    }
                    _environment = oldScope;

                    return result!;
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
                            try
                            {
                                foreach (AstNode a in w.Body)
                                    Execute(a);
                            }
                            catch (StopSignal)
                            {
                                break;
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
                case RepeatStatementNode r:
                    if (r.Count == null)
                    {
                        while (true)
                        {
                            try
                            {
                                foreach (AstNode a in r.Body)
                                    Execute(a);
                            }
                            catch (StopSignal)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        int count = (int)Evaluate(r.Count);
                        for (int i = 0; i < count; i++)
                        {
                            try
                            {
                                foreach (AstNode a in r.Body)
                                    Execute(a);
                            }
                            catch (StopSignal)
                            {
                                break;
                            }
                        }
                    }
                    break;
                case StopStatementNode s:
                    object? stopValue = null;
                    if(s.Value != null)
                    {
                        stopValue = Evaluate(s.Value);
                    }
                    throw new StopSignal(stopValue);

                case MatchStatementNode m:
                    object matchValue = Evaluate(m.Value);
                    foreach (MatchArm arm in m.Arms)
                    {
                        if (Matches(arm.Pattern, matchValue))
                        {
                            foreach (AstNode a in arm.Body)
                                Execute(a);
                            break;
                        }
                    }
                    break;
                case FunctionDeclaration f:
                    _functions[f.Name] = f;
                    break;
                default:
                    throw new Exception($"Cannot execute node {node.GetType().Name}");
            }
        }
        private bool Matches(AstNode pattern, object value)
        {
            switch (pattern)
            {
                case WildcardPattern:
                    return true;
                case RangePattern r:
                    int from = (int)Evaluate(r.From);
                    int to = (int)Evaluate(r.To);
                    int val = (int)value;
                    return val >= from && val <= to;
                default:
                    object patternValue = Evaluate(pattern);
                    return value.Equals(patternValue);
            }
        }
    }
}