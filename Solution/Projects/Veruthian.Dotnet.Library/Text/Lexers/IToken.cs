using System;
using System.Collections.Generic;
using Veruthian.Dotnet.Library.Text.Code;

namespace Veruthian.Dotnet.Library.Text.Lexers
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