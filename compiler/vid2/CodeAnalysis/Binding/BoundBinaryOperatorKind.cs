namespace MYCOMPILER.CodeAnalysis.Binding
{
    internal enum BoundBinaryOperatorKind
    {
        Addition,
        Subtraction,
        Multiplication,
        Division,

        LogicalAnd,
        LogicalOr,
        Equality,
        Inequality
    }
}