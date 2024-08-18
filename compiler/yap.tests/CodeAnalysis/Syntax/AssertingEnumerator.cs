using MYCOMPILER.CodeAnalysis.Syntax;

namespace yap.tests.CodeAnalysis.Syntax
{
    internal sealed class AssertingEnumerator : IDisposable{

        private readonly IEnumerator<SyntaxeNode> _enumerator;
        private bool hasErrors;
        public AssertingEnumerator(SyntaxeNode node)
        {
            _enumerator = Flatten(node).GetEnumerator();
        }

        private bool markFailed()
        {
            hasErrors = true;
            return false;
        }

        public void Dispose()
        {
            if(!hasErrors)
                Assert.False(_enumerator.MoveNext());

            _enumerator.Dispose();
        }

        private static IEnumerable<SyntaxeNode> Flatten(SyntaxeNode node)
        {
            var stack = new Stack<SyntaxeNode>();
            stack.Push(node);

            while(stack.Count > 0)
            {
                var a = stack.Pop();
                yield return a;
                foreach(var child in a.GetChildren().Reverse())
                {
                    stack.Push(child);
                }
            }
        }

        public void AssertToken(SyntaxeKind kind, string text)
        {
            try
            {
                Assert.True(_enumerator.MoveNext());
                Assert.Equal(kind, _enumerator.Current.Kind);
                var token = Assert.IsType<SyntaxeToken>(_enumerator.Current);
                Assert.Equal(text, token.Text);    
            }
            catch when (markFailed())
            {
                throw;
            }
            
        }

        public void AssertNode(SyntaxeKind kind)
        {
            try
            {
                Assert.True(_enumerator.MoveNext());
                Assert.Equal(kind, _enumerator.Current.Kind);   
                Assert.IsNotType<SyntaxeToken>(_enumerator.Current);
            }
            catch when (markFailed())
            {
                throw;
            }
            
        }

        
    }

}