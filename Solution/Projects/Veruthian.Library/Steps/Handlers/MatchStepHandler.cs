using System;

namespace Veruthian.Library.Steps.Handlers
{
    public abstract class MatchStepHandler<TState, T> : StepHandler<TState, MatchStep<T>>
    {
        protected override bool? HandleStep(MatchStep<T> step, TState state, IStepHandler<TState> root)
        {
            var item = GetCurrent(state);

            var result = step.Match(item);

            while (result.Success && result.State != null && TryAdvance(state))
            {
                OnMatched(item);

                item = GetCurrent(state);

                result = step.Match(item);
            }

            return result.Success;
        }

        protected abstract T GetCurrent(TState state);

        protected abstract bool TryAdvance(TState state);

        protected virtual void OnMatched(T item)
        {
            if (Matched != null)
                Matched(item);
        }

        public event Action<T> Matched;
    }
}