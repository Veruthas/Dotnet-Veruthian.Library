using System;
using Veruthian.Dotnet.Library.Data;

namespace Veruthian.Dotnet.Library.Numeric
{
    public struct Nibble : ISequential<Nibble>, IFormattable, IConvertible
    {
        const byte mask = 0xF;

        public static readonly Nibble MinValue = new Nibble(0x0);

        public static readonly Nibble MaxValue = new Nibble(0xF);


        private readonly byte value;


        private Nibble(int value) => this.value = (byte)(value & mask);



        #region StringConverter

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

        #region Equality/Comparison

        public override int GetHashCode() => value.GetHashCode();

        public override bool Equals(object obj) => (obj is Nibble) ? this.Equals((Nibble)obj) : false;

        public static bool operator ==(Nibble left, Nibble right) => left.value == right.value;

        public static bool operator !=(Nibble left, Nibble right) => left.value != right.value;

        public bool Equals(Nibble other) => this.value == other.value;

        public static bool operator <(Nibble left, Nibble right) => left.value < right.value;

        public static bool operator >(Nibble left, Nibble right) => left.value > right.value;

        public static bool operator <=(Nibble left, Nibble right) => left.value <= right.value;

        public static bool operator >=(Nibble left, Nibble right) => left.value >= right.value;

        public int CompareTo(Nibble other) => this.value.CompareTo(other.value);

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

        TypeCode IConvertible.GetTypeCode() => TypeCode.Byte;
        bool IConvertible.ToBoolean(IFormatProvider provider) => ((IConvertible)this.value).ToBoolean(provider);
        byte IConvertible.ToByte(IFormatProvider provider) => ((IConvertible)this.value).ToByte(provider);
        char IConvertible.ToChar(IFormatProvider provider) => ((IConvertible)this.value).ToChar(provider);
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => ((IConvertible)this.value).ToDateTime(provider);
        decimal IConvertible.ToDecimal(IFormatProvider provider) => ((IConvertible)this.value).ToDecimal(provider);
        double IConvertible.ToDouble(IFormatProvider provider) => ((IConvertible)this.value).ToDouble(provider);
        short IConvertible.ToInt16(IFormatProvider provider) => ((IConvertible)this.value).ToInt16(provider);
        int IConvertible.ToInt32(IFormatProvider provider) => ((IConvertible)this.value).ToInt32(provider);
        long IConvertible.ToInt64(IFormatProvider provider) => ((IConvertible)this.value).ToInt64(provider);
        sbyte IConvertible.ToSByte(IFormatProvider provider) => ((IConvertible)this.value).ToSByte(provider);
        float IConvertible.ToSingle(IFormatProvider provider) => ((IConvertible)this.value).ToSingle(provider);

        string IConvertible.ToString(IFormatProvider provider) => ((IConvertible)this.value).ToString(provider);
        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => ((IConvertible)this.value).ToType(conversionType, provider);
        ushort IConvertible.ToUInt16(IFormatProvider provider) => ((IConvertible)this.value).ToUInt16(provider);
        uint IConvertible.ToUInt32(IFormatProvider provider) => ((IConvertible)this.value).ToUInt32(provider);
        ulong IConvertible.ToUInt64(IFormatProvider provider) => ((IConvertible)this.value).ToUInt64(provider);

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

        public static Nibble operator >>(Nibble value, int amount)
        {
            if (amount < 0)
                return value << -amount;
            else if (amount >= 4)
                return MinValue;
            else
                return new Nibble(value.value >> amount);
        }

        public static Nibble operator <<(Nibble value, int amount)
        {
            if (amount < 0)
                return value >> -amount;
            else if (amount >= 4)
                return MinValue;
            else
                return new Nibble(value.value << amount);
        }

        #endregion

        #region ISequential

        public bool Precedes(Nibble other) => this < other;

        public bool Follows(Nibble other) => this > other;

        Nibble ISequential<Nibble>.Next => this += 1;

        Nibble ISequential<Nibble>.Previous => this -= 1;

        Nibble IOrderable<Nibble>.Default => default(Nibble);

        Nibble IOrderable<Nibble>.MinValue => Nibble.MinValue;

        Nibble IOrderable<Nibble>.MaxValue => Nibble.MaxValue;

        #endregion
    }
}