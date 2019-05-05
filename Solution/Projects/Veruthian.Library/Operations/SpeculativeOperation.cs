using Veruthian.Library.Processing;
using Veruthian.Library.Types;

namespace Veruthian.Library.Operations
{
    public class SpeculativeOperation<TState> : BaseNestedOperation<TState>
        where TState : Has<ISpeculative>
    {
        public SpeculativeOperation(IOperation<TState> operation) : base(operation) { }


        public override string Description => $"speculate";

        protected override bool DoAction(TState state, ITracer<TState> tracer = null)
        {
            state.Get(out ISpeculative speculative);

            speculative.Mark();

            bool result = Operation.Perform(state, tracer);

            speculative.Rollback();

            return result;
        }
    }
}