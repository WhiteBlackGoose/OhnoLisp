using System.Text;

namespace OhnoLisp.Lexer;

public interface ILexer<TToken>
{
    bool Eof { get; }

    TToken Next();
}

public sealed class OhnoLexer : ILexer<TokenInfo>
{
    private readonly string source;
    private int curr;
    public OhnoLexer(string s)
    {
        source = s;
        curr = 0;
    }

    public bool Eof => curr >= source.Length;

    public TokenInfo Next()
        => source[curr++] switch
        {
            '(' => new(TokenType.LPAREN, "("),
            ')' => new(TokenType.RPAREN, ")"),
            >= '0' and <= '9' => new(TokenType.INTEGER, GoAsLongAs(c => c is >= '0' and <= '9')),
            >= 'A' and <= 'z' => new(TokenType.IDENTIFIER, GoAsLongAs(c => c is >= 'A' and <= 'z')),
            '\r' or '\n' or ' ' => new(TokenType.WHITESPACE, GoAsLongAs(c => c is '\r' or '\n' or ' ')),
            _ => throw new("ohno!")
        };
    
    private string GoAsLongAs(Func<char, bool> condition)
    {
        var sb = new StringBuilder();
        sb.Append(source[curr - 1]);
        while (!Eof && condition(source[curr]))
        {
            sb.Append(source[curr]);
            curr++;
        }
        return sb.ToString();
    }
}