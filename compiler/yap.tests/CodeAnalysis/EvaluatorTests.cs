using MYCOMPILER.CodeAnalysis;
using MYCOMPILER.CodeAnalysis.Syntax;

namespace yap.tests.CodeAnalysis
{
    public class EvaluationTests{
        [Theory]
        [InlineData("1",1)]
        [InlineData("-1",-1)]
        [InlineData("+1",1)]
        [InlineData("1 + 2",3)]
        [InlineData("4 * 2",8)]
        [InlineData("5 - 13",-8)]
        [InlineData("2 / 1",2)]
        [InlineData("(10 + 6)*2",32)]
        [InlineData("12 == 3",false)]
        [InlineData("10 == 10",true)]
        [InlineData("10 != 10",false)]
        [InlineData("10 != 100",true)]
        [InlineData("true",true)]
        [InlineData("true && false",false)]
        [InlineData("!false",true)]
        [InlineData("false || true",true)]
        [InlineData("(a=10)*a",100)]
        public void syntaxFactsGetTexts(string text, object expectedValue)
        {
            var tree = SyntaxTree.parse(text);
            var compilation = new Compilation(tree);
            var variables = new Dictionary<VariableSymbol, object>();
            var actualRes = compilation.Evaluate(variables);
            Assert.Empty(actualRes.Diagnostics);
            Assert.Equal(expectedValue, actualRes.Value);
            
        }
    }
}