using System;
using System.Collections.Generic;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps.Actions.Extensions
{
    public static class MatchExtensions
    {
        public static IStep MatchValue<T>(this StepGenerator generator, T value) where T : IEquatable<T> => new MatchValueStep<T>(value);

        public static IStep MatchSequence<T>(this StepGenerator generator, IEnumerable<T> sequence) where T : IEquatable<T> => new MatchSequenceStep<T>(sequence);

        public static IStep MatchSet<T>(this StepGenerator generator, IContainer<T> set) => new MatchSetStep<T>(set);
    }
}