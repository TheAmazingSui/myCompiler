namespace MYCOMPILER.CodeAnalysis.Binding
{
    internal class BoundVariableExpression : BoundExpression
    {

        public BoundVariableExpression(VariableSymbol variableSymbol)
        {
            VariableSymbol = variableSymbol;
        }

        public override Type Type => VariableSymbol.Type;

        public override BoundNodeKind Kind => BoundNodeKind.VariableExpression;
        public VariableSymbol VariableSymbol { get; }
    }
}