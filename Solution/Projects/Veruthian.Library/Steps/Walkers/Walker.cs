using System;
using System.Collections.Generic;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps.Walkers
{
    public class Walker : IStepWalker
    {
        IEnumerable<IStepHandler> handlers;

        Func<IStep> nullStepHandler;


        public Walker(params IStepHandler[] handlers)
            : this(null, (IEnumerable<IStepHandler>)handlers) { }

        public Walker(Func<IStep> nullStepHandler, params IStepHandler[] handlers)
            : this(null, (IEnumerable<IStepHandler>)handlers) { }


        public Walker(Func<IStep> nullStepHandler, IEnumerable<IStepHandler> handlers)
        {
            this.nullStepHandler = nullStepHandler ?? (() => throw new NullReferenceException("Step cannot be null"));

            this.handlers = handlers ?? new IStepHandler[0];
        }


        public virtual bool Walk(IStep step)
        {
            if (step == null)
                step = nullStepHandler();

            OnStepStarted(step);


            bool? result = null;

            foreach (var handler in handlers)
            {
                result = handler.Handle(this, step);

                if (result != null)
                    break;
            }

            if (result == null)
                result = false;


            OnStepCompleted(step, result.Value);

            return result.Value;
        }


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


        public event Action<IStepWalker, IStep> StepStarted;

        public event Action<IStepWalker, IStep, bool> StepCompleted;
    }
}