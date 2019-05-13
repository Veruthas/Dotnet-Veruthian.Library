using Veruthian.Library.Collections;
using Veruthian.Library.Collections.Extensions;
using Veruthian.Library.Processing;
using Veruthian.Library.Types;

namespace Veruthian.Library.Steps.Handlers
{
    public class SpeculativeConditonalStepHandler<TState> : ConditionalStepHandler<TState>
        where TState : ILookup<string, object>
    {
        public SpeculativeConditonalStepHandler(string speculativeAddress) => this.SpeculativeAddress = speculativeAddress;

        public string SpeculativeAddress { get; }


        protected override void OnSpeculationStarted(IStep speculation, TState state)
        {
            base.OnSpeculationStarted(speculation, state);

            state.Get(SpeculativeAddress, out ISpeculative speculative);

            speculative.Mark();
        }

        protected override void OnSpeculationCompleted(IStep speculation, TState state, bool? result)
        {
            base.OnSpeculationCompleted(speculation, state, result);

            state.Get(SpeculativeAddress, out ISpeculative speculative);

            speculative.Rollback();
        }
    }
}