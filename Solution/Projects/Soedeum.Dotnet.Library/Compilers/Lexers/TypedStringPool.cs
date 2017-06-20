using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    public struct TypedString<T>
    {
        public string Value { get; }

        public T Type { get; }


        public TypedString(string value, T type)
        {
            this.Value = value;

            this.Type = type;
        }
    }

    public class TypedStringPool<T>
    {
        Dictionary<string, TypedString<T>> pool = new Dictionary<string, TypedString<T>>();


        public TypedString<T> Recall(string value, T typeIfNotDefined = default(T))
        {            
            TypedString<T> result;

            if (!pool.TryGetValue(value, out result))
            {
                result = new TypedString<T>(value, typeIfNotDefined);
                
                pool.Add(value, result);
            }

            return result;
        }
    }
}