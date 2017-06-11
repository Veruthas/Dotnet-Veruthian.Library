using System;
using System.Collections.Generic;
using Soedeum.Dotnet.Library.Text;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    public interface IToken<TTokenType>
        where TTokenType : IEquatable<TTokenType>
    {
        string Source { get; }

        TextSpan Span { get; }
        
        TTokenType TokenType { get; }
        
        bool IsOf(TTokenType type);

        void VerifyIsOf(TTokenType type);
    }
}