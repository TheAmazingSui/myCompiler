

namespace MYCOMPILER.CodeAnalysis
{
    class Lexer
    {
        private readonly string text;

        private List<String> diagnostics = new List<String>();

        public IEnumerable<string> Diagnostics => diagnostics;
        private int position;
        public Lexer(string text)
        {
            this.text = text;
            this.position = 0;

        }

        private char Current{
            get{
                if(position>=text.Length)
                {
                    return '\0';
                }
                return text[position];
            }
        }

        private void Next()
        {
            position++;
        }

        public SyntaxeToken nextToken()
        {

            if (Current == '\0')
                return new SyntaxeToken(SyntaxeKind.EndOfFileToken, position, "\0", null);

            else if(char.IsDigit(Current))
            {
                var start = position;
                while(char.IsDigit(Current))
                {
                    Next();
                }
                var end = position;
                var length = end - start;
                var num = text.Substring(start, length);
                if(!int.TryParse(num, out var value )){
                    diagnostics.Add($"The number {num} cannot be represented by an int32 value");
                }
                return new SyntaxeToken(SyntaxeKind.NumberToken, start, num, value);
            }
            else if(char.IsWhiteSpace(Current))
            {
                var start = position;
                while(char.IsWhiteSpace(Current))
                {
                    Next();
                }
                var end = position;
                var length = end - start;
                var whiteS = text.Substring(start, length);
                int.TryParse(whiteS, out var value );
                return new SyntaxeToken(SyntaxeKind.WhiteSpaceToken, start, whiteS,value);

            }

            else if(Current == '+')
            {
                return new SyntaxeToken(SyntaxeKind.PlusToken, position++, "+", null);
            }
            else if(Current == '-')
            {
                return new SyntaxeToken(SyntaxeKind.MinusToken, position++, "-", null);
            }
            else if(Current == '*')
            {
                return new SyntaxeToken(SyntaxeKind.TimesToken, position++, "*", null);
            }else if(Current == '/')
            {
                return new SyntaxeToken(SyntaxeKind.DivideToken, position++, "/", null);
            }else if(Current == '(')
            {
                return new SyntaxeToken(SyntaxeKind.OpenParenthesisToken, position++, "(", null);
            }else if(Current == ')')
            {
                return new SyntaxeToken(SyntaxeKind.CloseParenthesisToken, position++, ")", null);
            }

            diagnostics.Add($"ERROR: Bad token input '{Current}'");

            return new SyntaxeToken(SyntaxeKind.BadToken, position++, text.Substring(position-1, 1),null);


        }
    }
}