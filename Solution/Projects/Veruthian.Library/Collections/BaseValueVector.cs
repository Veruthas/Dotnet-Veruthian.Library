using System;
using System.Collections.Generic;

namespace Veruthian.Library.Collections
{
    public abstract class BaseValueVector<T, TVector> : BaseVector<T, TVector>, IEquatable<TVector>
        where TVector : BaseValueVector<T, TVector>, new()
    {
        protected int hashCode;

        protected abstract bool Equals(T a, T b);

        public bool Equals(TVector other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IVector<T> other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object other)
        {
            if (other == null)
                return false;
            else if (this == other)
                return true;
            else if (other is TVector)
                return Equals((TVector)other);
            else if (other is IVector<T>)
                return Equals((IVector<T>)other);
            else if (other is IEnumerable<T>)
                return Equals((IVector<T>)other);
            else
                return false;
        }

        public override int GetHashCode() => hashCode;


    }
}