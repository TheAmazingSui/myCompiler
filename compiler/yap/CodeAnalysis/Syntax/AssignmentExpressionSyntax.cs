


namespace MYCOMPILER.CodeAnalysis.Syntax
{
    public sealed class AssignmentExpressionSyntax : ExpressionSyntaxe{
        public AssignmentExpressionSyntax(SyntaxeToken identifierToken,SyntaxeToken equalsToken ,ExpressionSyntaxe exp)
        {
            IdentifierToken = identifierToken;
            EqualsToken = equalsToken;
            Exp = exp;
        }

        public SyntaxeToken IdentifierToken { get;}
        public SyntaxeToken EqualsToken { get; }
        public ExpressionSyntaxe Exp { get; }

        public override SyntaxeKind Kind => SyntaxeKind.AssignmentExpression;

        public override IEnumerable<SyntaxeNode> GetChildren()
        {
            yield return IdentifierToken;
            yield return EqualsToken;
            yield return Exp;
        }
    }
}