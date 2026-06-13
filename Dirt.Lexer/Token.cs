namespace Dirt.Lexer
{
    public class Token
    {
        public TokenType TokenType { get; set; }
        public string Value { get; set; } = "";
        public int Line { get; set; }

        public Token(TokenType type, string value, int line)
        {
            TokenType = type;
            Value = value;
            Line = line;
        }
    }
}