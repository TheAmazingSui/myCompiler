

namespace MYCOMPILER.CodeAnalysis.Syntax
{
    sealed class LiteralExpressionSyntaxe : ExpressionSyntaxe
    {

        public LiteralExpressionSyntaxe(SyntaxeToken literalToken) : this(literalToken, literalToken.Value)
        {

        } 
        public LiteralExpressionSyntaxe(SyntaxeToken literalToken, object value)
        {
            LiteralToken = literalToken;
            Value = value; 
        }

        public SyntaxeToken LiteralToken { get; }
        public object Value { get; }

        public override SyntaxeKind Kind => SyntaxeKind.LiteralExpression;

        public override IEnumerable<SyntaxeNode> GetChildren()
        {
            yield return LiteralToken;
        }
    }
}