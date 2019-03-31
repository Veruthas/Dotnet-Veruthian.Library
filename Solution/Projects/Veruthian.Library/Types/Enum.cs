using System;
using System.Collections.Generic;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Types
{
    public abstract class Enum<E> : IBounded<E>, ISequential<E>, IFormattable
        where E : Enum<E>
    {
        readonly string name;

        readonly int ordinal;


        protected Enum(string name)
        {
            this.name = name;

            this.ordinal = items.Count;

            if (names.ContainsKey(name))
                throw new ArgumentException($"Item with name '{name}' already exists!");

            items.Add((E)this);

            names.Add(name, (E)this);
        }

        public string Name => name;

        public int Ordinal => ordinal;


        public override int GetHashCode() => ordinal.GetHashCode();


        public override string ToString() => name;

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (!string.IsNullOrEmpty(format))
            {
                switch (char.ToUpper(format[0]))
                {                    
                    case 'G':
                        return name;
                    default:
                        return ordinal.ToString(format, formatProvider);
                }
            }

            return ToString();
        }

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

        public bool Follows(E other) => other == null ? false : this.ordinal > other.ordinal;

        public int CompareTo(E other) => other == null ? -1 : this.ordinal.CompareTo(other.ordinal);


        public bool Equals(E other) => this == other;


        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is E))
                return false;
            else
                return Equals(obj as E);
        }

        public static bool operator ==(Enum<E> left, Enum<E> right) => left?.ordinal == right?.ordinal;

        public static bool operator !=(Enum<E> left, Enum<E> right) => left?.ordinal != right?.ordinal;

        public static bool operator <(Enum<E> left, Enum<E> right) => left?.ordinal < right?.ordinal;

        public static bool operator >(Enum<E> left, Enum<E> right) => left?.ordinal > right?.ordinal;

        public static bool operator <=(Enum<E> left, Enum<E> right) => left?.ordinal <= right?.ordinal;

        public static bool operator >=(Enum<E> left, Enum<E> right) => left?.ordinal >= right?.ordinal;

        #endregion

        #region Items

        static List<E> items = new List<E>();

        static Dictionary<string, E> names = new Dictionary<string, E>();

        static Enum()
        {
            Type enumClass = typeof(E);

            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(enumClass.TypeHandle);
        }


        public static IEnumerable<E> Items
        {
            get
            {
                foreach (var item in items)
                    yield return item;
            }
        }


        public static bool TryGetByOrdinal(int ordinal, out E result)
        {
            if ((uint)ordinal <= items.Count)
            {
                result = items[ordinal];
                return true;
            }
            else
            {
                result = default(E);
                return false;
            }
        }

        public static bool TryGetByName(string name, out E result)
        {
            return names.TryGetValue(name, out result);
        }

        public static bool TryGetByName(string name, StringComparison comparisonType, out E result)
        {
            foreach (var item in items)
            {
                if (string.Equals(name, item.name, comparisonType))
                {
                    result = item;
                    return true;
                }
            }

            result = default(E);
            return false;
        }

        #endregion
    }
}