


namespace MYCOMPILER.CodeAnalysis.Syntax
{
    public sealed class NameExpressionSyntax : ExpressionSyntaxe{
        public NameExpressionSyntax(SyntaxeToken identifierToken)
        {
            IdentifierToken = identifierToken;
        }

        public SyntaxeToken IdentifierToken { get;}

        public override SyntaxeKind Kind => SyntaxeKind.NameExpression;

        public override IEnumerable<SyntaxeNode> GetChildren()
        {
            yield return IdentifierToken;
        }
    }
}