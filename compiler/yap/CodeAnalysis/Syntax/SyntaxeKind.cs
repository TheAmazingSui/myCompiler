

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
        EqualToken,

        //KeyWord
        TrueKeyword,
        FalseKeyword,

        //Bools
        BangToken,
        LogicalAndToken,
        LogicalOrToken,

        DoubleEqualToken,
        NotEqualToken,

        //Expressions
        BinaryExpression,
        ParenthesizedExpression,
        LiteralExpression,
        UnaryExpression,
        NameExpression,
        AssignmentExpression
    }
} 