using System;

using MYCOMPILER.CodeAnalysis.Syntax;
using MYCOMPILER.CodeAnalysis;
using MYCOMPILER.CodeAnalysis.Binding;

namespace vid2
{
    class Program
    {
         static void Main(string[] args)
        {
            bool showTree = false;
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
                var binder = new Binder();
                var boundExp = binder.bindExpression(exp.Root);
                var diagnostics = exp.Diagnostics.Concat(binder.Diagnostics).ToArray();

                if(showTree)
                {
                    
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    PrettyPrint(exp.Root);
                    Console.ResetColor();
                }
                
                if(diagnostics.Any())
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    foreach(var e in exp.Diagnostics)
                    {
                        Console.WriteLine(e);
                    }
                    Console.ResetColor();
                }
                else{
                    Evaluator eval = new Evaluator(boundExp);
                    var result = eval.Evaluate();
                    Console.WriteLine(result);

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
