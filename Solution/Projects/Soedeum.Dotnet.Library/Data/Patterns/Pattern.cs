using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Data.Patterns
{
    public class Pattern<TOn, TId>
    {
        // New
        // Match(T | T... | Pattern)        
        // MatchIf(T | T... | Pattern)        
        // MatchAtLeast(int times, T | T... | Pattern)
        // MatchAtMost(int times, T | T... | Pattern)
        // MatchBetween(int min, int max, T | T... | Pattern)
        // MatchWhile(T | T... | Pattern)
        // FollowedBy(T | T... | Pattern)
        // NotFollowedBy(T | T... | Pattern)
        // OneOf(Pattern...)
        // Complete()
        // Call(Pattern)

        public static Pattern<TOn, TId> New() => new Pattern<TOn, TId>();
    }
}