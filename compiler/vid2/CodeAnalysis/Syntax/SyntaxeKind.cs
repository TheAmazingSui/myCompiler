

namespace MYCOMPILER.CodeAnalysis.Syntax{
    public enum SyntaxeKind{

        //Tokens
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
        IdentifierKeyword,

        //KeyWord
        TrueKeyword,
        FalseKeyword,
        BangToken,
        LogicalAndToken,
        LogicalOrToken,

        //Expressions
        BinaryExpression,
        ParenthesizedExpression,
        LiteralExpression,
        UnaryExpression
    }
} 