using System;
using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Collections;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Steps
{
    public abstract class Step : IStep, IVector<IStep>
    {
        #region Abstracts

        public abstract string Description { get; }

        protected abstract Number SubStepCount { get; }

        protected abstract IStep GetSubStep(Number verifiedAddress);

        #endregion

        #region Implemented        

        public override string ToString() => Description;

        #endregion

        #region SubSteps

        IVector<IStep> IStep.SubSteps => this;

        Number IVector<Number, IStep>.Start => new Number();

        Number IContainer<IStep>.Count => SubStepCount;


        private void VerifyAddress(Number address)
        {
            if (!HasAddress(address))
                throw new ArgumentOutOfRangeException(nameof(address));
        }

        private bool HasAddress(Number address) => address < SubStepCount;

        bool ILookup<Number, IStep>.HasAddress(Number address) => HasAddress(address);


        IStep ILookup<Number, IStep>.this[Number address]
        {
            get
            {
                VerifyAddress(address);

                return GetSubStep(address);
            }
        }

        bool ILookup<Number, IStep>.TryGet(Number address, out IStep value)
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

        IEnumerable<Number> ILookup<Number, IStep>.Addresses => Enumerables.GetRange(Number.Zero, SubStepCount);

        IEnumerable<(Number Address, IStep Value)> ILookup<Number, IStep>.Pairs
        {
            get
            {
                for (var i = Number.Zero; i < SubStepCount; i++)
                    yield return (i, GetSubStep(i));
            }
        }

        #endregion
    }
}