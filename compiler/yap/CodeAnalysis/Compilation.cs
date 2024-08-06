using MYCOMPILER.CodeAnalysis.Syntax;

using MYCOMPILER.CodeAnalysis.Binding;

namespace MYCOMPILER.CodeAnalysis
{
    public sealed class Compilation{
        public Compilation(SyntaxTree tree)
        {
            Tree = tree; 
        }

        public SyntaxTree Tree { get; }

        public EvaluationResult Evaluate(Dictionary<VariableSymbol,object> variables)
        {
            var binder = new Binder(variables);
            var boundExp = binder.bindExpression(Tree.Root);

            var diagnostics = Tree.Diagnostics.Concat(binder.Diagnostics).ToArray();
            if (diagnostics.Any())
                return new EvaluationResult(diagnostics, null);
            var evaluator = new Evaluator(boundExp, variables);
            var value = evaluator.Evaluate();
            return new EvaluationResult(Array.Empty<Diagnostic>(), value);


        }
    }
}