

namespace MYCOMPILER.CodeAnalysis
{
    public class SyntaxeToken : SyntaxeNode
    {
        public SyntaxeToken(SyntaxeKind kind,int position,string text, object value)
        {
            Kind = kind;
            Position = position;
            Text = text;
            Value = value;
        }
        public override SyntaxeKind Kind { get; }
        public int Position { get; }
        public string Text { get; }

        public object Value { get; }

        public override IEnumerable<SyntaxeNode> GetChildren(){
            return Enumerable.Empty<SyntaxeNode>();
        }

    }
}