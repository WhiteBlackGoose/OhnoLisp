# Oh, no! LISP

C# implementation of something looking like Lisp idk, too lazy to read the wikipage about what LISP is.

It only supports commands `add`, `mul`, and `print` and integers as values.

So um, it has tokens, lexer, AST, parser, interpreter, and compiler to IL.

## What it can

E. g. take this code:
```cs
var source = @"
(print
    (add
        (mul 3 b)
        a
    )
)
";
```

Now let's create lexer for it:
```cs
var lexer = new OhnoLexer(source);
```

We can go over it with `lexer.Eof` and `lexer.Next()`, but we can just trust the parser:

```cs
var parser = new OhnoParser();
var ast = parser.Parse(lexer);
```

`ast` is of type `INode` which can be `Command`, `Integer`, or `Variable`.

Now we can run an interpreter of our AST passing in the values of arguments:
```cs
var interpreter = new OhnoInterpreter();
interpreter.Run(ast, new() { ["a"] = 5, ["b"] = 10 });
```

Or we can compile it to IL and run the delegate:
```cs
var compiler = new OhnoCompiler();
Func<int, int, int> func = compiler.Compile(ast, "a", "b");
func(5, 10);
```

## Why???

Because how else would you spend an evening???

Wrote this thing in 1.5 hour, gotta get back to reading a book.
