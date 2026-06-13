namespace Dirt.Lexer
{
    public class Lexer
    {
        private readonly Dictionary<string, TokenType> _keyWords = new Dictionary<string, TokenType>()
        {
            {"when", TokenType.When },
            {"otherwise", TokenType.Otherwise },
            {"repeat", TokenType.Repeat },
            {"match", TokenType.Match },
            {"stop", TokenType.Stop },
            {"and", TokenType.And },
            {"or", TokenType.Or },
            {"not", TokenType.Not },
            {"out", TokenType.Output },
            {"int", TokenType.IntegerType },
            {"dec", TokenType.DecimalType },
            {"string", TokenType.StringType },
            {"bool", TokenType.BoolType },
            {"true", TokenType.True },
            {"false", TokenType.False }
        };

        private string _source;
        private int _index;
        private int _line;

        public Lexer(string source)
        {
            _source = source;
            _index = 0;
            _line = 1;
        }

        public List<Token> Tokenize()
        {
            List<Token> tokens = new List<Token>();
            while (_index < _source.Length)
            {
                char current = _source[_index];
                switch (current)
                {
                    //cat 1 cases
                    case '_':AddToken(tokens, TokenType.Underscore, "_");break;
                    case ':': AddToken(tokens, TokenType.Colon, ":"); break;
                    case '+': AddToken(tokens, TokenType.Plus, "+"); break;
                    case '(': AddToken(tokens, TokenType.LBracket, "("); break;
                    case ')': AddToken(tokens, TokenType.RBracket, ")"); break;
                    case '{': AddToken(tokens, TokenType.LBrace, "{"); break;
                    case '}': AddToken(tokens, TokenType.RBrace, "}"); break;
                    case '*': AddToken(tokens, TokenType.Star, "*"); break;
                    case ',': AddToken(tokens, TokenType.Comma, ","); break;
                    case '"': ReadString(tokens); break;
                    case '=':
                        //cat 2 cases
                        if (_index + 1 < _source.Length && _source[_index + 1] == '=')
                        { AddToken(tokens, TokenType.EqualsEquals, "=="); _index++; }
                        else AddToken(tokens, TokenType.Equals, "=");
                        break;
                    case '-':
                        if (_index + 1 < _source.Length && _source[_index + 1] == '>')
                        { AddToken(tokens, TokenType.Arrow, "->"); _index++; }
                        else AddToken(tokens, TokenType.Minus, "-");
                        break;
                    case '.':
                        if (_index + 1 < _source.Length && _source[_index + 1] == '.')
                        { AddToken(tokens, TokenType.DoubleDot, ".."); _index++; }
                        else throw new Exception($"Unexpected character '.' on line {_line}");
                        break;
                    case '/':
                        if (_index + 1 < _source.Length && _source[_index + 1] == '/')
                        { while (_index < _source.Length && _source[_index] != '\n') _index++; }
                        else AddToken(tokens, TokenType.Slash, "/");
                        break;
                    case '<':
                        if (_index + 1 < _source.Length && _source[_index + 1] == '=')
                        { AddToken(tokens, TokenType.SmallerEquals, "<="); _index++; }
                        else AddToken(tokens, TokenType.Smaller, "<");
                        break;
                    case '>':
                        if (_index + 1 < _source.Length && _source[_index + 1] == '=')
                        { AddToken(tokens, TokenType.GreaterEquals, ">="); _index++; }
                        else AddToken(tokens, TokenType.Greater, ">");
                        break;
                    case '!':
                        if (_index + 1 < _source.Length && _source[_index + 1] == '=')
                        { AddToken(tokens, TokenType.NotEquals, "!="); _index++; }
                        else AddToken(tokens, TokenType.Not, "!");
                        break;
                    case '\n':
                        AddToken(tokens, TokenType.Newline, "\n");
                        _line++;
                        break;
                    case ' ':
                    case '\t':
                        break;
                        // cat 3 cases
                    default:
                        if (char.IsLetter(current)) ReadIdentifier(tokens);
                        else if (char.IsDigit(current)) ReadNumbers(tokens);
                        else throw new Exception($"Unexpected character '{current}' on line {_line}");
                        break;
                }
                _index++;
            }
            tokens.Add(new Token(TokenType.EOF, "", _line));
            return tokens;
        }

        private void AddToken(List<Token> list, TokenType type, string value)
        {
            list.Add(new Token(type, value, _line));
        }

        private void ReadIdentifier(List<Token> list)
        {
            string collector = "";
            while (_index < _source.Length && char.IsLetter(_source[_index]))
                collector += _source[_index++];
            _index--;
            if (_keyWords.TryGetValue(collector, out TokenType type))
                AddToken(list, type, collector);
            else
                AddToken(list, TokenType.Identifier, collector);
        }

        private void ReadNumbers(List<Token> list)
        {
            string collector = "";
            while (_index < _source.Length && (char.IsDigit(_source[_index]) ||
                  (_source[_index] == '.' && _index + 1 < _source.Length && char.IsDigit(_source[_index + 1]))))
                collector += _source[_index++];
            _index--;
            if (collector.Contains('.'))
                AddToken(list, TokenType.DecimalLiteral, collector);
            else
                AddToken(list, TokenType.NumberLiteral, collector);
        }

        private void ReadString(List<Token> list)
        {
            string collector = "";
            _index++;
            while (_index < _source.Length && _source[_index] != '"' && _source[_index] != '\n')
                collector += _source[_index++];
            if (_index >= _source.Length || _source[_index] == '\n')
                throw new Exception($"Unterminated string on line {_line}");
            _index++;
            _index--;
            AddToken(list, TokenType.StringLiteral, collector);
        }
    }
}