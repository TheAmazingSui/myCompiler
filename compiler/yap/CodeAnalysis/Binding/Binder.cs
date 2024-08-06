
using MYCOMPILER.CodeAnalysis.Syntax;

namespace MYCOMPILER.CodeAnalysis.Binding
{

    internal sealed class Binder{

        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
        private readonly Dictionary<string, object> _variables;

        public Binder(Dictionary<string, object> variables)
        {
            _variables = variables;
        }

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
                case SyntaxeKind.NameExpression:
                    return bindNameExpression((NameExpressionSyntax)syntax);
                case SyntaxeKind.AssignmentExpression:
                    return bindAssignmentExpression((AssignmentExpressionSyntax)syntax);

                default:
                    throw new Exception($"Unexpected Kind encountered: {syntax.Kind}");   

            }
        }

        private BoundExpression bindAssignmentExpression(AssignmentExpressionSyntax syntax)
        {
            var boundExp = bindExpression(syntax.Exp);
            var name = syntax.IdentifierToken.Text;

            var defaultValue = (object)null;
            if(boundExp.Type == typeof(int))
            {
                defaultValue = (object)0;
            }
            else if (boundExp.Type == typeof(bool))
            {
                defaultValue = (object)false;
            }
            else{
                defaultValue = null;
            }
            if(defaultValue == null)
                throw new Exception($"Unsupported type {boundExp.Type}");
            _variables[name] = defaultValue;
            return new BoundAssignmentExpression(name, boundExp);
        }

        private BoundExpression bindNameExpression(NameExpressionSyntax syntax)
        {
            var name = syntax.IdentifierToken.Text;
            if(!_variables.TryGetValue(name, out var value))
            {
                _diagnostics.ReportUndefinedName(syntax.IdentifierToken.Span, name);
                return new BoundLiteralExpression(0);
            }
            var type = value.GetType();
            return new BoundVariableExpression(name, type);
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