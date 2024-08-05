
namespace MYCOMPILER.CodeAnalysis.Syntax
{
    public sealed class UnaryExpressionSyntaxe : ExpressionSyntaxe{
        public UnaryExpressionSyntaxe(ExpressionSyntaxe operand, SyntaxeToken operatorToken)
        {
            Operand = operand;
            OperatorToken = operatorToken;
        }

        public override SyntaxeKind Kind => SyntaxeKind.UnaryExpression;

        public ExpressionSyntaxe Operand { get; }
        public SyntaxeToken OperatorToken { get; }

        public override IEnumerable<SyntaxeNode> GetChildren()
        {
            yield return OperatorToken;
            yield return Operand;

        }
    }
}