

namespace MYCOMPILER.CodeAnalysis
{
    sealed class LiteralExpressionSyntaxe : ExpressionSyntaxe
    {
        public LiteralExpressionSyntaxe(SyntaxeToken literalToken)
        {
            LiteralToken = literalToken;
        }

        public SyntaxeToken LiteralToken { get; }
        public override SyntaxeKind Kind => SyntaxeKind.LiteralExpression;

        public override IEnumerable<SyntaxeNode> GetChildren()
        {
            yield return LiteralToken;
        }
    }
}