using System;
using System.Collections.Generic;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps.Matching
{
    public class MatchStepGenerator : StepGeneratorAdapter
    {
        public MatchStepGenerator(IStepGenerator generator) : base(generator) { }


        public MatchStep<T> MatchItem<T>(T value, Func<T, string> toString = null) 
            where T : IEquatable<T> 
            => new MatchItemStep<T>(value, toString);

        public MatchStep<T> MatchSet<T>(IContainer<T> set, Func<IContainer<T>, string> toString = null) 
            => new MatchSetStep<T>(set, toString);

        public MatchStep<T> MatchSequence<T>(IEnumerable<T> sequence, Func<IEnumerable<T>, string> toString = null) 
            where T : IEquatable<T> 
            => new MatchSequenceStep<T>(sequence, toString);
    }
}