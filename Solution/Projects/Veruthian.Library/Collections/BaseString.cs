using System;
using System.Collections.Generic;
using Veruthian.Library.Numeric;
using Veruthian.Library.Utility;

namespace Veruthian.Library.Collections
{
    public abstract class BaseString<A, T, TString> : BaseVector<A, T, TString>, IEquatable<TString>, IComparable<TString>
        where A : ISequential<A>
        where TString : BaseString<A, T, TString>, new()
    {
        public static readonly TString Empty = new TString();

        #region Equality

        protected static EqualityComparison<T> EqualityComparer = GetDefaultEqualityComparison();

        private static EqualityComparison<T> GetDefaultEqualityComparison()
        {
            if (typeof(IEquatable<T>).IsAssignableFrom(typeof(T)))
                return (left, right) => ((IEquatable<T>)left).Equals(right);
            else
                return (left, right) => Equals(left, right);
        }

        private static IEnumerable<T> GetEnumerable(T[] array) => array;

        private static bool EnumeratorsEqual(EqualityComparison<T> equals, IEnumerator<T> left, IEnumerator<T> right)
        {
            ExceptionHelper.VerifyNotNull(equals, nameof(equals));

            while (left.MoveNext())
            {
                if (!right.MoveNext())
                    return false;

                if (!equals(left.Current, right.Current))
                    return false;
            }

            return !right.MoveNext();
        }


        public bool Equals(TString other, EqualityComparison<T> equals)
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

                return EnumeratorsEqual(equals, items, others);
            }
        }

        public bool EqualsVector(IVector<A, T> other, EqualityComparison<T> equals)
        {
            if (other == null)
                return false;
            else if (this.Count != other.Count)
                return false;
            else
            {
                var items = this.GetEnumerator();

                var others = other.GetEnumerator();

                return EnumeratorsEqual(equals, items, others);
            }
        }

        public bool EqualsVector<B>(IVector<B, T> other, EqualityComparison<T> equals)
            where B : ISequential<B>
        {
            if (other == null)
                return false;
            else if (this.Count != other.Count)
                return false;
            else
            {
                var items = this.GetEnumerator();

                var others = other.GetEnumerator();

                return EnumeratorsEqual(EqualityComparer, items, others);
            }
        }

        public bool EqualsVector(IVector<T> other, EqualityComparison<T> equals)
        {
            if (other == null)
                return false;
            else if (this.Count != other.Count)
                return false;
            else
            {
                var items = this.GetEnumerator();

                var others = other.GetEnumerator();

                return EnumeratorsEqual(EqualityComparer, items, others);
            }
        }

        public bool EqualsArray(T[] other, EqualityComparison<T> equals)
        {
            if (other == null)
                return false;
            else if (this.Count != other.Length)
                return false;
            else
            {
                var items = this.GetEnumerator();

                var others = GetEnumerable(other).GetEnumerator();

                return EnumeratorsEqual(EqualityComparer, items, others);
            }
        }

        public bool EqualsEnumerable(IEnumerable<T> other, EqualityComparison<T> equals)
        {
            var items = this.GetEnumerator();

            var others = other.GetEnumerator();

            return EnumeratorsEqual(EqualityComparer, items, others);
        }



        public bool Equals(TString other) => Equals(other, EqualityComparer);

        public bool EqualsVector(IVector<A, T> other) => EqualsVector(other, EqualityComparer);

        public bool EqualsVector<OA>(IVector<OA, T> other) where OA : ISequential<OA> => EqualsVector(other, EqualityComparer);

        public bool EqualsVector(IVector<T> other) => EqualsVector(other, EqualityComparer);

        public bool EqualsArray(T[] other) => EqualsArray(other, EqualityComparer);

        public bool EqualsEnumerable(IEnumerable<T> other) => EqualsEnumerable(other, EqualityComparer);

        public override bool Equals(object other)
        {
            if (other == null)
                return false;
            else if (other is TString)
                return Equals((TString)other);
            else if (other is IVector<T>)
                return EqualsVector((IVector<T>)other);
            else if (other is T[])
                return EqualsArray((T[])other);
            else if (other is IEnumerable<T>)
                return EqualsEnumerable((IEnumerable<T>)other);
            else
                return false;
        }

        #endregion

        #region Comparison

        protected static Comparison<T> Comparer = GetDefaultComparison();

        private static Comparison<T> GetDefaultComparison()
        {
            if (typeof(IComparable<T>).IsAssignableFrom(typeof(T)))
                return (left, right) => ((IComparable<T>)left).CompareTo(right);
            else
                return (left, right) => 0;
        }

        public static int CompareEnumerable(Comparison<T> comparer, IEnumerator<T> left, IEnumerator<T> right)
        {
            ExceptionHelper.VerifyNotNull(comparer, nameof(comparer));

            while (left.MoveNext())
            {
                if (!right.MoveNext())
                    return 1;

                var result = comparer(left.Current, right.Current);

                if (result != 0)
                    return result;
            }

            return right.MoveNext() ? -1 : 0;
        }

        public int CompareTo(TString other, Comparison<T> comparer)
            => CompareEnumerable(comparer, this.GetEnumerator(), other.GetEnumerator());

        public int CompareToVector(IVector<A, T> other, Comparison<T> comparer)
            => CompareEnumerable(comparer, this.GetEnumerator(), other.GetEnumerator());

        public int CompareToVector(IVector<T> other, Comparison<T> comparer)
            => CompareEnumerable(comparer, this.GetEnumerator(), other.GetEnumerator());

        public int CompareToVector<B>(IVector<B, T> other, Comparison<T> comparer) where B : ISequential<B>
            => CompareEnumerable(comparer, this.GetEnumerator(), other.GetEnumerator());

        public int CompareToArray(T[] other, Comparison<T> comparer) 
            => CompareEnumerable(comparer, this.GetEnumerator(), GetEnumerable(other).GetEnumerator());

        public int CompareToEnumeratble(IEnumerable<T> other, Comparison<T> comparer)
            => CompareEnumerable(comparer, this.GetEnumerator(), other.GetEnumerator());

        public int CompareTo(TString other)
            => CompareTo(other, Comparer);

        #endregion

        #region Hashcode

        int hashcode;

        // Hashcode
        public override int GetHashCode() => this.hashcode;

        protected override void OnSizeSet()
        {
            this.hashcode = HashCodes.Default.Combine((int)this.Size, this.Items);
        }

        #endregion
    }
}