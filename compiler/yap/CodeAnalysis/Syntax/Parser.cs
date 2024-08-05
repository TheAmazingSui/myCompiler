


namespace MYCOMPILER.CodeAnalysis.Syntax
{
    class Parser
    {

        private readonly SyntaxeToken[] tokensArray;
        private int position;

        private DiagnosticBag diagnostic = new DiagnosticBag();

        public DiagnosticBag Diagnostic => diagnostic;
        public Parser(string text)
        {
            Lexer lexer = new Lexer(text);
            SyntaxeToken token = lexer.nextToken();
            List<SyntaxeToken> token_list = new List<SyntaxeToken>();
            position = 0;
            while (token.Kind != SyntaxeKind.EndOfFileToken)
            {
                if (token.Kind != SyntaxeKind.WhiteSpaceToken)
                {
                    token_list.Add(token);
                }
                token = lexer.nextToken();

            }
            tokensArray = token_list.ToArray();
            diagnostic.AddRange(lexer.Diagnostics);

        }

        private SyntaxeToken Peek(int offset)
        {
            if (position + offset >= tokensArray.Length)
            {
                return new SyntaxeToken(SyntaxeKind.EndOfFileToken, position, "\0", null);
            }
            return tokensArray[position + offset];
        }

        private SyntaxeToken Current => Peek(0);

        private SyntaxeToken NextToken()
        {
            SyntaxeToken curr = Current;
            position++;
            return curr;
        }

        public SyntaxeToken match(SyntaxeKind kind)
        {
            if (Current.Kind == kind)
            {
                var curr = NextToken();
                return curr;
            }

            diagnostic.ReportUnexpectedToken(Current.Span, Current.Kind, kind);
            return new SyntaxeToken(kind, Current.Position, null, null);
        }

        public SyntaxTree parse()
        {
            var exp = parseExpression();
            var endof = match(SyntaxeKind.EndOfFileToken);
            return new SyntaxTree(diagnostic, exp, endof);
        }

        private ExpressionSyntaxe parseExpression(int parentPrecedence = 0)
        {
            var unary = SyntaxFacts.GetUnaryOperatorPriority(Current.Kind);
            ExpressionSyntaxe left;
            if(unary !=0 && unary >= parentPrecedence)
            {
                var operatorToken = NextToken();
                var operand =  parseExpression(unary);
                left = new UnaryExpressionSyntaxe(operand, operatorToken);
            }
            else{
                left = ParsePrimaryExpression();
            }

            while (true)
            {
                var opPriority = SyntaxFacts.GetBinaryOperatorPriority(Current.Kind);
                if (opPriority == 0 || opPriority <= parentPrecedence)
                    break;
                var op = NextToken();
                var right = parseExpression(opPriority);
                left = new BinaryExpressionSyntaxe(left, op, right);
            }
            return left;
        }



        private ExpressionSyntaxe ParsePrimaryExpression()
        {

            if (Current.Kind == SyntaxeKind.OpenParenthesisToken)
            {
                var left = NextToken();
                var exp = parseExpression();
                var right = match(SyntaxeKind.CloseParenthesisToken);
                return new ParenthesizedExpressionSyntax(left, exp, right);
            }
            else if(Current.Kind == SyntaxeKind.TrueKeyword || Current.Kind == SyntaxeKind.FalseKeyword)    
            {
                var keyWord = NextToken();
                var value = keyWord.Kind == SyntaxeKind.TrueKeyword;
                return new LiteralExpressionSyntaxe(keyWord, value);
            }
            SyntaxeToken numExp = match(SyntaxeKind.NumberToken);
            return new LiteralExpressionSyntaxe(numExp);
        }
    }
}