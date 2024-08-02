using MYCOMPILER.CodeAnalysis.Syntax;

namespace MYCOMPILER.CodeAnalysis.Binding
{
    internal sealed class BoundBinaryOperator{
        private BoundBinaryOperator(SyntaxeKind kind, BoundBinaryOperatorKind boundOP, Type leftType,Type rightType ,Type resType)
        {
            Kind = kind;
            BoundOP = boundOP;
            LeftType = leftType;
            RightType = rightType;
            ResType = resType;
        }

        public SyntaxeKind Kind { get; }
        public BoundBinaryOperatorKind BoundOP { get; }
        public Type LeftType { get; }
        public Type RightType { get; }
        public Type ResType { get; }

        private static BoundBinaryOperator[] operators =
        {
            new BoundBinaryOperator(SyntaxeKind.PlusToken, BoundBinaryOperatorKind.Addition, typeof(int), typeof(int), typeof(int)),
            new BoundBinaryOperator(SyntaxeKind.MinusToken, BoundBinaryOperatorKind.Subtraction, typeof(int),typeof(int),typeof(int)),
            new BoundBinaryOperator(SyntaxeKind.TimesToken, BoundBinaryOperatorKind.Multiplication, typeof(int), typeof(int),typeof(int)),
            new BoundBinaryOperator(SyntaxeKind.DivideToken, BoundBinaryOperatorKind.Division, typeof(int), typeof(int), typeof(int)),
            new BoundBinaryOperator(SyntaxeKind.LogicalAndToken, BoundBinaryOperatorKind.LogicalAnd, typeof(bool),typeof(bool),typeof(bool)),
            new BoundBinaryOperator(SyntaxeKind.LogicalOrToken, BoundBinaryOperatorKind.LogicalOr, typeof(bool),typeof(bool),typeof(bool))
        };

        public static BoundBinaryOperator bind(SyntaxeKind kind, Type leftType, Type rightType)
        {
            foreach(var op in operators)
            {
                if (op.Kind == kind && op.LeftType == leftType && op.RightType == rightType) return op; 
            }
            return null;
        }
    }
}