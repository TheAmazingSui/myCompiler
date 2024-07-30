

namespace MYCOMPILER.CodeAnalysis{
    enum SyntaxeKind{

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

        //Expressions
        BinaryExpression,
        ParenthesizedExpression,
        LiteralExpression
    }
} 