using MYCOMPILER.CodeAnalysis.Syntax;

namespace MYCOMPILER.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryOperator{

        private BoundUnaryOperator(SyntaxeKind kind, BoundUnaryOperatorKind boundKind, Type opType) : this(kind, boundKind, opType, opType)
        {

        }
        private BoundUnaryOperator(SyntaxeKind kind, BoundUnaryOperatorKind boundKind, Type opType, Type resType)
        {
            Kind = kind;
            BoundKind = boundKind;
            OpType = opType;
            ResType = resType;
        }

        public SyntaxeKind Kind { get; }
        public BoundUnaryOperatorKind BoundKind { get; }
        public Type OpType { get; }
        public Type ResType { get; }

        private static BoundUnaryOperator[] operators =
        {
            new BoundUnaryOperator(SyntaxeKind.BangToken, BoundUnaryOperatorKind.LogicalNegation, typeof(bool)),
            new BoundUnaryOperator(SyntaxeKind.MinusToken, BoundUnaryOperatorKind.Negation, typeof(int)),
            new BoundUnaryOperator(SyntaxeKind.PlusToken, BoundUnaryOperatorKind.Identity, typeof(int))
        };

        public static BoundUnaryOperator bind(SyntaxeKind kind, Type operandType)
        {
            foreach(var op in operators)
            {
                if (op.Kind == kind && op.OpType == operandType) return op; 
            }
            return null;
        }
    }
}