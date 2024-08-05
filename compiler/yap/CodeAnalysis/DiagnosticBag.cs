using System.Collections;
using MYCOMPILER.CodeAnalysis.Syntax;

namespace MYCOMPILER.CodeAnalysis
{
    internal sealed class DiagnosticBag : IEnumerable<Diagnostic>
    {

        private readonly List<Diagnostic> diagnostics = new List<Diagnostic>();

        public IEnumerator<Diagnostic> GetEnumerator() => diagnostics.GetEnumerator();

        public void ReportInvalidNumber(TextSpan textSpan, string text, Type type)
        {
            var message = $"The number {text} is not a valid {type}!";
            Report(textSpan, message);
        }

        public void AddRange(DiagnosticBag ddiagnostics)
        {
            diagnostics.AddRange(ddiagnostics.diagnostics);
        }

        public void ReportBadCharacter(int position, char character)
        {
            var message = $"ERROR: Bad token input '{character}'";
            TextSpan span = new TextSpan(position, 1);
            Report(span, message);

        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        private void Report(TextSpan span, string message)
        {
            var diagnostic = new Diagnostic(span, message);
            diagnostics.Add(diagnostic);
        }

        public void ReportUnexpectedToken(TextSpan span, SyntaxeKind currentkind, SyntaxeKind kind)
        {
            var message = $"ERROR: Unexpected token '{currentkind}', expected '{kind}'";
            Report(span, message);
        }

        public void ReportUndefinedBinaryOperator(TextSpan span, string text, Type leftType, Type rightType)
        {
            var message = $"Binary operator '{text}' is not defined for type {leftType} and {rightType}";
            Report(span, message);
        }

        public void ReportUndefinedUnaryOperator(TextSpan span, string text, Type type)
        {
            var message = $"Unary operator '{text}' is not defined for type {type}";
            Report(span, message);
        }
    }
}