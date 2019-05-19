using System;
using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Numeric.Binary
{
    public struct Nibble : INumeric<Nibble>, IBounded<Nibble>, ILogical<Nibble>, IVector<bool>, IFormattable
    {
        private readonly byte value;


        private Nibble(int value) => this.value = (byte)(value & mask);


        #region Constants

        const byte mask = 0xF;

        public static readonly Nibble MinValue = new Nibble(0x0);

        public static readonly Nibble MaxValue = new Nibble(0xF);

        #endregion

        #region Conversion

        public static implicit operator byte(Nibble value) => value.value;

        public static implicit operator ushort(Nibble value) => value.value;

        public static implicit operator uint(Nibble value) => value.value;

        public static implicit operator ulong(Nibble value) => value.value;

        public static implicit operator sbyte(Nibble value) => (sbyte)value.value;

        public static implicit operator short(Nibble value) => value.value;

        public static implicit operator int(Nibble value) => value.value;

        public static implicit operator long(Nibble value) => value.value;

        public static implicit operator float(Nibble value) => value.value;

        public static implicit operator double(Nibble value) => value.value;

        public static explicit operator char(Nibble value) => (char)value.value;


        public static implicit operator Nibble(byte value) => new Nibble(value);

        public static explicit operator Nibble(ushort value) => new Nibble(value);

        public static explicit operator Nibble(uint value) => new Nibble((int)value);

        public static explicit operator Nibble(ulong value) => new Nibble((int)value);

        public static explicit operator Nibble(sbyte value) => new Nibble(value);

        public static explicit operator Nibble(short value) => new Nibble(value);

        public static explicit operator Nibble(int value) => new Nibble(value);

        public static explicit operator Nibble(long value) => new Nibble((int)value);

        public static explicit operator Nibble(float value) => new Nibble((int)value);

        public static explicit operator Nibble(double value) => new Nibble((int)value);

        public static explicit operator Nibble(char value) => new Nibble(value);

        #endregion

        #region Comparison

        public override int GetHashCode() => value.GetHashCode();

        public bool Equals(Nibble other) => this.value == other.value;

        public override bool Equals(object obj) => (obj is Nibble) ? this.Equals((Nibble)obj) : false;

        public int CompareTo(Nibble other) => this.value.CompareTo(other.value);


        public static bool operator ==(Nibble left, Nibble right) => left.value == right.value;

        public static bool operator !=(Nibble left, Nibble right) => left.value != right.value;


        public static bool operator <(Nibble left, Nibble right) => left.value < right.value;

        public static bool operator >(Nibble left, Nibble right) => left.value > right.value;

        public static bool operator <=(Nibble left, Nibble right) => left.value <= right.value;

        public static bool operator >=(Nibble left, Nibble right) => left.value >= right.value;

        #endregion

        #region Operators

        public static Nibble operator ++(Nibble value) => unchecked(new Nibble(value.value + 1));

        public static Nibble operator --(Nibble value) => unchecked(new Nibble(value.value - 1));

        public static Nibble operator +(Nibble left, Nibble right) => unchecked(new Nibble(left.value + right.value));

        public static Nibble operator -(Nibble left, Nibble right) => unchecked(new Nibble(left.value - right.value));

        public static Nibble operator *(Nibble left, Nibble right) => unchecked(new Nibble(left.value * right.value));

        public static Nibble operator /(Nibble left, Nibble right) => unchecked(new Nibble(left.value / right.value));

        public static Nibble operator %(Nibble left, Nibble right) => unchecked(new Nibble(left.value % right.value));


        public static Nibble operator ~(Nibble value) => new Nibble(~value.value);

        public static Nibble operator &(Nibble left, Nibble right) => new Nibble(left.value & right.value);

        public static Nibble operator |(Nibble left, Nibble right) => new Nibble(left.value | right.value);

        public static Nibble operator ^(Nibble left, Nibble right) => new Nibble(left.value ^ right.value);

        #endregion

        #region INumeric

        public bool Precedes(Nibble other) => this < other;

        public bool Follows(Nibble other) => this > other;

        Nibble INumeric<Nibble>.Sum(Nibble addend) => this + addend;

        Nibble INumeric<Nibble>.Difference(Nibble subtrahend) => this - subtrahend;

        Nibble INumeric<Nibble>.Delta(Nibble subtrahend) => this - subtrahend;

        Nibble INumeric<Nibble>.Product(Nibble multiplicand) => this * multiplicand;

        Nibble INumeric<Nibble>.Power(Nibble exponent)
        {
            ulong last = value;

            ulong result = 0;

            while (exponent != 0)
            {
                if ((exponent & 1) == 1)
                    result *= last;

                exponent >>= 1;

                last *= last;
            }

            return new Nibble((int)result);
        }

        Nibble INumeric<Nibble>.Quotient(Nibble divisor) => this / divisor;

        Nibble INumeric<Nibble>.Remainder(Nibble divisor) => this % divisor;

        (Nibble Quotient, Nibble Remainder) INumeric<Nibble>.Division(Nibble divisor) => (this / divisor, this % divisor);



        #endregion

        #region IBounded

        Nibble IBounded<Nibble>.MinValue => Nibble.MinValue;

        Nibble IBounded<Nibble>.MaxValue => Nibble.MaxValue;

        #endregion

        #region IBinary

        Nibble ILogical<Nibble>.And(Nibble other) => this & other;

        Nibble ILogical<Nibble>.Or(Nibble other) => this | other;

        Nibble ILogical<Nibble>.Xor(Nibble other) => this ^ other;

        Nibble ILogical<Nibble>.Not() => ~this;


        const int bits = 4;

        int IVector<int, bool>.Start => 0;

        bool ILookup<int, bool>.this[int address] => TryGet(address, out var result) ? result : throw new ArgumentOutOfRangeException(nameof(address));


        bool IContainer<bool>.Contains(bool value) => (value && this.value != 0) || (!value && this.value != 0xF);

        bool ILookup<int, bool>.HasAddress(int address) => (uint)address <= bits;

        private bool TryGet(int address, out bool value)
        {
            if ((uint)address <= bits)
            {
                value = ((this.value >> address) & 0x1) == 1;

                return true;
            }
            else
            {
                value = false;

                return false;
            }
        }

        bool ILookup<int, bool>.TryGet(int address, out bool value) => TryGet(address, out value);


        int IContainer<bool>.Count => bits;


        public IEnumerator<bool> GetEnumerator()
        {
            for (int i = 0; i < bits; i++)
                yield return ((this.value >> i) & 0x1) == 0x1;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerable<int> ILookup<int, bool>.Addresses
        {
            get
            {
                for (int i = 0; i < bits; i++)
                    yield return i;
            }
        }

        IEnumerable<(int Address, bool Value)> ILookup<int, bool>.Pairs
        {
            get
            {
                for (int i = 0; i < bits; i++)
                    yield return (i, ((this.value >> i) & 0x1) == 0x1);
            }
        }

        #endregion

        #region Strings

        public string ToString(bool uppercase)
        {
            char result;

            if (value >= 10)
            {
                if (uppercase)
                {
                    result = (char)('A' + (value - 10));
                }
                else
                {
                    result = (char)('a' + (value - 10));
                }
            }
            else
            {
                result = (char)(value + '0');
            }

            return result.ToString();
        }

        public override string ToString() => ToString(true);

        public string ToString(IFormatProvider provider) => value.ToString(provider);

        public string ToString(string format) => value.ToString(format);

        public string ToString(string format, IFormatProvider provider) => value.ToString(format, provider);


        public bool TryParse(string value, out Nibble result)
        {
            if (byte.TryParse(value, out byte byteResult) && byteResult <= 15)
            {
                result = new Nibble(byteResult);

                return true;
            }
            else
            {
                result = default(Nibble);

                return false;
            }
        }

        #endregion
    }
}