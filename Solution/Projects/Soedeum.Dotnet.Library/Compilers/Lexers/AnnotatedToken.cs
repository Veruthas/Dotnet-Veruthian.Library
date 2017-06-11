using System;
using System.Collections.Generic;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Text;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    public class AnnotatedToken<TTokenType, TKey, TValue> : Token<TTokenType>, ITable<TKey, TValue>
    {
        public AnnotatedToken(string source, TextSpan span, TTokenType tokenType)
            : base(source, span, tokenType)
        {
        }

        Table<TKey, TValue> annotations;

        public bool Define(TKey key, TValue value, bool overwrite = false)
        {
            if (annotations == null)
                annotations = new Table<TKey, TValue>();

            return annotations.Define(key, value, overwrite);
        }


        public int Count => annotations == null ? 0 : annotations.Count;

        public bool IsEmpty => Count == 0;

        public IEnumerable<TKey> Keys => annotations == null ? EmptyEnumerable<TKey>.Default : annotations.Keys;

        public IEnumerable<TValue> Values => annotations == null ? EmptyEnumerable<TValue>.Default : annotations.Values;

        public IEnumerable<KeyValuePair<TKey, TValue>> Items => (annotations == null) ? EmptyEnumerable<KeyValuePair<TKey, TValue>>.Default : annotations.Items;

        public void Clear() => annotations?.Clear();


        public bool UnDefine(TKey key) => (annotations == null) ? false : UnDefine(key);

        public bool IsDefined(TKey key) => annotations == null ? false : annotations.IsDefined(key);

        public TValue Resolve(TKey key)
        {
            if (annotations == null)
                throw new KeyNotFoundException();
            else
                return annotations.Resolve(key);
        }

        public bool TryResolve(TKey key, out TValue result)
        {
            if (annotations == null)
            {
                result = default(TValue);
                return false;
            }
            else
                return annotations.TryResolve(key, out result);
        }

        public bool TryResolve<TCast>(TKey key, out TCast result) where TCast : TValue
        {

            if (annotations == null)
            {
                result = default(TCast);
                return false;
            }
            else
                return annotations.TryResolve(key, out result);
        }
    }
}