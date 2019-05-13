using System;

namespace Veruthian.Library.Steps.Handlers
{
    public abstract class StepHandler<TState, TStep> : IStepHandler<TState>
        where TStep : IStep
    {
        public virtual bool? Handle(IStep step, TState state, IStepHandler<TState> root = null)
        {
            switch (step)
            {
                case TStep tstep:
                    {
                        var result = OnStepStarted(tstep, state);

                        if (result == null)
                            result = HandleStep(tstep, state, root);

                        OnStepCompleted(tstep, state, result);

                        return result;
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        protected abstract bool? HandleStep(TStep step, TState state, IStepHandler<TState> root);


        protected virtual bool? OnStepStarted(TStep step, TState state)
        {
            if (StepStarted != null)
                return StepStarted(step, state);

            return null;
        }

        protected virtual void OnStepCompleted(TStep step, TState state, bool? result)
        {
            if (StepCompleted != null)
                StepCompleted(step, state, result);
        }


        public event Func<TStep, TState, bool?> StepStarted;

        public event Action<TStep, TState, bool?> StepCompleted;
    }
}