
using MYCOMPILER.CodeAnalysis.Syntax;

namespace MYCOMPILER.CodeAnalysis.Binding
{

    internal sealed class Binder{

        private readonly List<string> _diagnostics = new List<string>();

        public IEnumerable<string> Diagnostics => _diagnostics;


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
                _diagnostics.Add($"Binary operator '{syntax.OperatorToken.Text}' is not defined for type {boundLeft.Type} and {boundRight.Type}");
                return boundLeft;
            }
            return new BoundBinaryExpression(boundLeft, boundBinaryOp, boundRight); 
        }

        private BoundBinaryOperatorKind? bindBinaryOperatorKind(SyntaxeKind kind, Type leftType, Type rightType)
        {
            if (leftType == typeof(int) && rightType == typeof(int))
            {
                switch (kind)
                {
                    case SyntaxeKind.PlusToken:
                        return BoundBinaryOperatorKind.Addition;
                    case SyntaxeKind.MinusToken:
                        return BoundBinaryOperatorKind.Subtraction;
                    case SyntaxeKind.TimesToken:
                        return BoundBinaryOperatorKind.Multiplication;
                    case SyntaxeKind.DivideToken:
                        return BoundBinaryOperatorKind.Division;
                    default:
                        throw new Exception($"ERROR: Unrecognized binary operator: {kind}");
                }
            }
            else if (leftType == typeof(bool) && rightType == typeof(bool))
            {
                switch (kind)
                {
                    case SyntaxeKind.LogicalAndToken:
                        return BoundBinaryOperatorKind.LogicalAnd;
                    case SyntaxeKind.LogicalOrToken:
                        return BoundBinaryOperatorKind.LogicalOr;
                    default:
                        throw new Exception($"ERROR: Unrecognized binary operator: {kind}");
                }
            }
            else return null;

        }

        private BoundExpression bindUnaryExpression(UnaryExpressionSyntaxe syntax)
        {
            var boundOperand = bindExpression(syntax.Operand);
            var boundOp = BoundUnaryOperator.bind(syntax.OperatorToken.Kind, boundOperand.Type);
            if(boundOp == null)
            {
                _diagnostics.Add($"Unary operator '{syntax.OperatorToken.Text}' is not defined for type {boundOperand.Type}");
                return boundOperand;
            }
            return new BoundUnaryExpression(boundOperand, boundOp); 
        }

        private BoundUnaryOperatorKind? bindUnaryOperatorKind(SyntaxeKind kind, Type operandtype)
        {
            if (operandtype == typeof(int))
            {
                switch(kind)
                {
                    case SyntaxeKind.PlusToken:
                        return BoundUnaryOperatorKind.Identity;
                    case SyntaxeKind.MinusToken:
                        return BoundUnaryOperatorKind.Negation;
                    default:
                        throw new Exception($"ERROR: Incorrect unary operator: {kind}");
                }
            }
            else if(operandtype == typeof(bool))
            {
                if (kind == SyntaxeKind.BangToken) return BoundUnaryOperatorKind.LogicalNegation;
            }
            return null;
        }

        private BoundExpression bindLiteralExpression(LiteralExpressionSyntaxe syntax)
        {
            var value = syntax.Value ?? 0;
            return new BoundLiteralExpression(value);
        }
    }
}