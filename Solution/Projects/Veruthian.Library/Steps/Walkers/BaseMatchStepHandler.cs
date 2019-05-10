using System;

namespace Veruthian.Library.Steps.Walkers
{
    public abstract class BaseMatchStepHandler<T> : IStepHandler
    {
        public bool? Handle(IStepWalker walker, IStep step)
        {
            switch (step)
            {
                case MatchStep<T> matcher:
                    return HandleMatchStep(walker, matcher);
                default:
                    return null;
            }
        }

        protected virtual bool HandleMatchStep(IStepWalker walker, MatchStep<T> matcher)
        {
            var item = PeekItem(walker);

            var result = matcher.Match(item);

            while (result.Success && result.State != null && MoveNextItem(walker))
            {
                item = PeekItem(walker);

                result = matcher.Match(item);
            }

            return result.Success;
        }

        protected abstract T PeekItem(IStepWalker walker);

        protected abstract bool MoveNextItem(IStepWalker walker);
    }
}