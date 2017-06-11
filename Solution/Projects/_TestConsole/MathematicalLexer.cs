using Soedeum.Dotnet.Library.Compilers.Lexers;

namespace _TestConsole
{
    /* Language:
        rule Mathematical := Assignment*;
        rule Assignment := Variable Assignment Equation;
        rule Equation := Term (Operator Term)*;
        rule Operator := Plus | Minus | Times | DivideBy | Exponent;
        rule Term := Variable | Number | EnclosedTerm;
        rule EnclosedTerm := (Plus | Minus)? OpenParentheses Equation CloseParentheses;


        token Number :=  Integer (  ( '.' (Numeric)+ ) | (('E' | 'e') Integer)  ) ?;
        subtoken Integer := ('+' | '-')? (Numeric)+;
        
        token Variable := (Alpha | '_')(AlphaNumeric | '_')+;
        
        token Plus := '+';
        token Minus := '-';
        token Times := '*';
        token DivideBy := '/';
        token Exponent := '^';        
        token Assign := ':=';
        token OpenParentheses := '(';
        token CloseParentheses := ')';

        charset Numeric = '0'..'9';
        charset Alpha = 'A'..'Z' | 'a'..'z';
        charset AlphaNumeric = Alpha | Numeric;

    */
    public enum MathematicalTokenType
    {
        Eof,
        Error,

        Number,
        Variable,

        Plus,
        Minus,
        Times,
        DivideBy,
        
        Assign,
        

        OpenParentheses,
        CloseParentheses
    }
    
    public class MathematicalToken
    {


    }
    public class MathematicalLexer
    {

    }
}