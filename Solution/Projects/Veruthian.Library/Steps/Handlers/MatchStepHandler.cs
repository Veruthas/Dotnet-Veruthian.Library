using System;

namespace Veruthian.Library.Steps.Handlers
{
    public abstract class MatchStepHandler<T> : StepHandler<MatchStep<T>>
    {
        protected override bool? HandleStep(MatchStep<T> step, StateTable state, IStepHandler root)
        {
            if (IsEnd(state))
                return false;

            var item = GetCurrent(state);

            var result = step.Match(item);

            while (result.Success)
            {
                OnMatched(item);

                Advance(state);                

                if (result.State != null && !IsEnd(state))
                {
                    item = GetCurrent(state);

                    result = step.Match(item);
                }
                else
                {
                    break;
                }
            }

            return result.Success;
        }

        protected abstract bool IsEnd(StateTable state);

        protected abstract T GetCurrent(StateTable state);

        protected abstract void Advance(StateTable state);

        protected virtual void OnMatched(T item)
        {
            if (Matched != null)
                Matched(item);
        }

        public event Action<T> Matched;
    }
}