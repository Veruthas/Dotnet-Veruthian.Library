using System;

namespace Soedeum.Dotnet.Library.Text.Lexers
{
    public class TokenTypeMismatchException<TType> : System.Exception
    {
        public TokenTypeMismatchException(TType expected, TType found)
            : base(GetMessage(expected, found)) { }


        public TokenTypeMismatchException(TType expected, TType found, Exception inner)
            : base(GetMessage(expected, found), inner)
        {
            Expected = expected;
            Found = found;
        }

        TType Expected { get; }

        TType Found { get; }


        private static string GetMessage(TType expected, TType found)
        {
            return string.Format("{0} Expected '{1}', Found '{2}'", DefaultMessage, expected, found);
        }
        private static readonly string DefaultMessage = "Token types did not match.";
    }
}