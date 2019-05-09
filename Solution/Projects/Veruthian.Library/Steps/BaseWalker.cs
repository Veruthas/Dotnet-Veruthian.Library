using System;

namespace Veruthian.Library.Steps
{
    public abstract class BaseWalker : IWalker
    {
        public virtual bool Walk(IStep step)
        {
            OnStepStarted(step);

            var result = Perform(step);

            OnStepCompleted(step, result);

            return result;
        }

        protected abstract bool Perform(IStep step);


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