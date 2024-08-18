using MYCOMPILER.CodeAnalysis.Syntax;

namespace yap.tests.CodeAnalysis.Syntax
{
    public class SyntaxFactsTest{
        [Theory]
        [MemberData(nameof(GetSyntaxesData))]
        public void syntaxFactsGetTexts(SyntaxeKind kind)
        {
            var text = SyntaxFacts.getText(kind);
            if (text == null)
                return;
            var tokens= SyntaxTree.parseToken(text);
            var token = Assert.Single(tokens);
            Assert.Equal(kind, token.Kind);
            Assert.Equal(text, token.Text);
            
        }


        public static IEnumerable<object[]> GetSyntaxesData()
        {
            var kinds = (SyntaxeKind[]) Enum.GetValues(typeof(SyntaxeKind));
            foreach(var kind in kinds)
            {
                yield return new object[] { kind };
            }
        }
    }

}