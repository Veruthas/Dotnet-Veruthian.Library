using System;
using System.Collections;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Operations
{
    public abstract class Operation<TState> : IOperation<TState>
    {
        public abstract string Description { get; }

        public bool Perform(TState state, IOperationTracer<TState> tracer = null)
        {
            if (tracer != null)
                tracer.OperationStarting(this, state);

            bool result = DoAction(state, tracer);

            if (tracer != null)
                tracer.OperationFinishing(this, state, result);

            return result;
        }


        protected abstract bool DoAction(TState state, IOperationTracer<TState> tracer = null);

        public override string ToString() => Description;



        public abstract IOperation<TState> GetSubOperation(int index);

        protected void VerifyIndex(int index)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException("index");
        }

        public abstract IEnumerator<IOperation<TState>> GetEnumerator();

        public abstract int Count { get; }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}