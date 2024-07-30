

namespace MYCOMPILER.CodeAnalysis
{
    class Evaluator
    {
        public Evaluator(ExpressionSyntaxe root)
        {
            Root = root;
        }

        public ExpressionSyntaxe Root { get; }

        public int Evaluate()
        {
            return EvaluateExpression(Root);
        }

        public int EvaluateExpression(ExpressionSyntaxe root)
        {

            if (root is LiteralExpressionSyntaxe n)
            {
                return (int)n.LiteralToken.Value;
            }
            else if(root is UnaryExpressionSyntaxe u)
            {
                var ans = EvaluateExpression(u.Operand);
                if (u.OperatorToken.Kind == SyntaxeKind.PlusToken)
                    return ans;
                else if (u.OperatorToken.Kind == SyntaxeKind.MinusToken)
                    return -ans;
                else
                    throw new Exception($"Unexpected operator: '{u.OperatorToken.Kind}!");
                
            }
            else if (root is BinaryExpressionSyntaxe b)
            {
                var lft = EvaluateExpression(b.Left);
                var rgt = EvaluateExpression(b.Right);

                if (b.OperatorToken.Kind == SyntaxeKind.PlusToken)
                {
                    return lft + rgt;
                }
                else if (b.OperatorToken.Kind == SyntaxeKind.MinusToken)
                {
                    return lft - rgt;
                }
                else if (b.OperatorToken.Kind == SyntaxeKind.TimesToken)
                {
                    return lft * rgt;
                }
                else if (b.OperatorToken.Kind == SyntaxeKind.DivideToken)
                {
                    return lft / rgt;
                }
                else
                {
                    throw new Exception($"Unexpected operator: '{b.OperatorToken.Kind}!");
                }
            }
            else if (root is ParenthesizedExpressionSyntax p)
            {
                return EvaluateExpression(p.NumExp);
            }
            throw new Exception($"Unexpected node: {root.Kind}");

        }
    }
}