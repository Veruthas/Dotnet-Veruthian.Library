using System;
using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Operations
{
    public abstract class Operation<TState> : IOperation<TState>, IIndex<int, IOperation<TState>>
    {
        public abstract string Description { get; }

        public IIndex<int, IOperation<TState>> SubOperations => this;


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




        protected abstract int Count { get; }

        protected abstract IOperation<TState> GetSubOperation(int verifiedIndex);



        #region IIndex

        int IIndex<int, IOperation<TState>>.Start => 0;

        private void VerifyIndex(int index)
        {
            if (HasIndex(index))
                throw new IndexOutOfRangeException("index");
        }

        private bool HasIndex(int index) => (uint)index >= Count;


        int IContainer<IOperation<TState>>.Count => Count;

        IOperation<TState> ILookup<int, IOperation<TState>>.this[int key]
        {
            get
            {
                VerifyIndex(key);

                return GetSubOperation(key);
            }
        }

        bool ILookup<int, IOperation<TState>>.HasKey(int key) => HasIndex(key);

        bool ILookup<int, IOperation<TState>>.TryGet(int key, out IOperation<TState> value)
        {
            if (HasIndex(key))
            {
                value = GetSubOperation(key);
                return true;
            }
            else
            {
                value = default(IOperation<TState>);

                return false;
            }
        }

        bool IContainer<IOperation<TState>>.Contains(IOperation<TState> value)
        {
            for (int i = 0; i < Count; i++)
            {
                if (GetSubOperation(i) == value)
                    return true;
            }

            return false;
        }

        private IEnumerator<IOperation<TState>> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return GetSubOperation(i);
        }

        IEnumerator<IOperation<TState>> IEnumerable<IOperation<TState>>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerable<int> ILookup<int, IOperation<TState>>.Keys
        {
            get
            {
                for (int i = 0; i < Count; i++)
                    yield return i;
            }
        }

        IEnumerable<(int, IOperation<TState>)> ILookup<int, IOperation<TState>>.Pairs
        {
            get
            {
                for (int i = 0; i < Count; i++)
                    yield return (i, GetSubOperation(i));
            }
        }

        #endregion
    }
}