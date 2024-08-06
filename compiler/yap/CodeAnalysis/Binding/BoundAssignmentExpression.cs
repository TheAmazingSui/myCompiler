namespace MYCOMPILER.CodeAnalysis.Binding
{
    internal class BoundAssignmentExpression : BoundExpression
    {

        public BoundAssignmentExpression(VariableSymbol variableSymbol, BoundExpression boundExp)
        {
            VariableSymbol = variableSymbol;
            BoundExp = boundExp;
        }
        public VariableSymbol VariableSymbol { get; }
        public BoundExpression BoundExp { get; }

        public override Type Type => BoundExp.Type;

        public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;
    }
}