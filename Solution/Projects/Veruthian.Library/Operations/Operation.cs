using System;
using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Operations
{

    public abstract class Operation<TState> : IOperation<TState>, IIndex<IOperation<TState>>
    {
        #region Abstracts

        protected abstract bool DoAction(TState state, ITracer<TState> tracer = null);

        public abstract string Description { get; }

        protected abstract int Count { get; }

        protected abstract IOperation<TState> GetSubOperation(int verifiedIndex);

        #endregion

        #region Implemented
        public bool Perform(TState state, ITracer<TState> tracer = null)
        {
            if (tracer != null)
                tracer.OnStarting(this, state);

            bool result = DoAction(state, tracer);

            if (tracer != null)
                tracer.OnFinishing(this, state, result);

            return result;
        }

        public IIndex<IOperation<TState>> SubOperations => this;

        public override string ToString() => Description;

        #endregion

        #region SubOperations

        int IIndex<int, IOperation<TState>>.Start => 0;

        int IContainer<IOperation<TState>>.Count => Count;


        private void VerifyIndex(int index)
        {
            if (HasIndex(index))
                throw new IndexOutOfRangeException();
        }

        private bool HasIndex(int index) => (uint)index >= Count;

        bool ILookup<int, IOperation<TState>>.HasKey(int key) => HasIndex(key);


        IOperation<TState> ILookup<int, IOperation<TState>>.this[int key]
        {
            get
            {
                VerifyIndex(key);
                return GetSubOperation(key);
            }
        }

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
                if (GetSubOperation(i) == value)
                    return true;

            return false;
        }


        private IEnumerator<IOperation<TState>> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return GetSubOperation(i);
        }

        IEnumerator<IOperation<TState>> IEnumerable<IOperation<TState>>.GetEnumerator() => GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerable<int> ILookup<int, IOperation<TState>>.Keys => Enumerables.GetRange(0, Count);

        IEnumerable<(int, IOperation<TState>)> ILookup<int, IOperation<TState>>.Pairs 
        {
            get
            {
                for (int i = 0; i < Count;i++)
                    yield return (i, GetSubOperation(i));
            }
        }

        #endregion
    }
}