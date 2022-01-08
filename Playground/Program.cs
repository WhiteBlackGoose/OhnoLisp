// See https://aka.ms/new-console-template for more information
using OhnoLisp.Compiler;
using OhnoLisp.Interpreter;
using OhnoLisp.Lexer;
using OhnoLisp.Parser;

var source = @"
(print
    (add
        (mul 3 b)
        a
    )
)
";

var lexer = new OhnoLexer(source);
var parser = new OhnoParser();
var ast = parser.Parse(lexer);

var interpreter = new OhnoInterpreter();
interpreter.Run(ast, new() { ["a"] = 5, ["b"] = 10 });

var compiler = new OhnoCompiler();
var func = compiler.Compile(ast, "a", "b");
func(5, 10);
