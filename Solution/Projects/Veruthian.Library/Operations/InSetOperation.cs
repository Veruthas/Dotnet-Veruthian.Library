using System;
using Veruthian.Library.Numeric;
using Veruthian.Library.Readers;
using Veruthian.Library.Types;
using Veruthian.Library.Utility;

namespace Veruthian.Library.Operations
{
    public class InSetOperation<TState, T> : BaseReadOperation<TState, T>
        where TState : Has<IReader<T>>
        where T : ISequential<T>, IBounded<T>
    {
        RangeSet<T> set;

        public InSetOperation(RangeSet<T> set)
        {
            ExceptionHelper.VerifyNotNull(set);

            this.set = set;
        }

        public override string Description => $"in({set})";

        protected override bool Match(T value) => set.Contains(value);
    }
}