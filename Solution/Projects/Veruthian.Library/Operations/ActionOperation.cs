using System;

namespace Veruthian.Library.Operations
{
    public delegate bool OperationAction<TState>(TState state, ITracer<TState> tracer = null);

    public class ActionOperation<TState> : BaseSimpleOperation<TState>
    {
        OperationAction<TState> action;

        string description;


        public ActionOperation(OperationAction<TState> action, string description = null)
        {
            this.action = action;

            this.description = description;
        }


        public override string Description => description ?? "<action>";

        protected override bool DoAction(TState state, ITracer<TState> tracer = null) => action(state, tracer);
    }
}