using System;
using System.Collections.Generic;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps.Matching
{
    public class MatchStepGenerator : StepGeneratorAdapter
    {
        public MatchStepGenerator(IStepGenerator generator) : base(generator) { }


        public MatchStep<T> MatchItem<T>(T value) 
            where T : IEquatable<T> 
            => new MatchItemStep<T>(value);

        public MatchStep<T> MatchSet<T>(IContainer<T> set) 
            => new MatchSetStep<T>(set);

        public MatchStep<T> MatchSequence<T, S>(S sequence) 
            where T : IEquatable<T> 
            where S : IEnumerable<T> 
            => new MatchSequenceStep<T, S>(sequence);
    }
}