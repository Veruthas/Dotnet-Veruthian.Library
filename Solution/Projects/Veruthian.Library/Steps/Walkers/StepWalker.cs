using System;
using System.Collections.Generic;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps.Walkers
{
    public class StepWalker<TState> : IStepWalker<TState>
    {
        IEnumerable<IStepHandler<TState>> handlers;

        Func<IStep> nullStepHandler;


        public StepWalker(params IStepHandler<TState>[] handlers)
            : this(null, (IEnumerable<IStepHandler<TState>>)handlers) { }

        public StepWalker(Func<IStep> nullStepHandler, params IStepHandler<TState>[] handlers)
            : this(null, (IEnumerable<IStepHandler<TState>>)handlers) { }


        public StepWalker(Func<IStep> nullStepHandler, IEnumerable<IStepHandler<TState>> handlers)
        {
            this.nullStepHandler = nullStepHandler ?? (() => throw new NullReferenceException("Step cannot be null"));

            this.handlers = handlers ?? new IStepHandler<TState>[0];
        }


        public virtual bool Walk(IStep step, TState state = default(TState))
        {
            if (step == null)
                step = nullStepHandler();

            OnStepStarted(step, state);


            bool? result = null;

            foreach (var handler in handlers)
            {
                result = handler.Handle(step, this, state);

                if (result != null)
                    break;
            }

            if (result == null)
                result = false;


            OnStepCompleted(step, state, result.Value);

            return result.Value;
        }


        protected virtual void OnStepStarted(IStep step, TState state)
        {
            if (StepStarted != null)
                StepStarted(step, this, state);
        }

        protected virtual void OnStepCompleted(IStep step, TState state, bool result)
        {
            if (StepCompleted != null)
                StepCompleted(step, this, state, result);
        }


        public event Action<IStep, IStepWalker<TState>, TState> StepStarted;

        public event Action<IStep, IStepWalker<TState>, TState, bool> StepCompleted;
    }
}