using MYCOMPILER.CodeAnalysis.Syntax;

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

        public int Evaluate()
        {
            return EvaluateExpression(Root);
        }

        public int EvaluateExpression(BoundExpression root)
        {

            if (root is BoundLiteralExpression n)
            {
                return (int)n.Value;
            }
            else if(root is BoundUnaryExpression u)
            {
                var ans = EvaluateExpression(u.Operand);
                if (u.BoundOp == BoundUnaryOperatorKind.Identity)
                    return ans;
                else if (u.BoundOp == BoundUnaryOperatorKind.Negation)
                    return -ans;
                else
                    throw new Exception($"Unexpected operator: '{u.BoundOp}!");
                
            }
            else if (root is BoundBinaryExpression b)
            {
                var lft = EvaluateExpression(b.Left);
                var rgt = EvaluateExpression(b.Right);

                if (b.OpKind == BoundBinaryOperatorKind.Addition)
                {
                    return lft + rgt;
                }
                else if (b.OpKind == BoundBinaryOperatorKind.Subtraction)
                {
                    return lft - rgt;
                }
                else if (b.OpKind == BoundBinaryOperatorKind.Multiplication)
                {
                    return lft * rgt;
                }
                else if (b.OpKind == BoundBinaryOperatorKind.Division)
                {
                    return lft / rgt;
                }
                else
                {
                    throw new Exception($"Unexpected operator: '{b.OpKind}!");
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