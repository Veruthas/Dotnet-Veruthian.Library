using System;
using System.Collections.Generic;

namespace Veruthian.Library.Steps.Handlers
{
    public class RootStepHandler<TState> : StepHandlers<TState>
    {
        IEnumerable<IStepHandler<TState>> handlers;

        Func<IStep> nullStepHandler;


        public RootStepHandler(params IStepHandler<TState>[] handlers)
            : this(null, (IEnumerable<IStepHandler<TState>>)handlers) { }

        public RootStepHandler(Func<IStep> nullStepHandler, params IStepHandler<TState>[] handlers)
            : this(null, (IEnumerable<IStepHandler<TState>>)handlers) { }

        public RootStepHandler(Func<IStep> nullStepHandler, IEnumerable<IStepHandler<TState>> handlers)
        {
            this.nullStepHandler = nullStepHandler ?? (() => throw new NullReferenceException("Step cannot be null"));

            this.handlers = handlers ?? new IStepHandler<TState>[0];
        }


        public override bool? Handle(IStep step, TState state, IStepHandler<TState> root = null)
        {
            if (step == null)
                step = nullStepHandler();

            if (root == null)
                root = this;

            return base.Handle(step, state, root);
        }

        protected override bool? HandleStep(IStep step, TState state, IStepHandler<TState> root) => null;
    }
}