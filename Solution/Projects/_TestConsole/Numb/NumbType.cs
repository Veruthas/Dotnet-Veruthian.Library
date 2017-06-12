namespace _TestConsole.Numb
{
    public enum NumbType
    {
        // Tokens
        EofToken,
        ErrorToken,

        NumberToken,
        VariableToken,

        PlusToken,
        MinusToken,
        TimesToken,
        DivideByToken,

        PowerOfToken,
        
        AssignToken,
        
        SemicolonToken,

        OpenParenthesesToken,
        CloseParenthesesToken,


        // Rules
        NumbRule,
        AssignmentRule,
        EquationRule,
        OperatorRule,
        TermRule,
        EnclosedTerm,
    }
}