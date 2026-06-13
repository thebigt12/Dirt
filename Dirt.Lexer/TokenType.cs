namespace Dirt.Lexer
{
    public enum TokenType
    {
        // Keywords
        When,
        Otherwise,
        Repeat,
        Match,
        Stop,
        And,
        Or,
        Not,
        Output,
        // Types
        IntegerType,
        DecimalType,
        StringType,
        BoolType,
        // Literals
        NumberLiteral,
        DecimalLiteral,
        StringLiteral,
        True,
        False,
        // Symbols
        Colon,
        Equals,
        Plus,
        Minus,
        Star,
        Slash,
        LBracket,
        RBracket,
        LBrace,
        RBrace,
        Arrow,
        DoubleDot,
        Comma,
        Underscore,
        // Comparators
        EqualsEquals,
        Greater,
        GreaterEquals,
        Smaller,
        SmallerEquals,
        NotEquals,
        // Others
        Identifier,
        Newline,
        EOF
    }
}