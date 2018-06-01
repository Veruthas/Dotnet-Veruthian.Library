using System;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public abstract class SimpleOperation<TState> : IOperation<TState>
    {
        public bool Perform(TState state) => DoAction(state);
        

        public bool Perform(TState state, IOperationTracer<TState> tracer)
        {
            tracer.StartingOperation(this, state);

            bool result = DoAction(state);

            tracer.FinishingOperation(this, state, result);

            return result;
        }

        protected abstract bool DoAction(TState state);
    }
}