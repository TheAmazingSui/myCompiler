namespace MYCOMPILER.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryExpression : BoundExpression
    {
        public BoundUnaryExpression(BoundExpression operand, BoundUnaryOperatorKind boundOp)
        {
            Operand = operand;
            BoundOp = boundOp;
        }

        public BoundExpression Operand { get; }
        public BoundUnaryOperatorKind BoundOp { get;}

        public override Type Type => Operand.Type;

        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
    }
}