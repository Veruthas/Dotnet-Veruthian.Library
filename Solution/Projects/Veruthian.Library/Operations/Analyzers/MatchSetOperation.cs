using System;
using Veruthian.Library.Numeric;
using Veruthian.Library.Readers;
using Veruthian.Library.Types;
using Veruthian.Library.Utility;

namespace Veruthian.Library.Operations.Analyzers
{
    public class MatchSetOperation<TState, TReader, T> : BaseMatchOperation<TState, TReader, T>
        where TState : Has<TReader>
        where TReader : IReader<T>
        where T : ISequential<T>, IBounded<T>
    {
        RangeSet<T> set;

        public MatchSetOperation(RangeSet<T> set)
        {
            ExceptionHelper.VerifyNotNull(set);

            this.set = set;
        }

        public RangeSet<T> Set => set;

        public override string Description => $"in({set})";

        protected override bool Match(T value) => set.Contains(value);
    }
}