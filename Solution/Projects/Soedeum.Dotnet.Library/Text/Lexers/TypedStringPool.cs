using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Text.Lexers
{
    public struct TypedCodeString<T>
    {
        public CodeString Value { get; }

        public T Type { get; }


        public TypedCodeString(CodeString value, T type)
        {
            this.Value = value;

            this.Type = type;
        }
    }

    public class TypedCodeStringPool<T>
    {
        Dictionary<CodeString, TypedCodeString<T>> pool = new Dictionary<CodeString, TypedCodeString<T>>();


        public TypedCodeString<T> Recall(CodeString value, T typeIfNotDefined = default(T))
        {            
            TypedCodeString<T> result;

            if (!pool.TryGetValue(value, out result))
            {
                result = new TypedCodeString<T>(value, typeIfNotDefined);
                
                pool.Add(value, result);
            }

            return result;
        }
    }
}