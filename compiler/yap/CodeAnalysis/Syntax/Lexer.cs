

namespace MYCOMPILER.CodeAnalysis.Syntax
{
    class Lexer
    {
        private readonly string text;

        private DiagnosticBag diagnostics = new DiagnosticBag();

        public DiagnosticBag Diagnostics => diagnostics;
        private int position;
        public Lexer(string text)
        {
            this.text = text;
            this.position = 0;

        }

        private char Current => Peek(0);

        private char Lookahead => Peek(1);
        private char Peek(int offset)
        {
            var index = position + offset;
            if(index>=text.Length)
            {
                return '\0';
            }
            return text[index];
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
                    diagnostics.ReportInvalidNumber(new TextSpan(start,length), text, typeof(int));
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
            //True or False
            else if(char.IsLetter(Current))
            {
                var start = position;
                while(char.IsLetter(Current))
                {
                    Next();
                }
                var end = position;
                var length = end - start;
                var boolS = text.Substring(start, length);
                var kind = SyntaxFacts.GetKeywordKind(boolS);
                return new SyntaxeToken(kind, start, boolS,null);

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
            else if(Current=='!')
            {
                var prev_pos = position;
                if (Lookahead == '=')
                {
                    position += 2;
                    return new SyntaxeToken(SyntaxeKind.NotEqualToken, prev_pos, "!=", null);
                }
                    
                return new SyntaxeToken(SyntaxeKind.BangToken, position++, "!", null);
            }
            else if(Current=='&')
            {
                var prev_pos = position;
                if(Lookahead == '&')
                {
                    position += 2;
                    return new SyntaxeToken(SyntaxeKind.LogicalAndToken, prev_pos, "&&", null);
                }
                    
            }
            else if(Current=='|')
            {
                var prev_pos = position;
                if(Lookahead == '|')
                {
                    position += 2;
                    return new SyntaxeToken(SyntaxeKind.LogicalOrToken, prev_pos, "||", null);
                }
                    
            }
            else if(Current == '=')
            {
                var prev_pos = position;
                if (Lookahead == '=')
                {
                    position += 2;
                    return new SyntaxeToken(SyntaxeKind.DoubleEqualToken, prev_pos, "==", null);
                }
                //assignment
                return new SyntaxeToken(SyntaxeKind.EqualToken, position++, "=", null);
                    
            }

            diagnostics.ReportBadCharacter(position, Current);

            return new SyntaxeToken(SyntaxeKind.BadToken, position++, text.Substring(position-1, 1),null);


        }
    }
}