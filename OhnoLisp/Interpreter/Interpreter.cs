using OhnoLisp.AST;

namespace OhnoLisp.Interpreter;

public sealed class OhnoInterpreter
{
    public int Run(INode code, Dictionary<string, int> arguments)
    {
        if (code is Integer i)
            return i.Value;
        if (code is Variable v)
        {
            return arguments[v.Name];
        }
        var cmd = (Command)code;
        return cmd.CommandName switch
        {
            "add" => Run(code.Children[0], arguments) + Run(code.Children[1], arguments),
            "mul" => Run(code.Children[0], arguments) * Run(code.Children[1], arguments),
            "print" => Print(Run(code.Children[0], arguments)),
            _ => throw new($"Unexpected command {cmd.CommandName}")
        };

        static int Print(int a)
        {
            Console.WriteLine(a);
            return a;
        }
    }
}