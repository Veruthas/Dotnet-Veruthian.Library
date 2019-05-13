using System;
using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps
{
    public abstract class Step : IStep, IVector<IStep>
    {
        #region Abstracts

        public abstract string Description { get; }

        protected abstract int SubStepCount { get; }

        protected abstract IStep GetSubStep(int verifiedAddress);

        #endregion

        #region Implemented        

        public override string ToString() => Description;

        #endregion

        #region SubSteps

        IVector<IStep> IStep.SubSteps => this;

        int IVector<int, IStep>.Start => 0;

        int IContainer<IStep>.Count => SubStepCount;


        private void VerifyAddress(int address)
        {
            if (!HasAddress(address))
                throw new ArgumentOutOfRangeException(nameof(address));
        }

        private bool HasAddress(int address) => (uint)address < SubStepCount;

        bool ILookup<int, IStep>.HasAddress(int address) => HasAddress(address);


        IStep ILookup<int, IStep>.this[int address]
        {
            get
            {
                VerifyAddress(address);

                return GetSubStep(address);
            }
        }

        bool ILookup<int, IStep>.TryGet(int address, out IStep value)
        {
            if (HasAddress(address))
            {
                value = GetSubStep(address);

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

        IEnumerable<int> ILookup<int, IStep>.Addresses => Enumerables.GetRange(0, SubStepCount);

        IEnumerable<(int Address, IStep Value)> ILookup<int, IStep>.Pairs
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