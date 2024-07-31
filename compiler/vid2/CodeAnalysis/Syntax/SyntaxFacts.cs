

namespace MYCOMPILER.CodeAnalysis.Syntax
{
    internal static class SyntaxFacts{
        public static int GetBinaryOperatorPriority(SyntaxeKind kind)
        {
            switch(kind)
            {
                case SyntaxeKind.PlusToken:
                case SyntaxeKind.MinusToken:
                    return 1;
                case SyntaxeKind.TimesToken:
                case SyntaxeKind.DivideToken:
                    return 2;
                default:
                    return 0;
            }
        }

        public static int GetUnaryOperatorPriority(SyntaxeKind kind)
        {
            switch(kind)
            {
                case SyntaxeKind.PlusToken:
                case SyntaxeKind.MinusToken:
                    return 3;
                default:
                    return 0;
            }
        }
    }
    
}