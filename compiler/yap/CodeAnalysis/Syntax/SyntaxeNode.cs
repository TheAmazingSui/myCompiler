
namespace MYCOMPILER.CodeAnalysis.Syntax
{
    public abstract class SyntaxeNode{
        public abstract SyntaxeKind Kind{get;}

        public abstract  IEnumerable<SyntaxeNode> GetChildren();
    }    
}

