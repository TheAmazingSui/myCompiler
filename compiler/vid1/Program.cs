// See https://aka.ms/new-console-template for more information
using System;
using System.Data;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace prog{
    class Program{
        static void Main(string[] args)
        {
            bool showTree = false;
            while(true)
            {
                Console.Write(">");
                var line = Console.ReadLine();
                if(string.IsNullOrWhiteSpace(line))
                {
                    return;
                }

                if (line == "#showTree")
                {
                    showTree = !showTree;
                    Console.WriteLine(showTree ? "Showing parse trees:" : "Not showing parse Trees");
                    continue;
                }

                //Lexer lexer = new Lexer(line);
                SyntaxTree exp = SyntaxTree.parse(line);
                var color = Console.ForegroundColor;
                if(showTree)
                {
                    
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    PrettyPrint(exp.Root);
                    Console.ForegroundColor = color;
                }
                

                if(exp.Diagnostics.Any())
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    foreach(var e in exp.Diagnostics)
                    {
                        Console.WriteLine(e);
                    }
                    Console.ForegroundColor = color;
                }
                else{
                    Evaluator eval = new Evaluator(exp.Root);
                    var result = eval.Evaluate();
                    Console.WriteLine(result);

                }
            }
        }
        static void PrettyPrint(SyntaxeNode node, string indent = "")
        {
            Console.Write(indent);
            Console.Write(node.Kind);

            if(node is SyntaxeToken t && t.Value != null)
            {
                Console.Write(" ");
                Console.Write(t.Value);
            }

            Console.WriteLine();
            indent += "    ";

            foreach(var child in node.GetChildren())
            {
                PrettyPrint(child, indent);
            }
            return;

        }
    }
    enum SyntaxeKind{
        NumberToken,
        WhiteSpaceToken,
        PlusToken,
        MinusToken,
        TimesToken,
        DivideToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        BadToken,
        EndOfFileToken,
        NumberExpression,
        BinaryExpression,
        ParenthesizedExpression
    }

    class SyntaxeToken : SyntaxeNode
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

    abstract class SyntaxeNode{
        public abstract SyntaxeKind Kind{get;}

        public abstract  IEnumerable<SyntaxeNode> GetChildren();
    }

    abstract class ExpressionSyntaxe : SyntaxeNode{

    }

    sealed class NumberExpressionSyntaxe: ExpressionSyntaxe{
        public NumberExpressionSyntaxe(SyntaxeToken numberToken){
            NumberToken = numberToken;
        }

        public SyntaxeToken NumberToken { get; }
        public override SyntaxeKind Kind => SyntaxeKind.NumberExpression;

        public override IEnumerable<SyntaxeNode> GetChildren()
        {
            yield return NumberToken;
        }
    }

    sealed class BinaryExpressionSyntaxe : ExpressionSyntaxe{
        public BinaryExpressionSyntaxe(ExpressionSyntaxe left, SyntaxeToken operatorToken, ExpressionSyntaxe right){
            Left = left;
            Right = right;
            OperatorToken = operatorToken;
        }

        public ExpressionSyntaxe Left { get; }
        public ExpressionSyntaxe Right { get; }
        public SyntaxeToken OperatorToken { get; }

        public override SyntaxeKind Kind => SyntaxeKind.BinaryExpression;

        public override IEnumerable<SyntaxeNode> GetChildren()
        {
            yield return Left;
            yield return OperatorToken;
            yield return Right;
        }
    }
    
    
    sealed class SyntaxTree{
        public SyntaxTree(IEnumerable<string> diagnostics,ExpressionSyntaxe root, SyntaxeToken endOfFile)
        {
            Root = root;
            EndOfFile = endOfFile;
            Diagnostics = diagnostics.ToArray();

        }

        public static SyntaxTree parse(string text)
        {
            var parser = new Parser(text);
            return parser.parse();
        }

        public ExpressionSyntaxe Root { get;  }
        public SyntaxeToken EndOfFile { get; }
        public IReadOnlyList<string> Diagnostics { get; }
    }
    
    sealed class ParenthesizedExpressionSyntax: ExpressionSyntaxe{

        public ParenthesizedExpressionSyntax(SyntaxeToken openParenthesis, ExpressionSyntaxe numExp, SyntaxeToken closedParenthesis)
        {
            OpenParenthesis = openParenthesis;
            NumExp = numExp;
            ClosedParenthesis = closedParenthesis;
        }

        public SyntaxeToken OpenParenthesis { get; }
        public ExpressionSyntaxe NumExp { get; }
        public SyntaxeToken ClosedParenthesis { get; }

        public override SyntaxeKind Kind => SyntaxeKind.ParenthesizedExpression;

        public override IEnumerable<SyntaxeNode> GetChildren()
        {
            yield return OpenParenthesis;
            yield return NumExp;
            yield return ClosedParenthesis;

        }
    }
    
    class Parser{

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
            while(token.Kind != SyntaxeKind.EndOfFileToken)
            {
                if(token.Kind!=SyntaxeKind.WhiteSpaceToken)
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
            if(position + offset >= tokensArray.Length){
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
            if(Current.Kind == kind)
            {
                var curr = NextToken();
                return curr;
            }

            diagnostic.Add($"ERROR: Unexpected token '{Current.Kind}', expected '{kind}'");
            return new SyntaxeToken(kind, Current.Position, null, null);
        }

        public SyntaxTree parse()
        {
            var exp = parseTerm();
            var endof = match(SyntaxeKind.EndOfFileToken);
            return new SyntaxTree(diagnostic, exp, endof);
        }

        public ExpressionSyntaxe parseTerm()
        {
            var left = parseFactor();
            while (Current.Kind == SyntaxeKind.PlusToken || Current.Kind == SyntaxeKind.MinusToken)
            {
                SyntaxeToken NodeOp = NextToken();
                var right = parseFactor();
                left = new BinaryExpressionSyntaxe(left, NodeOp, right);
            }
            return left;
        }

        public ExpressionSyntaxe parseFactor()
        {
            var left = ParsePrimaryExpression();
            while (Current.Kind == SyntaxeKind.TimesToken || Current.Kind == SyntaxeKind.DivideToken)
            {
                SyntaxeToken NodeOp = NextToken();
                var right = ParsePrimaryExpression();
                left = new BinaryExpressionSyntaxe(left, NodeOp, right);
            }
            return left;
        }

        private ExpressionSyntaxe ParsePrimaryExpression()
        {

            if(Current.Kind == SyntaxeKind.OpenParenthesisToken)
            {
                var left = NextToken();
                var exp = parseTerm();
                var right = match(SyntaxeKind.CloseParenthesisToken);
                return new ParenthesizedExpressionSyntax(left, exp, right);
            }
            SyntaxeToken numExp = match(SyntaxeKind.NumberToken);
            return new NumberExpressionSyntaxe(numExp);
        }
    }

    class Evaluator{
        public Evaluator(ExpressionSyntaxe root)
        {
            Root = root;
        }

        public ExpressionSyntaxe Root { get; }

        public int Evaluate()
        {
            return EvaluateExpression(Root);
        }

        public int EvaluateExpression(ExpressionSyntaxe root)
        {

            if(root is NumberExpressionSyntaxe n)
            {
                return (int)n.NumberToken.Value;
            }
            else if(root is BinaryExpressionSyntaxe b)
            {
                var lft = EvaluateExpression(b.Left);
                var rgt = EvaluateExpression(b.Right);

                if(b.OperatorToken.Kind == SyntaxeKind.PlusToken)
                {
                    return lft + rgt;
                }
                else if(b.OperatorToken.Kind == SyntaxeKind.MinusToken)
                {
                    return lft - rgt;
                }
                else if(b.OperatorToken.Kind == SyntaxeKind.TimesToken)
                {
                    return lft * rgt;
                }
                else if(b.OperatorToken.Kind == SyntaxeKind.DivideToken)
                {
                    return lft / rgt;
                }
                else{
                    throw new Exception($"Unexpected operator: '{b.OperatorToken.Kind}!");
                }
            }
            else if(root is ParenthesizedExpressionSyntax p)
            {
                return EvaluateExpression(p.NumExp);
            }
            throw new Exception($"Unexpected node: {root.Kind}");

        }
    }

}
