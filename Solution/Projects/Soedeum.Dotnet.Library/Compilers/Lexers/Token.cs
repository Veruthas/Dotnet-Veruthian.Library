using System;
using System.Collections.Generic;
using Soedeum.Dotnet.Library.Text;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    public class Token<TTokenType> : IToken<TTokenType>
    {
        string source;

        TextSpan span;

        TTokenType tokenType;


        public Token(string source, TextSpan span, TTokenType tokenType)
        {
            this.source = source;

            this.span = span;

            this.tokenType = tokenType;
        }

        public string Source => source;

        public TextSpan Span => span;

        public TTokenType TokenType => tokenType;


        protected virtual bool TokenTypeEquals(TTokenType tokenType) => this.tokenType.Equals(tokenType);
        
        public bool IsOf(TTokenType tokenType)
        {
            return TokenTypeEquals(tokenType);
        }

        public void VerifyIsOf(TTokenType tokenType)
        {
            if (!TokenTypeEquals(tokenType))
                throw new TokenTypeMismatchException<TTokenType>(tokenType, this.tokenType);
        }
    }
}