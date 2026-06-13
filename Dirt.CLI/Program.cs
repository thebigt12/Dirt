using Dirt.Lexer;
using Dirt.Parser;
using Dirt.Parser.Nodes;

string source = "match (x) {\n1 -> {\nout(\"eins\")\n}\n2..5 -> {\nout(\"zwei\")\n}\n_ -> {\nout(\"rest\")\n}\n}";
Lexer lexer = new Lexer(source);
List<Token> tokens = lexer.Tokenize();

Parser parser = new Parser(tokens);
List<AstNode> ast = parser.Parse();

foreach(AstNode node in ast)
{
    Print(node, 0);
}

void Print(AstNode node, int indent)
{
    string pad = new string(' ', indent * 2);
    switch (node)
    {
        case VarDeclaration v:
            Console.WriteLine($"{pad}VarDeclaration: {v.Name} ({v.Type})");
            Print(v.Value, indent + 1);
            break;
        case BinaryExpression b:
            Console.WriteLine($"{pad}BinaryExpression: {b.Op}");
            Print(b.Left, indent + 1);
            Print(b.Right, indent + 1);
            break;
        case NumberLiteralNode n:
            Console.WriteLine($"{pad}Number: {n.Value}");
            break;
        case DecimalLiteralNode d:
            Console.WriteLine($"{pad}Decimal: {d.Value}");
            break;
        case StringLiteralNode s:
            Console.WriteLine($"{pad}String: {s.Value}");
            break;
        case BoolLiteralNode bo:
            Console.WriteLine($"{pad}Bool: {bo.Value}");
            break;
        case OutStatement o:
            Console.WriteLine($"{pad}OutStatement");
            Print(o.Value, indent + 1);
            break;
        case IdentifierNode vr:
            Console.WriteLine($"{pad}Variable: {vr.Name}");
            break;
        case WhenStatementNode w:
            Console.WriteLine($"{pad}When (IsRepeat: {w.IsRepeat})");
            Console.WriteLine($"{pad}  Condition:");
            Print(w.Condition, indent + 2);
            Console.WriteLine($"{pad}  Body:");
            foreach (AstNode stmt in w.Body)
                Print(stmt, indent + 2);
            if (w.Otherwise != null)
            {
                Console.WriteLine($"{pad}  Otherwise:");
                foreach (AstNode stmt in w.Otherwise)
                    Print(stmt, indent + 2);
            }
            break;
        case RepeatStatementNode r:
            Console.WriteLine($"{pad}Repeat (Count: {(r.Count == null ? "infinite" : "")})");
            if (r.Count != null)
                Print(r.Count, indent + 1);
            Console.WriteLine($"{pad}  Body:");
            foreach (AstNode stmt in r.Body)
                Print(stmt, indent + 2);
            break;
        case MatchStatementNode m:
            Console.WriteLine($"{pad}Match");
            Console.WriteLine($"{pad}  Value:");
            Print(m.Value, indent + 2);
            foreach (MatchArm arm in m.Arms)
            {
                Console.WriteLine($"{pad}  Arm:");
                Console.WriteLine($"{pad}    Pattern:");
                Print(arm.Pattern, indent + 3);
                Console.WriteLine($"{pad}    Body:");
                foreach (AstNode stmt in arm.Body)
                    Print(stmt, indent + 3);
            }
            break;
        case RangePattern rp:
            Console.WriteLine($"{pad}Range:");
            Print(rp.From, indent + 1);
            Print(rp.To, indent + 1);
            break;
        case WildcardPattern:
            Console.WriteLine($"{pad}Wildcard (_)");
            break;
    }
}