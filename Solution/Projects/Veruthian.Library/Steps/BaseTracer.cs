using System;

namespace Veruthian.Library.Steps
{
    public abstract class BaseTracer : ITracer
    {
        public virtual bool Trace(IStep step) => Perform(step);


        protected virtual bool Perform(IStep step)
        {
            OnStepStarted(step);

            var result = Handle(step);

            OnStepCompleted(step, result);

            return result;
        }

        protected abstract bool Handle(IStep step);


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


        public event Action<ITracer, IStep> StepStarted;

        public event Action<ITracer, IStep, bool> StepCompleted;
    }
}