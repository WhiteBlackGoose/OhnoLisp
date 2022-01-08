using System.Reflection.Emit;
using OhnoLisp.AST;

namespace OhnoLisp.Compiler;

public sealed class OhnoCompiler
{
    private void GenerateIL(ILGenerator gen, INode source, Dictionary<string, int> orderOfVariables)
    {
        switch (source)
        {
            case Integer i:
                gen.Emit(OpCodes.Ldc_I4, i.Value);
                break;
            case Variable v:
                gen.Emit(OpCodes.Ldarg, orderOfVariables[v.Name]);
                break;
            case Command cmd:
                for (int i = cmd.Children.Count - 1; i >= 0; i--)
                    GenerateIL(gen, cmd.Children[i], orderOfVariables);
                switch (cmd.CommandName)
                {
                    case "add":
                        gen.Emit(OpCodes.Add);
                        break;
                    case "mul":
                        gen.Emit(OpCodes.Mul);
                        break;
                    case "print":
                        gen.Emit(OpCodes.Dup);
                        gen.EmitCall(OpCodes.Call, typeof(Console).GetMethod("WriteLine", 0, new [] { typeof(int) }) ?? throw new(), null);
                        break;
                }
                break;
        }
    }

    private T CompileToDelegate<T>(INode source, IReadOnlyList<string> variables) where T : Delegate
    {
        var dict = new Dictionary<string, int>();
        for (int i = 0; i < variables.Count; i++)
            dict[variables[i]] = i;

        var method = new DynamicMethod(Guid.NewGuid().ToString(), typeof(int), variables.Select(_ => typeof(int)).ToArray());
        var gen = method.GetILGenerator();
        GenerateIL(gen, source, dict);
        gen.Emit(OpCodes.Ret);
        return method.CreateDelegate<T>();
    }

    public Func<int, int> Compile(INode source, string var1)
    {
        return CompileToDelegate<Func<int, int>>(source, new [] { var1 });
    }

    public Func<int, int, int> Compile(INode source, string var1, string var2)
    {
        return CompileToDelegate<Func<int, int, int>>(source, new [] { var1, var2 });
    }
}