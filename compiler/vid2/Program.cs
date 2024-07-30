using System;

using MYCOMPILER.CodeAnalysis;


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
                var color = Console.ForegroundColor;
                if(showTree)
                {
                    
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    PrettyPrint(exp.Root);
                    Console.ForegroundColor = color;
                }
                

                if(exp.Diagnostics.Any())
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    foreach(var e in exp.Diagnostics)
                    {
                        Console.WriteLine(e);
                    }
                    Console.ForegroundColor = color;
                }
                else{
                    Evaluator eval = new Evaluator(exp.Root);
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
