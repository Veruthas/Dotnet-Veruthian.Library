using Veruthian.Library.Processing;
using Veruthian.Library.Types;

namespace Veruthian.Library.Operations.Analyzers
{
    public class SpeculativeOperation<TState> : BaseNestedOperation<TState>
        where TState : Has<ISpeculative>
    {
        public SpeculativeOperation(IOperation<TState> operation) : base(operation) { }


        public override string Description => $"speculate({Operation})";

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