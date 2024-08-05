using MYCOMPILER.CodeAnalysis.Binding;

namespace MYCOMPILER.CodeAnalysis
{
    internal sealed class Evaluator
    {
        public Evaluator(BoundExpression root)
        {
            Root = root;
        }

        private readonly BoundExpression Root;

        public object Evaluate()
        {
            return EvaluateExpression(Root);
        }

        public object EvaluateExpression(BoundExpression root)
        {

            if (root is BoundLiteralExpression n)
            {
                return n.Value;
            }
            else if(root is BoundUnaryExpression u)
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
            else if (root is BoundBinaryExpression b)
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
            //else if (root is ParenthesizedExpressionSyntax p)
            //{
                //return EvaluateExpression(p.NumExp);
            //}
            throw new Exception($"Unexpected node: {root.Kind}");

        }
    }
}