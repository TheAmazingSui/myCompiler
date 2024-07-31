
namespace MYCOMPILER.CodeAnalysis.Syntax
{
    sealed class SyntaxTree
    {
        public SyntaxTree(IEnumerable<string> diagnostics, ExpressionSyntaxe root, SyntaxeToken endOfFile)
        {
            Root = root;
            EndOfFile = endOfFile;
            Diagnostics = diagnostics.ToArray();

        }

        public static SyntaxTree parse(string text)
        {
            var parser = new Parser(text);
            return parser.parse();
        }

        public ExpressionSyntaxe Root { get; }
        public SyntaxeToken EndOfFile { get; }
        public IReadOnlyList<string> Diagnostics { get; }
    }
}