


namespace MYCOMPILER.CodeAnalysis
{
    sealed class BinaryExpressionSyntaxe : ExpressionSyntaxe
    {
        public BinaryExpressionSyntaxe(ExpressionSyntaxe left, SyntaxeToken operatorToken, ExpressionSyntaxe right)
        {
            Left = left;
            Right = right;
            OperatorToken = operatorToken;
        }

        public ExpressionSyntaxe Left { get; }
        public ExpressionSyntaxe Right { get; }
        public SyntaxeToken OperatorToken { get; }

        public override SyntaxeKind Kind => SyntaxeKind.BinaryExpression;

        public override IEnumerable<SyntaxeNode> GetChildren()
        {
            yield return Left;
            yield return OperatorToken;
            yield return Right;
        }
    }
}