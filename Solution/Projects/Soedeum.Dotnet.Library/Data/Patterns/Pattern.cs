using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Data.Patterns
{
    public class Pattern<T>
    {
        // Match(T | Pattern)
        // Call(TIdentifier)
        // Optional()
        // Repeat()
        // Repeat(int times)
        // Repeat(int min, int max)        
        // FollowedBy(T | Pattern)
        // NotFollowedBy(T | Pattern)
        // Choice(params Pattern[] pattern)      
        public Pattern<T> Match(T value)
        {
            return this;
        }

        public Pattern<T> Match(IEnumerable<T> value)
        {
            return this;
        }


        public Pattern<T> Call(string name)
        {
            return this;
        }

        public Pattern<T> FollowedBy(Pattern<T> pattern)
        {
            return this;
        }

        public Pattern<T> NotFollowedBy(Pattern<T> pattern)
        {
            return this;
        }

        public static Pattern<T> New() => new Pattern<T>();
    }
}