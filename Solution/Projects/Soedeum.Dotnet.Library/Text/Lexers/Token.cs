using System;
using System.Collections.Generic;
using Soedeum.Dotnet.Library.Text;

namespace Soedeum.Dotnet.Library.Text.Lexers
{
    public class Token<TType> : IToken<TType>
    {
        string source;

        TextLocation location;

        CodeString value;

        TType type;


        public Token(string source, TextLocation location, CodeString value, TType type)
        {
            this.source = source;

            this.location = location;

            this.value = value;

            this.type = type;
        }

        public string Source => source;

        public TextLocation Location => location;

        public CodeString Value => value;

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
            return string.Format("Type: '{0}'; Source: '{1}'; Location: {2}; Value:", type, source, location.ToString(), value);
        }
        public virtual string ToShortString()
        {
            return string.Format("Type: '{0}'; Value: '{2}'", type, value);
        }
    }
}