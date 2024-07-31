namespace MYCOMPILER.CodeAnalysis.Binding
{
    internal sealed class BoundBinaryExpression : BoundExpression
    {
        public BoundBinaryExpression(BoundExpression left, BoundBinaryOperatorKind opKind ,BoundExpression right)
        {
            Left = left;
            OpKind = opKind; 
            Right = right;
        }

        public override Type Type => Left.Type;

        public override BoundNodeKind Kind => BoundNodeKind.BinaryExpression;

        public BoundExpression Left { get; }
        public BoundBinaryOperatorKind OpKind { get; }
        public BoundExpression Right { get; }
    }
}