using Veruthian.Library.Numeric;
using Veruthian.Library.Readers;
using Veruthian.Library.Types;

namespace Veruthian.Library.Operations
{
    public class MatchSetOperation<T, TState> : BaseSimpleOperation<TState>
        where TState : Has<IReader<T>>
        where T : ISequential<T>, IBounded<T>
    {
        RangeSet<T> set;

        public MatchSetOperation(RangeSet<T> set) => this.set = set;

        public RangeSet<T> Set => set;

        public override string Description => $"MatchSet({set.ToString()}";

        protected override bool DoAction(TState state, ITracer<TState> tracer = null)
        {
            state.Get(out IReader<T> reader);

            if (!reader.IsEnd)
            {
                var value = reader.Peek();

                if (this.set.Contains(value))
                {
                    reader.Read();

                    return true;
                }
            }

            return false;
        }
    }
}