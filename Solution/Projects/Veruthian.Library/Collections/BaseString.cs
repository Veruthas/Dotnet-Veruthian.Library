using System;
using System.Collections.Generic;
using Veruthian.Library.Numeric;
using Veruthian.Library.Utility;
using Veruthian.Library.Utility.Extensions;

namespace Veruthian.Library.Collections
{
    public abstract class BaseString<A, T, TString> : BaseFixedVector<A, T, TString>, IEquatable<TString>, IComparable<TString>
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
            if (other.IsNull())
                return false;
            else if (object.ReferenceEquals(this, other))
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
            if (other.IsNull())
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
            if (other.IsNull())
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
            if (other.IsNull())
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
            if (other.IsNull())
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

        public bool EqualsVector<B>(IVector<B, T> other) where B : ISequential<B> => EqualsVector(other, EqualityComparer);

        public bool EqualsVector(IVector<T> other) => EqualsVector(other, EqualityComparer);

        public bool EqualsArray(params T[] other) => EqualsArray(other, EqualityComparer);

        public bool EqualsEnumerable(IEnumerable<T> other) => EqualsEnumerable(other, EqualityComparer);


        public override bool Equals(object other)
        {
            if (other.IsNull())
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


        public static bool Equals(TString left, object right)
            => left.IsNull() ? right.IsNull() : left.Equals(right);


        public static bool Equals(TString left, TString right)
            => left.IsNull() ? right.IsNull() : left.Equals(right);

        public static bool EqualsVector(TString left, IVector<A, T> right)
            => left.IsNull() ? right.IsNull() : left.EqualsVector(right);

        public static bool EqualsVector<B>(TString left, IVector<B, T> right) where B : ISequential<B>
            => left.IsNull() ? right.IsNull() : left.EqualsVector<B>(right);

        public static bool EqualsArray(TString left, params T[] right)
            => left.IsNull() ? right.IsNull() : left.EqualsArray(right);

        public static bool EqualsEnumerable(TString left, IEnumerable<T> right)
            => left.IsNull() ? right.IsNull() : left.EqualsEnumerable(right);



        public static bool Equals(TString left, TString right, EqualityComparison<T> equals)
            => left.IsNull() ? right.IsNull() : left.Equals(right, equals);

        public static bool EqualsVector(TString left, IVector<A, T> right, EqualityComparison<T> equals)
            => left.IsNull() ? right.IsNull() : left.EqualsVector(right, equals);

        public static bool EqualsVector<B>(TString left, IVector<B, T> right, EqualityComparison<T> equals) where B : ISequential<B>
            => left.IsNull() ? right.IsNull() : left.EqualsVector<B>(right, equals);

        public static bool EqualsArray(TString left, T[] right, EqualityComparison<T> equals)
            => left.IsNull() ? right.IsNull() : left.EqualsArray(right, equals);

        public static bool EqualsEnumerable(TString left, IEnumerable<T> right, EqualityComparison<T> equals)
            => left.IsNull() ? right.IsNull() : left.EqualsEnumerable(right, equals);

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

        public int CompareToEnumerable(IEnumerable<T> other, Comparison<T> comparer)
            => CompareEnumerable(comparer, this.GetEnumerator(), other.GetEnumerator());


        public int CompareTo(TString other)
            => CompareTo(other, Comparer);

        public int CompareToVector(IVector<A, T> other)
            => CompareEnumerable(Comparer, this.GetEnumerator(), other.GetEnumerator());

        public int CompareToVector(IVector<T> other)
            => CompareEnumerable(Comparer, this.GetEnumerator(), other.GetEnumerator());

        public int CompareToVector<B>(IVector<B, T> other) where B : ISequential<B>
            => CompareEnumerable(Comparer, this.GetEnumerator(), other.GetEnumerator());

        public int CompareToArray(T[] other)
            => CompareEnumerable(Comparer, this.GetEnumerator(), GetEnumerable(other).GetEnumerator());

        public int CompareToEnumerable(IEnumerable<T> other)
            => CompareEnumerable(Comparer, this.GetEnumerator(), other.GetEnumerator());


        public static int Compare(TString left, TString right)
            => left == null ? (right == null ? 0 : -1) : left.CompareTo(right);

        public static int CompareToVector(TString left, IVector<A, T> right)
            => left == null ? (right == null ? 0 : -1) : left.CompareToVector(right);

        public static int CompareToVector(TString left, IVector<T> right)
            => left == null ? (right == null ? 0 : -1) : left.CompareToVector(right);

        public static int CompareToVector<B>(TString left, IVector<B, T> right) where B : ISequential<B>
            => left == null ? (right == null ? 0 : -1) : left.CompareToVector(right);

        public static int CompareToArray(TString left, T[] right)
            => left == null ? (right == null ? 0 : -1) : left.CompareToArray(right);

        public static int CompareToEnumerable(TString left, IEnumerable<T> right)
            => left == null ? (right == null ? 0 : -1) : left.CompareToEnumerable(right);


        public static int Compare(TString left, TString right, Comparison<T> comparer)
            => left == null ? (right == null ? 0 : -1) : left.CompareTo(right, comparer);

        public static int CompareToVector(TString left, IVector<A, T> right, Comparison<T> comparer)
            => left == null ? (right == null ? 0 : -1) : left.CompareToVector(right, comparer);

        public static int CompareToVector(TString left, IVector<T> right, Comparison<T> comparer)
            => left == null ? (right == null ? 0 : -1) : left.CompareToVector(right, comparer);

        public static int CompareToVector<B>(TString left, IVector<B, T> right, Comparison<T> comparer) where B : ISequential<B>
            => left == null ? (right == null ? 0 : -1) : left.CompareToVector(right, comparer);

        public static int CompareToArray(TString left, T[] right, Comparison<T> comparer)
            => left == null ? (right == null ? 0 : -1) : left.CompareToArray(right, comparer);

        public static int CompareToEnumerable(TString left, IEnumerable<T> right, Comparison<T> comparer)
            => left == null ? (right == null ? 0 : -1) : left.CompareToEnumerable(right, comparer);

        #endregion

        #region Hashcode

        int hashcode;

        // Hashcode
        public override int GetHashCode() => this.hashcode;

        protected override void Initialize(T[] items)
        {
            base.Initialize(items);

            this.hashcode = HashCodes.Default.Combine(Count.ToSignedInt(), this.Items);
        }


        #endregion
    }


    public abstract class BaseString<T, TString> : BaseString<Number, T, TString>, IVector<T>
        where TString : BaseString<T, TString>, new()
    {
        public override Number Start => Number.Zero;

        protected override int VerifiedAddressToIndex(Number address) => (int)address;

        protected override Number OffsetStartAddress(Number offset) => offset;

        public override bool IsValidAddress(Number address) => address < Count;
    }
}