using System;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public abstract class Operation<TState> : IOperation<TState>
    {
        public abstract string Name { get; }

        public bool Perform(TState state, IOperationTracer<TState> tracer = null)
        {
            if (tracer != null)
                tracer.StartingOperation(this, state);

            bool result = DoAction(state, tracer);

            if (tracer != null)
                tracer.FinishingOperation(this, state, result);

            return result;
        }


        protected abstract bool DoAction(TState state, IOperationTracer<TState> tracer = null);

        public override string ToString() => Name;
    }
}