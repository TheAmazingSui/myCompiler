using MYCOMPILER.CodeAnalysis.Syntax;
using MYCOMPILER.CodeAnalysis;
using MYCOMPILER.CodeAnalysis.Binding;

namespace vid3
{
    class Program
    {
         static void Main(string[] args)
        {
            bool showTree = false;
            Dictionary<VariableSymbol,object> variables = new Dictionary<VariableSymbol,object>();
            while(true)
            {
                Console.Write(">");
                var line = Console.ReadLine();
                if(string.IsNullOrWhiteSpace(line))
                {
                    return;
                }

                if (line == "#showTree")
                {
                    showTree = !showTree;
                    Console.WriteLine(showTree ? "Showing parse trees:" : "Not showing parse Trees");
                    continue;
                }

                //Lexer lexer = new Lexer(line);
                SyntaxTree exp = SyntaxTree.parse(line);
                var compilation = new Compilation(exp);
                var result = compilation.Evaluate(variables);
                var diagnostics = result.Diagnostics;

                if(showTree)
                {
                    
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    PrettyPrint(exp.Root);
                    Console.ResetColor();
                }
                
                if(diagnostics.Any())
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    foreach(var e in diagnostics)
                    {

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(e);
                        Console.ResetColor();

                        var prefix = line.Substring(0, e.Span.Start);
                        var error = line.Substring(e.Span.Start, e.Span.Length);
                        var suffix = line.Substring(e.Span.End);

                        Console.WriteLine("    ");
                        Console.Write(prefix);

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(error);
                        Console.ResetColor();


                        Console.Write(suffix);
                        Console.WriteLine();
                    }
                    Console.ResetColor();
                }
                else{
                    Console.WriteLine(result.Value);

                }
            }
        }

        static void PrettyPrint(SyntaxeNode node, string indent = "")
        {
            Console.Write(indent);
            Console.Write(node.Kind);

            if(node is SyntaxeToken t && t.Value != null)
            {
                Console.Write(" ");
                Console.Write(t.Value);
            }

            Console.WriteLine();
            indent += "    ";

            foreach(var child in node.GetChildren())
            {
                PrettyPrint(child, indent);
            }
            return;

        }
    }
}
