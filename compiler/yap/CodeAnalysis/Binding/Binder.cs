
using MYCOMPILER.CodeAnalysis.Syntax;

namespace MYCOMPILER.CodeAnalysis.Binding
{

    internal sealed class Binder{

        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();

        public DiagnosticBag Diagnostics => _diagnostics;


        public BoundExpression bindExpression(ExpressionSyntaxe syntax)
        {
            switch(syntax.Kind)
            {
                case SyntaxeKind.LiteralExpression:
                    return bindLiteralExpression((LiteralExpressionSyntaxe)syntax);
                case SyntaxeKind.UnaryExpression:
                    return bindUnaryExpression((UnaryExpressionSyntaxe)syntax);
                case SyntaxeKind.BinaryExpression:
                    return bindBinaryExpression((BinaryExpressionSyntaxe)syntax);
                case SyntaxeKind.ParenthesizedExpression:
                    return bindExpression(((ParenthesizedExpressionSyntax)syntax).NumExp);
                default:
                    throw new Exception($"Unexpected Kind encountered: {syntax.Kind}");   

            }
        }

        private BoundExpression bindBinaryExpression(BinaryExpressionSyntaxe syntax)
        {
            var boundLeft = bindExpression(syntax.Left);
            var boundRight = bindExpression(syntax.Right);
            var boundBinaryOp = BoundBinaryOperator.bind(syntax.OperatorToken.Kind,boundLeft.Type,boundRight.Type);
            if(boundBinaryOp == null)
            {
                _diagnostics.ReportUndefinedBinaryOperator(syntax.OperatorToken.Span, syntax.OperatorToken.Text,boundLeft.Type, boundRight.Type);
                return boundLeft;
            }
            return new BoundBinaryExpression(boundLeft, boundBinaryOp, boundRight); 
        }

        private BoundExpression bindUnaryExpression(UnaryExpressionSyntaxe syntax)
        {
            var boundOperand = bindExpression(syntax.Operand);
            var boundOp = BoundUnaryOperator.bind(syntax.OperatorToken.Kind, boundOperand.Type);
            if(boundOp == null)
            {
                _diagnostics.ReportUndefinedUnaryOperator(syntax.OperatorToken.Span, syntax.OperatorToken.Text , boundOperand.Type);
                return boundOperand;
            }
            return new BoundUnaryExpression(boundOperand, boundOp); 
        }

        private BoundExpression bindLiteralExpression(LiteralExpressionSyntaxe syntax)
        {
            var value = syntax.Value ?? 0;
            return new BoundLiteralExpression(value);
        }
    }
}