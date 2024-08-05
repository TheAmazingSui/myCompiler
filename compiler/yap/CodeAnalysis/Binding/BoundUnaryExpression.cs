namespace MYCOMPILER.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryExpression : BoundExpression
    {
        public BoundUnaryExpression(BoundExpression operand, BoundUnaryOperator boundOp)
        {
            Operand = operand;
            BoundOp = boundOp;
        }

        public BoundExpression Operand { get; }
        public BoundUnaryOperator BoundOp { get;}

        public override Type Type => BoundOp.ResType;

        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
    }
}