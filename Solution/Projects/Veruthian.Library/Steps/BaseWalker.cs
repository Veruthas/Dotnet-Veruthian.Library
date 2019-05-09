using System;

namespace Veruthian.Library.Steps
{
    public abstract class BaseWalker : IWalker
    {
        public virtual bool Walk(IStep step)
        {
            if (step == null)
                step = HandleNullStep(step);

            OnStepStarted(step);

            var result = Handle(step);

            OnStepCompleted(step, result);

            return result;
        }


        protected abstract bool Handle(IStep step);

        protected abstract IStep HandleNullStep(IStep step);


        protected virtual void OnStepStarted(IStep step)
        {
            if (StepStarted != null)
                StepStarted(this, step);
        }

        protected virtual void OnStepCompleted(IStep step, bool result)
        {
            if (StepCompleted != null)
                StepCompleted(this, step, result);
        }


        public event Action<IWalker, IStep> StepStarted;

        public event Action<IWalker, IStep, bool> StepCompleted;
    }
}