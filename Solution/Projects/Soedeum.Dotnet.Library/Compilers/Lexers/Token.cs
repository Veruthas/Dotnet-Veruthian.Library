using System;
using System.Collections.Generic;
using Soedeum.Dotnet.Library.Text;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    public class Token<TType> : IToken<TType>
    {
        string source;

        TextSpan span;

        TType type;


        public Token(string source, TextSpan span, TType type)
        {
            this.source = source;

            this.span = span;

            this.type = type;
        }

        public string Source => source;

        public TextSpan Span => span;

        public TType Type => type;


        protected virtual bool TypeEquals(TType type) => this.type.Equals(type);

        public bool IsOf(TType type)
        {
            return TypeEquals(type);
        }

        public void VerifyIsOf(TType type)
        {
            if (!TypeEquals(type))
                throw new TokenTypeMismatchException<TType>(type, this.type);
        }

        public override string ToString()
        {
            return string.Format("Type: '{0}'; Source: '{1}'; {2}", type, source, span.ToString());
        }
        public virtual string ToShortString()
        {
            return string.Format("Type: '{0}'; Value: '{2}'", type, source, span.Text);
        }
    }
}