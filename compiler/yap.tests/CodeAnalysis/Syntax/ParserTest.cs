using MYCOMPILER.CodeAnalysis.Syntax;

namespace yap.tests.CodeAnalysis.Syntax
{
    public class ParserTest
    {

        [Theory]
        [MemberData(nameof(GetBinaryOperatorPairsData))]
        public void parseBinaryExpPrec(SyntaxeKind op1, SyntaxeKind op2)
        {
            var op1Prec = SyntaxFacts.GetBinaryOperatorPriority(op1);
            var op2Prec = SyntaxFacts.GetBinaryOperatorPriority(op2);
            var op1Text = SyntaxFacts.getText(op1);
            var op2Text = SyntaxFacts.getText(op2);
            var text = $"a {op1Text} b {op2Text} c";
            var exp = SyntaxTree.parse(text).Root;

            if(op1Prec >= op2Prec)
            {
                //      op2
                //     /    \
                //    op1    c
                //  /    \  
                // a      b 
                using(var e= new AssertingEnumerator(exp))
                {
                    e.AssertNode(SyntaxeKind.BinaryExpression);
                    e.AssertNode(SyntaxeKind.BinaryExpression);
                    e.AssertNode(SyntaxeKind.NameExpression);
                    e.AssertToken(SyntaxeKind.IdentifierKeyword, "a");
                    e.AssertToken(op1, op1Text);
                    e.AssertNode(SyntaxeKind.NameExpression);
                    e.AssertToken(SyntaxeKind.IdentifierKeyword, "b");
                    e.AssertToken(op2, op2Text);
                    e.AssertNode(SyntaxeKind.NameExpression);
                    e.AssertToken(SyntaxeKind.IdentifierKeyword, "c");



                }
            }
            else{

                //     op1
                //    /   \
                //   a    op2
                //       /   \
                //      b     c

                using(var e = new AssertingEnumerator(exp))
                {
                    e.AssertNode(SyntaxeKind.BinaryExpression);
                    e.AssertNode(SyntaxeKind.NameExpression);
                    e.AssertToken(SyntaxeKind.IdentifierKeyword, "a");
                    e.AssertToken(op1, op1Text);
                    e.AssertNode(SyntaxeKind.BinaryExpression);
                    e.AssertNode(SyntaxeKind.NameExpression);
                    e.AssertToken(SyntaxeKind.IdentifierKeyword, "b");
                    e.AssertToken(op2, op2Text);
                    e.AssertNode(SyntaxeKind.NameExpression);
                    e.AssertToken(SyntaxeKind.IdentifierKeyword, "c");

                }                
            }


        }

        public static IEnumerable<object[]> GetBinaryOperatorPairsData()
        {
            foreach(var op1 in SyntaxFacts.GetBinaryOperatorKinds())
            {
                foreach(var op2 in SyntaxFacts.GetBinaryOperatorKinds())
                {
                    yield return new object[] { op1, op2 }; 
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetUnaryOperatorPairsData))]
        public void parseUnaryExpPrec(SyntaxeKind unaryKind, SyntaxeKind binaryKind)
        {
            var unaryPrec = SyntaxFacts.GetUnaryOperatorPriority(unaryKind);
            var binaryPrec = SyntaxFacts.GetBinaryOperatorPriority(binaryKind);
            var unaryText = SyntaxFacts.getText(unaryKind);
            var binaryText = SyntaxFacts.getText(binaryKind);
            var text = $"{unaryText} a {binaryText} b";
            var exp = SyntaxTree.parse(text).Root;

            if(unaryPrec >= binaryPrec)
            {
                //      binary
                //     /    \
                //    unary  b
                //     |  
                //     a      
                using(var e= new AssertingEnumerator(exp))
                {
                    e.AssertNode(SyntaxeKind.BinaryExpression);
                    e.AssertNode(SyntaxeKind.UnaryExpression);
                    e.AssertToken(unaryKind, unaryText);
                    e.AssertNode(SyntaxeKind.NameExpression);
                    e.AssertToken(SyntaxeKind.IdentifierKeyword, "a");
                    e.AssertToken(binaryKind, binaryText);
                    e.AssertNode(SyntaxeKind.NameExpression);
                    e.AssertToken(SyntaxeKind.IdentifierKeyword, "b");


                }
            }
            else{

                //      unary
                //       |
                //      binary
                //     /   \
                //     a    c

                using(var e = new AssertingEnumerator(exp))
                {


                    e.AssertNode(SyntaxeKind.UnaryExpression);
                    e.AssertToken(unaryKind, unaryText);
                    e.AssertNode(SyntaxeKind.BinaryExpression);
                    e.AssertNode(SyntaxeKind.NameExpression);
                    e.AssertToken(SyntaxeKind.IdentifierKeyword, "a");
                    e.AssertToken(binaryKind, binaryText);
                    e.AssertNode(SyntaxeKind.NameExpression);
                    e.AssertToken(SyntaxeKind.IdentifierKeyword, "b");

                }                
            }


        }
        public static IEnumerable<object[]> GetUnaryOperatorPairsData()
        {
            foreach(var unary in SyntaxFacts.GetUnaryOperatorKind())
            {
                foreach(var binary in SyntaxFacts.GetBinaryOperatorKinds())
                {
                    yield return new object[] { unary, binary }; 
                }
            }
        }
    }

    

}