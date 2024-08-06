


namespace MYCOMPILER.CodeAnalysis.Syntax
{
    internal static class SyntaxFacts{
        public static int GetBinaryOperatorPriority(SyntaxeKind kind)
        {
            switch(kind)
            {
                case SyntaxeKind.LogicalOrToken:
                    return 1;
                case SyntaxeKind.LogicalAndToken:
                    return 2;
                case SyntaxeKind.DoubleEqualToken:
                case SyntaxeKind.NotEqualToken:
                    return 3;
                case SyntaxeKind.PlusToken:
                case SyntaxeKind.MinusToken:
                    return 4;
                case SyntaxeKind.TimesToken:
                case SyntaxeKind.DivideToken:
                    return 5;
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
                case SyntaxeKind.BangToken:
                    return 6;
                default:
                    return 0;
            }
        }

        internal static SyntaxeKind GetKeywordKind(string boolS)
        {
            switch(boolS)
            {
                case "true":
                    return SyntaxeKind.TrueKeyword;
                case "false":
                    return SyntaxeKind.FalseKeyword;
                default:
                    return SyntaxeKind.IdentifierKeyword;

            }
        }
    }
    
}