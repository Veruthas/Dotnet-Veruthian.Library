using Veruthian.Library.Processing;
using Veruthian.Library.Types;

namespace Veruthian.Library.Steps.Handlers
{
    public class SpeculativeConditonalStepHandler<TState> : ConditionalStepHandler<TState>
        where TState : Has<ISpeculative>
    {
        protected override void OnSpeculationStarted(IStep speculation, TState state)
        {
            base.OnSpeculationStarted(speculation, state);

            state.Get(out var speculative);

            speculative.Mark();
        }

        protected override void OnSpeculationCompleted(IStep speculation, TState state, bool? result)
        {
            base.OnSpeculationCompleted(speculation, state, result);

            state.Get(out var speculative);

            if (result == true)
                speculative.Commit();
            else
                speculative.Rollback();
        }
    }
}