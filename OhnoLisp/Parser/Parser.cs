using OhnoLisp.AST;
using OhnoLisp.Lexer;

namespace OhnoLisp.Parser;

public sealed class OhnoParser
{
    public INode Parse(ILexer<TokenInfo> lexer)
    {
        var children = new List<INode>();
        string? name = null;
        while (!lexer.Eof)
        {
            var token = lexer.Next();
            if (token.TokenType is TokenType.LPAREN)
            {
                var child = Parse(lexer);
                children.Add(child);
            }
            else if (token.TokenType is TokenType.RPAREN)
            {
                return Build(children, name);
            }
            else if (token.TokenType is TokenType.IDENTIFIER)
            {
                if (name is null)
                    name = token.Text;
                else
                    children.Add(new Variable(token.Text));
            }
            else if (token.TokenType is TokenType.INTEGER)
            {
                children.Add(new Integer(int.Parse(token.Text)));
            }
            else if (token.TokenType is TokenType.WHITESPACE)
            {
                // do nothing xixixi
            }
        }
        return Build(children, name);
    }

    private static INode Build(IReadOnlyList<INode> children, string? name)
    {
        if (name is null)
        {
            if (children.Count != 1)
                throw new("No name found, expected a value");
            return children[0];
        }
        return new Command(children, name);
    }
}