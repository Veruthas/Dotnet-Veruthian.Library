using System;
using System.Collections.Generic;
using Soedeum.Dotnet.Library.Text;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    public class Token<T> : IToken<T>
        where T : IEquatable<T>
    {
        string source;

        TextSpan span;

        T tokenType;


        public Token(string source, TextSpan span, T tokenType)
        {
            this.source = source;

            this.span = span;

            this.tokenType = tokenType;
        }

        public string Source => source;

        public TextSpan Span => span;

        public T TokenType => tokenType;


        public bool IsOf(T tokenType)
        {
            return this.tokenType.Equals(tokenType);
        }

        public void VerifyIsOf(T tokenType)
        {
            if (!IsOf(tokenType))
                throw new TokenTypeMismatchException<T>(tokenType, this.tokenType);
        }
    }
}