using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Numeric
{
    public abstract class Enum<E> : IBounded<E>, ISequential<E>
        where E : Enum<E>
    {
        readonly string name;

        readonly int ordinal;


        protected Enum(string name)
        {
            this.name = name;

            this.ordinal = items.Count;

            items.Add((E)this);

            names.Add(name, (E)this);
        }

        public string Name => name;

        public int Ordinal => ordinal;



        public override int GetHashCode() => ordinal.GetHashCode();


        public static bool TryParse(string value, out E result) => names.TryGetValue(value, out result);

        public override string ToString() => name;


        #region Operators

        public static E MinValue => items.Count == 0 ? null : items[0];

        public static E MaxValue => items.Count == 0 ? null : items[items.Count - 1];

        E IBounded<E>.MinValue => MinValue;

        E IBounded<E>.MaxValue => MaxValue;


        public E Next
        {
            get
            {
                if (this.ordinal == items.Count - 1)
                    throw new OverflowException("Item is upper bound");
                else
                    return items[this.ordinal + 1];
            }
        }

        public E Previous
        {
            get
            {
                if (this.ordinal == 0)
                    throw new OverflowException("Item is lower bound");
                else
                    return items[this.ordinal - 1];
            }
        }

        public bool Precedes(E other) => other == null ? false : this.ordinal < other.ordinal;

        public bool Follows(E other) => other == null ? true : this.ordinal > other.ordinal;

        public int CompareTo(E other) => other == null ? -1 : this.ordinal.CompareTo(other.ordinal);


        public bool Equals(E other) => this == other;


        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is E))
                return false;
            else
                return Equals(obj as E);
        }

        public static bool operator ==(Enum<E> left, Enum<E> right) => left.ordinal == right.ordinal;

        public static bool operator !=(Enum<E> left, Enum<E> right) => left.ordinal != right.ordinal;

        public static bool operator <(Enum<E> left, Enum<E> right) => left.ordinal < right.ordinal;

        public static bool operator >(Enum<E> left, Enum<E> right) => left.ordinal > right.ordinal;

        public static bool operator <=(Enum<E> left, Enum<E> right) => left.ordinal <= right.ordinal;

        public static bool operator >=(Enum<E> left, Enum<E> right) => left.ordinal >= right.ordinal;

        #endregion

        #region Items

        public static IEnumerable<E> Items
        {
            get
            {
                foreach (var item in items)
                    yield return item;
            }
        }

        public static E GetByOrdinal(int ordinal) => (uint)ordinal >= items.Count ? throw new IndexOutOfRangeException() : items[ordinal];

        static List<E> items = new List<E>();

        static Dictionary<string, E> names = new Dictionary<string, E>();

        #endregion
    }
}