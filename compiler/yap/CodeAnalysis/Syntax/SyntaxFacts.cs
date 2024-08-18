


namespace MYCOMPILER.CodeAnalysis.Syntax
{
    public static class SyntaxFacts{
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


        public static IEnumerable<SyntaxeKind> GetUnaryOperatorKind()
        {
            var kinds = (SyntaxeKind[]) Enum.GetValues(typeof(SyntaxeKind));
            foreach(var kind in kinds)
            {
                if(GetUnaryOperatorPriority(kind)>0)
                {
                    yield return kind;
                }
            }
        }

        public static IEnumerable<SyntaxeKind> GetBinaryOperatorKinds()
        {
            var kinds = (SyntaxeKind[]) Enum.GetValues(typeof(SyntaxeKind));
            foreach(var kind in kinds)
            {
                if(GetBinaryOperatorPriority(kind)>0)
                {
                    yield return kind;
                }
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

        public static string getText(SyntaxeKind Kind)
        {
            switch(Kind)
            {
                case SyntaxeKind.PlusToken:
                    return "+";
                case SyntaxeKind.MinusToken:
                    return "-";
                case SyntaxeKind.TimesToken:
                    return "*";
                case SyntaxeKind.DivideToken:
                    return "/";
                case SyntaxeKind.BangToken:
                    return "!";
                case SyntaxeKind.EqualToken:
                    return "=";
                case SyntaxeKind.LogicalAndToken:
                    return "&&";
                case SyntaxeKind.LogicalOrToken:
                    return "||";
                case SyntaxeKind.DoubleEqualToken:
                    return "==";
                case SyntaxeKind.NotEqualToken:
                    return "!=";
                case SyntaxeKind.OpenParenthesisToken:
                    return "(";
                case SyntaxeKind.CloseParenthesisToken:
                    return ")";
                case SyntaxeKind.FalseKeyword:
                    return "false";
                case SyntaxeKind.TrueKeyword:
                    return "true";
                default:
                    return null;
            }
        }
    }
    
}