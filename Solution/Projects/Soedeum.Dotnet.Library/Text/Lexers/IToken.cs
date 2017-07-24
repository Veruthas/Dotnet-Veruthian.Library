using System;
using System.Collections.Generic;
using Soedeum.Dotnet.Library.Text.Code;

namespace Soedeum.Dotnet.Library.Text.Lexers
{
    public interface IToken<TType>
    {
        string Source { get; }

        TextLocation Location { get; }

        CodeString Value { get; }

        TType Type { get; }

        bool IsOf(TType type);

        void VerifyIsOf(TType type);
    }
}