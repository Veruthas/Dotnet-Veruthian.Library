using System;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    public class TokenTypeMismatchException<TTokenType> : System.Exception
    {
        public TokenTypeMismatchException(TTokenType expected, TTokenType found)
            : base(GetMessage(expected, found)) { }


        public TokenTypeMismatchException(TTokenType expected, TTokenType found, Exception inner)
            : base(GetMessage(expected, found), inner)
        {
            Expected = expected;
            Found = found;
        }

        TTokenType Expected { get; }

        TTokenType Found { get; }


        private static string GetMessage(TTokenType expected, TTokenType found)
        {
            return string.Format("{0} Expected '{1}', Found '{2}'", DefaultMessage, expected, found);
        }
        private static readonly string DefaultMessage = "Token types did not match.";
    }
}