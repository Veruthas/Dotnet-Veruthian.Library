using System;
using Veruthian.Library.Collections;
using Veruthian.Library.Readers;
using Veruthian.Library.Types;
using Veruthian.Library.Utility;

namespace Veruthian.Library.Operations.Analyzers
{
    public class MatchAheadSetOperation<TState, TReader, T> : BaseMatchAheadOperation<TState, TReader, T>
        where TState : Has<TReader>
        where TReader : ILookaheadReader<T>
    {
        IContainer<T> set;

        public MatchAheadSetOperation(int lookahead, IContainer<T> set) : base(lookahead)
        {
            ExceptionHelper.VerifyNotNull(set);

            this.set = set;
        }

        public IContainer<T> Set => set;

        public override string Description => $"in(+{lookahead}, {set})";

        protected override bool Match(T value) => set.Contains(value);
    }
}