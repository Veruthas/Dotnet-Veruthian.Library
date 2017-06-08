using System;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    public interface ITokenType<T> : IEquatable<T>
        where T : ITokenType<T>
    {
        string Name { get; }

        string DefaultValue { get; }
    }
}