using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Data.Patterns
{
    public class Pattern<T>
    {
        // New()
        // Match(T | Pattern)        
        // MatchIf(T | Pattern)      
        // MatchWhile(T | Pattern)
        // MatchAtLeast(int times, T | Pattern)
        // MatchAtMost(int times, T |  Pattern)
        // MatchBetween(int min, int max, T | Pattern)
        // MatchOneOf(Pattern...) --> Full backtracking
        // TryOneOf(int until, Pattern...) --> Commit after x items matched
        // MatchAhead(T | Pattern)
        // NonMatchAhead(T | Pattern)

        public static Pattern<T> New() => new Pattern<T>();

        // Match
        public Pattern<T> Match(T item) => this;

        public Pattern<T> Match(Pattern<T> pattern) => this;


        // MatchIf
        public Pattern<T> MatchIf(T item) => this;

        public Pattern<T> MatchIf(Pattern<T> pattern) => this;


        // MatchWhile
        public Pattern<T> MatchWhile(T item) => this;

        public Pattern<T> MatchWhile(Pattern<T> pattern) => this;


        // MatchAtLeast
        public Pattern<T> MatchAtLeast(int times, T item) => this;

        public Pattern<T> MatchAtLeast(int times) => this;


        // MatchAtMost
        public Pattern<T> MatchAtMost(int times, T item) => this;

        public Pattern<T> MatchAtMost(int times, Pattern<T> pattern) => this;


        // MatchBetween
        public Pattern<T> MatchBetween(int min, int max, T item) => this;

        public Pattern<T> MatchBetween(int min, int max, Pattern<T> pattern) => this;


        // MatchAhead
        public Pattern<T> MatchAhead(T item) => this;

        public Pattern<T> MatchAhead(Pattern<T> pattern) => this;

        // NonMatchAhead
        public Pattern<T> NonMatchAhead(T item) => this;

        public Pattern<T> NonMatchAhead(Pattern<T> pattern) => this;


        // MatchOneOf
        public Pattern<T> MatchOneOf(params Pattern<T>[] patterns) => this;
    }
}