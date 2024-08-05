
namespace MYCOMPILER.CodeAnalysis.Syntax
{
    public sealed class SyntaxTree
    {
        public SyntaxTree(IEnumerable<Diagnostic> diagnostics, ExpressionSyntaxe root, SyntaxeToken endOfFile)
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
        public IReadOnlyList<Diagnostic> Diagnostics { get; }
    }
}