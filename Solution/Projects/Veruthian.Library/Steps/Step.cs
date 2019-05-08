using System;
using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps
{
    public abstract class Step : IStep, IIndex<IStep>
    {
        #region Abstracts

        public abstract string Description { get; }

        protected abstract int SubStepCount { get; }

        protected abstract IStep GetSubStep(int verifiedIndex);

        #endregion

        #region Implemented

        public IIndex<IStep> SubSteps => this;

        public override string ToString() => Description;

        #endregion

        #region SubSteps

        int IIndex<int, IStep>.Start => 0;

        int IContainer<IStep>.Count => SubStepCount;


        private void VerifyIndex(int index)
        {
            if (!HasIndex(index))
                throw new IndexOutOfRangeException();
        }

        private bool HasIndex(int index) => (uint)index < SubStepCount;

        bool ILookup<int, IStep>.HasKey(int key) => HasIndex(key);


        IStep ILookup<int, IStep>.this[int key]
        {
            get
            {
                VerifyIndex(key);

                return GetSubStep(key);
            }
        }

        bool ILookup<int, IStep>.TryGet(int key, out IStep value)
        {
            if (HasIndex(key))
            {
                value = GetSubStep(key);

                return true;
            }
            else
            {
                value = default(IStep);

                return false;
            }
        }


        bool IContainer<IStep>.Contains(IStep value)
        {
            for (int i = 0; i < SubStepCount; i++)
                if (GetSubStep(i) == value)
                    return true;

            return false;
        }


        private IEnumerator<IStep> GetEnumerator()
        {
            for (int i = 0; i < SubStepCount; i++)
                yield return GetSubStep(i);
        }

        IEnumerator<IStep> IEnumerable<IStep>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerable<int> ILookup<int, IStep>.Keys => Enumerables.GetRange(0, SubStepCount);

        IEnumerable<(int, IStep)> ILookup<int, IStep>.Pairs
        {
            get
            {
                for (int i = 0; i < SubStepCount; i++)
                    yield return (i, GetSubStep(i));
            }
        }

        #endregion
    }
}