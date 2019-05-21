using System;
using System.Collections.Generic;
using Veruthian.Library.Numeric;
using Veruthian.Library.Utility;

namespace Veruthian.Library.Collections
{
    public abstract class BaseString<A, T, TVector> : BaseVector<A, T, TVector>, IEquatable<TVector>
        where A : ISequential<A>
        where TVector : BaseString<A, T, TVector>, new()
    {
        private static readonly Func<T, T, bool> comparer = GetComparer();

        private static Func<T, T, bool> GetComparer()
        {
            if (typeof(IEquatable<T>).IsAssignableFrom(typeof(T)))
                return (left, right) => ((IEquatable<T>)left).Equals(right);
            else
                return (left, right) => Equals(left, right);
        }

        protected virtual bool ItemEquals(T left, T right) => comparer(left, right);

        private static IEnumerable<T> GetEnumerable(T[] array) => array;

        private bool CompareEnumerators(IEnumerator<T> left, IEnumerator<T> right)
        {
            while (left.MoveNext())
            {
                right.MoveNext();

                if (!ItemEquals(left.Current, right.Current))
                    return false;
            }

            return !right.MoveNext();
        }

        public bool Equals(TVector other)
        {
            if (other == null)
                return false;
            else if (Equals(this, other))
                return true;
            else if (this.Count != other.Count)
                return false;
            else if (this.hashcode != other.hashcode)
                return false;
            else
            {
                var items = this.GetEnumerator();

                var others = other.GetEnumerator();

                return CompareEnumerators(items, others);
            }
        }

        public bool Equals(IVector<A, T> other)
        {
            if (other == null)
                return false;
            else if (this.Count != other.Count)
                return false;
            else
            {
                var items = this.GetEnumerator();

                var others = other.GetEnumerator();

                return CompareEnumerators(items, others);
            }
        }

        public bool Equals<AO>(IVector<AO, T> other)
            where AO : ISequential<AO>
        {
            if (other == null)
                return false;
            else if (this.Count != other.Count)
                return false;
            else
            {
                var items = this.GetEnumerator();

                var others = other.GetEnumerator();

                return CompareEnumerators(items, others);
            }
        }

        public bool Equals(IVector<T> other)
        {
            if (other == null)
                return false;
            else if (this.Count != other.Count)
                return false;
            else
            {
                var items = this.GetEnumerator();

                var others = other.GetEnumerator();

                return CompareEnumerators(items, others);
            }
        }


        public bool Equals(T[] other)
        {
            if (other == null)
                return false;
            else if (this.Count != other.Length)
                return false;
            else
            {
                var items = this.GetEnumerator();

                var others = GetEnumerable(other).GetEnumerator();

                return CompareEnumerators(items, others);
            }
        }


        public bool Equals(IEnumerable<T> other)
        {
            var items = this.GetEnumerator();

            var others = other.GetEnumerator();

            return CompareEnumerators(items, others);
        }


        public override bool Equals(object other)
        {
            if (other == null)
                return false;
            else if (other is TVector)
                return Equals((TVector)other);
            else if (other is IVector<T>)
                return Equals((IVector<T>)other);
            else if (other is T[])
                return Equals((T[])other);
            else if (other is IEnumerable<T>)
                return Equals((IEnumerable<T>)other);
            else
                return false;
        }

        int hashcode;

        // Hashcode
        public override int GetHashCode() => this.hashcode;

        protected override void OnSizeSet()
        {
            this.hashcode = HashCodes.Default.Combine((int)this.Size, this.Items);
        }
    }
}