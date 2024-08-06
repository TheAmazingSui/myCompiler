using MYCOMPILER.CodeAnalysis.Binding;

namespace MYCOMPILER.CodeAnalysis
{
    internal sealed class Evaluator
    {
        public Evaluator(BoundExpression root, Dictionary<string, object> variables)
        {
            Root = root;
            _variables = variables;
        }

        private readonly BoundExpression Root;
        private readonly Dictionary<string, object> _variables;

        public object Evaluate()
        {
            return EvaluateExpression(Root);
        }

        public object EvaluateExpression(BoundExpression node)
        {

            if (node is BoundLiteralExpression n)
            {
                return n.Value;
            }
            else if(node is BoundVariableExpression v)
            {
                return _variables[v.Name];
            }
            else if(node is BoundAssignmentExpression a)
            {
                var value = EvaluateExpression(a.BoundExp);
                _variables[a.Name] = value;
                return value;

            }
            else if(node is BoundUnaryExpression u)
            {
                var ans = EvaluateExpression(u.Operand);
                if (u.BoundOp.BoundKind == BoundUnaryOperatorKind.Identity)
                    return (int)ans;
                else if (u.BoundOp.BoundKind == BoundUnaryOperatorKind.Negation)
                    return -(int)ans;
                else if (u.BoundOp.BoundKind == BoundUnaryOperatorKind.LogicalNegation)
                    return !(bool)ans;
                else
                    throw new Exception($"Unexpected operator: '{u.BoundOp}!");
                
            }
            else if (node is BoundBinaryExpression b)
            {
                var lft = EvaluateExpression(b.Left);
                var rgt = EvaluateExpression(b.Right);

                switch (b.Op.BoundOP)
                {
                    case BoundBinaryOperatorKind.Addition:
                        return (int)lft + (int)rgt;
                    case BoundBinaryOperatorKind.Subtraction:
                        return (int)lft - (int)rgt;
                    case BoundBinaryOperatorKind.Multiplication:
                        return (int)lft * (int)rgt;
                    case BoundBinaryOperatorKind.Division:
                        return (int)lft / (int)rgt;
                    case BoundBinaryOperatorKind.LogicalAnd:
                        return (bool)lft && (bool)rgt;
                    case BoundBinaryOperatorKind.LogicalOr:
                        return (bool)lft || (bool)rgt;
                    case BoundBinaryOperatorKind.Equality:
                        return Equals(lft, rgt);
                    case BoundBinaryOperatorKind.Inequality:
                        return !Equals(lft, rgt);
                    default:
                        throw new Exception($"Unexpected operator: '{b.Op.BoundOP}!");
                }
            }
            //else if (node is ParenthesizedExpressionSyntax p)
            //{
                //return EvaluateExpression(p.NumExp);
            //}
            throw new Exception($"Unexpected node: {node.Kind}");

        }
    }
}