namespace MYCOMPILER.CodeAnalysis.Binding
{
    internal class BoundAssignmentExpression : BoundExpression
    {

        public BoundAssignmentExpression(string name, BoundExpression boundExp)
        {
            Name = name;
            BoundExp = boundExp;
        }

        public string Name { get; }
        public BoundExpression BoundExp { get; }

        public override Type Type => BoundExp.Type;

        public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;
    }
}