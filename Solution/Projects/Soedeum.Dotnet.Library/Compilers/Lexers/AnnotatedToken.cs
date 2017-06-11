using System;
using Soedeum.Dotnet.Library.Collections;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    public class AnnotatedToken<TTokenType, TKey, TValue> : Token<TTokenType>, IAnnotated<TKey, TValue>
        where TTokenType : IEquatable<TTokenType>
    {
        public bool Define(TKey key, TValue value, bool canOverwrite = true)
        {
            throw new NotImplementedException();
        }

        public bool IsDefined(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool TryResolve(TKey key, out TValue result, TValue defaultValue = default(TValue))
        {
            throw new NotImplementedException();
        }
    }
}