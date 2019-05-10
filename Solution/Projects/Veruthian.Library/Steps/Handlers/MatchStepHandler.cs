using System;

namespace Veruthian.Library.Steps.Handlers
{
    public abstract class MatchStepHandler<TState, T> : TypedStepHandler<TState, MatchStep<T>>
    {
        protected override bool? HandleStep(MatchStep<T> step, TState state, IStepHandler<TState> root)
        {
            var item = PeekItem(state);

            var result = step.Match(item);

            while (result.Success && result.State != null && MoveNextItem(state))
            {
                item = PeekItem(state);

                result = step.Match(item);
            }

            return result.Success;
        }

        protected abstract T PeekItem(TState state);

        protected abstract bool MoveNextItem( TState state);
    }
}