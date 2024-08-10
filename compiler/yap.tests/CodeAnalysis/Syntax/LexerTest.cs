using MYCOMPILER.CodeAnalysis.Syntax;

namespace yap.tests.CodeAnalysis.Syntax
{
    public class LexerTest
    {
        [Theory]
        [MemberData(nameof(getTokensData))]
        public void lexToken(SyntaxeKind kind, string text)
        {
            var tokens = SyntaxTree.parseToken(text);
            var token = Assert.Single(tokens);

            Assert.Equal(kind, token.Kind);
            Assert.Equal(text, token.Text);
        }

        [Theory]
        [MemberData(nameof(getTokensPairData))]
        public void lexTokenPairs(SyntaxeKind t1kind, string t1text, SyntaxeKind t2kind, string t2text)
        {
            var text = t1text + t2text;
            var tokens = SyntaxTree.parseToken(text).ToArray();
            
            Assert.Equal(2,tokens.Length);

            Assert.Equal(tokens[0].Kind, t1kind);
            Assert.Equal(tokens[1].Kind, t2kind);

            Assert.Equal(tokens[0].Text, t1text);
            Assert.Equal(tokens[1].Text, t2text);
        }

        [Theory]
        [MemberData(nameof(getTokensPairSepData))]
        public void lexTokenPairsSep(SyntaxeKind t1kind, string t1text, 
                                    SyntaxeKind sepKind, string sepText,
                                    SyntaxeKind t2kind, string t2text)
        {
            var text = t1text + sepText+ t2text;
            var tokens = SyntaxTree.parseToken(text).ToArray();
            
            Assert.Equal(3,tokens.Length);


            Assert.Equal(tokens[0].Kind, t1kind);
            Assert.Equal(tokens[1].Kind, sepKind);
            Assert.Equal(tokens[2].Kind, t2kind);
            

            Assert.Equal(tokens[0].Text, t1text);
            Assert.Equal(tokens[1].Text, sepText);
            Assert.Equal(tokens[2].Text, t2text);
        }

        public static IEnumerable<object[]> getTokensData()
        {
            foreach(var e in getTokens().Concat(getSeperator()))
            {
                yield return new object[] { e.kind, e.text };
            }
        }

        private static bool requiresSeperator(SyntaxeKind t1kind,SyntaxeKind t2kind)
        {
            var t1IsKeyword = t1kind.ToString().EndsWith("Keyword");
            var t2IsKeyword = t2kind.ToString().EndsWith("Keyword");
            if (t1kind == SyntaxeKind.IdentifierKeyword && t2kind == SyntaxeKind.IdentifierKeyword)
                return true;
            else if (t1IsKeyword && t2IsKeyword)
                return true;
            else if (t1IsKeyword && t2kind == SyntaxeKind.IdentifierKeyword)
                return true;
            else if (t1kind == SyntaxeKind.IdentifierKeyword && t2IsKeyword)
                return true;
            else if (t1kind == SyntaxeKind.NumberToken && t2kind == SyntaxeKind.NumberToken)
                return true;
            else if (t1kind == SyntaxeKind.BangToken && t2kind == SyntaxeKind.EqualToken)
                return true;
            else if (t1kind == SyntaxeKind.BangToken && t2kind == SyntaxeKind.DoubleEqualToken)
                return true;
            else if (t1kind == SyntaxeKind.EqualToken && t2kind == SyntaxeKind.EqualToken)
                return true;
            else if (t1kind == SyntaxeKind.EqualToken && t2kind == SyntaxeKind.DoubleEqualToken)
                return true;
            //More cases
            return false;
        }

        public static IEnumerable<object[]> getTokensPairData()
        {
            foreach(var e in getTokensPairs())
            {
                yield return new object[] { e.t1kind, e.t1text, e.t2kind,e.t2text };
            }
        }

        public static IEnumerable<object[]> getTokensPairSepData()
        {
            foreach(var e in getTokensPairsSep())
            {
                yield return new object[] { e.t1kind, e.t1text, e.sepKind, e.sepText, e.t2kind,e.t2text };
            }
        }

        private static IEnumerable<(SyntaxeKind kind, string text)>getTokens()
        {
            return new[]{
                (SyntaxeKind.IdentifierKeyword,"a"),
                (SyntaxeKind.IdentifierKeyword,"abc"),
                (SyntaxeKind.NumberToken,"3"),
                (SyntaxeKind.NumberToken,"123"),
                (SyntaxeKind.NumberToken,"87542"),
                //(SyntaxeKind.WhiteSpaceToken,"          "),
                //(SyntaxeKind.WhiteSpaceToken," "),
                //(SyntaxeKind.WhiteSpaceToken,"   "),
                //(SyntaxeKind.WhiteSpaceToken,"\n"),
                //(SyntaxeKind.WhiteSpaceToken,"\r"),
                (SyntaxeKind.PlusToken,"+"),
                (SyntaxeKind.MinusToken,"-"),
                (SyntaxeKind.TimesToken,"*"),
                (SyntaxeKind.DivideToken,"/"),
                (SyntaxeKind.OpenParenthesisToken,"("),
                (SyntaxeKind.CloseParenthesisToken,")"),
                (SyntaxeKind.EqualToken,"="), 
                (SyntaxeKind.TrueKeyword,"true"),
                (SyntaxeKind.FalseKeyword,"false"),
                (SyntaxeKind.BangToken,"!"),
                (SyntaxeKind.LogicalAndToken,"&&"),
                (SyntaxeKind.LogicalOrToken,"||"),
                (SyntaxeKind.DoubleEqualToken,"=="),
                (SyntaxeKind.NotEqualToken,"!="),
  
            };
        }

        private static IEnumerable<(SyntaxeKind kind, string text)>getSeperator()
        {
            return new[]{
                (SyntaxeKind.WhiteSpaceToken,"          "),
                (SyntaxeKind.WhiteSpaceToken," "),
                (SyntaxeKind.WhiteSpaceToken,"   "),
                (SyntaxeKind.WhiteSpaceToken,"\n"),
                (SyntaxeKind.WhiteSpaceToken,"\r"),
            };
        }

        private static IEnumerable<(SyntaxeKind t1kind, string t1text, SyntaxeKind t2kind, string t2text )>getTokensPairs()
        {
            foreach(var t1 in getTokens())
            {
                foreach (var t2 in getTokens())
                {
                    if(!requiresSeperator(t1.kind,t2.kind))
                        yield return (t1.kind, t1.text, t2.kind, t2.text);
                }
            }
        }

         private static IEnumerable<(SyntaxeKind t1kind, string t1text,
                                        SyntaxeKind sepKind, string sepText,
                                         SyntaxeKind t2kind, string t2text )>getTokensPairsSep()
        {
            foreach(var t1 in getTokens())
            {
                foreach (var t2 in getTokens())
                {
                    if (requiresSeperator(t1.kind, t2.kind))
                    {

                        foreach(var s in getSeperator())
                            yield return (t1.kind, t1.text, s.kind, s.text, t2.kind, t2.text);
                    }
                }
            }
        }

        
    }

}