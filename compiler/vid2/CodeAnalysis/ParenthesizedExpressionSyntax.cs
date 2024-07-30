

namespace MYCOMPILER.CodeAnalysis
{
    sealed class ParenthesizedExpressionSyntax : ExpressionSyntaxe
    {

        public ParenthesizedExpressionSyntax(SyntaxeToken openParenthesis, ExpressionSyntaxe numExp, SyntaxeToken closedParenthesis)
        {
            OpenParenthesis = openParenthesis;
            NumExp = numExp;
            ClosedParenthesis = closedParenthesis;
        }

        public SyntaxeToken OpenParenthesis { get; }
        public ExpressionSyntaxe NumExp { get; }
        public SyntaxeToken ClosedParenthesis { get; }

        public override SyntaxeKind Kind => SyntaxeKind.ParenthesizedExpression;

        public override IEnumerable<SyntaxeNode> GetChildren()
        {
            yield return OpenParenthesis;
            yield return NumExp;
            yield return ClosedParenthesis;

        }
    }
}