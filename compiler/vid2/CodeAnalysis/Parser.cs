


namespace MYCOMPILER.CodeAnalysis
{
    class Parser
    {

        private readonly SyntaxeToken[] tokensArray;
        private int position;

        private List<string> diagnostic = new List<string>();

        public IEnumerable<string> Diagnostic => diagnostic;
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

            diagnostic.Add($"ERROR: Unexpected token '{Current.Kind}', expected '{kind}'");
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
            SyntaxeToken numExp = match(SyntaxeKind.NumberToken);
            return new LiteralExpressionSyntaxe(numExp);
        }
    }
}