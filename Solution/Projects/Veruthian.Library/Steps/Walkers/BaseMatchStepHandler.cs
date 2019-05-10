using System;

namespace Veruthian.Library.Steps.Walkers
{
    public abstract class BaseMatchStepHandler<TState, T> : IStepHandler<TState>
    {
        public bool? Handle(IStep step, IStepWalker<TState> walker, TState state)
        {
            switch (step)
            {
                case MatchStep<T> matcher:
                    return HandleMatchStep(matcher, walker, state);
                default:
                    return null;
            }
        }

        protected virtual bool HandleMatchStep(MatchStep<T> matcher, IStepWalker<TState> walker, TState state)
        {
            var item = PeekItem(state);

            var result = matcher.Match(item);

            while (result.Success && result.State != null && MoveNextItem(state))
            {
                item = PeekItem(state);

                result = matcher.Match(item);
            }

            return result.Success;
        }

        protected abstract T PeekItem(TState state);

        protected abstract bool MoveNextItem( TState state);
    }
}