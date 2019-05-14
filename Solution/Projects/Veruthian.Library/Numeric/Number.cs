using System;
using System.Collections.Generic;
using System.Text;
using Veruthian.Library.Collections.Extensions;
using Veruthian.Library.Utility;

namespace Veruthian.Library.Numeric
{
    public struct Number : INumeric<Number>, ISequential<Number>
    {
        const int BitsInUnit = 32;


        public static readonly Number Default = Zero;

        public static readonly Number Zero = new Number(0);

        public static readonly Number One = new Number(1);


        readonly uint value;

        readonly uint[] units;


        private Number(uint value)
        {
            this.value = value;

            this.units = null;
        }

        private Number(params uint[] units)
            : this(0, units) { }

        private Number(uint value, uint[] units)
        {
            if (units == null || units.Length == 0)
            {
                this.value = value;

                this.units = null;
            }
            else if (units.Length == 1)
            {
                this.value = units[0];

                this.units = null;
            }
            else
            {
                this.value = 0;

                this.units = units;
            }
        }



        public bool IsZero => IsSimple && value == 0;

        private bool IsSimple => units == null;

        private uint this[int index] => units == null ? (index == 0 ? value : 0) : (index < units.Length ? units[index] : 0);

        private int UnitCount => units != null ? units.Length : 1;



        #region Comparison

        public bool Precedes(Number other)
        {
            // this < other?
            if (this.IsSimple)
            {
                return other.IsSimple ? this.value < other.value : true;
            }
            else
            {
                if (this.UnitCount == other.UnitCount)
                {
                    for (int i = this.UnitCount - 1; i >= 0; i--)
                        if (this.units[i] >= other.units[i])
                            return false;

                    return true;
                }
                else
                {
                    return this.UnitCount < other.UnitCount;
                }
            }
        }

        public bool Follows(Number other)
        {
            // this > other?
            if (this.IsSimple)
            {
                return other.IsSimple ? this.value > other.value : false;
            }
            else
            {
                if (this.UnitCount == other.UnitCount)
                {
                    for (int i = this.UnitCount - 1; i >= 0; i--)
                        if (this.units[i] <= other.units[i])
                            return false;

                    return true;
                }
                else
                {
                    return this.UnitCount > other.UnitCount;
                }
            }
        }

        public int CompareTo(Number other)
        {
            if (this.IsSimple)
            {
                if (other.IsSimple)
                {
                    return CompareValues(this.value, other.value);
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (this.UnitCount == other.UnitCount)
                {
                    for (int i = this.UnitCount - 1; i >= 0; i--)
                    {
                        var compare = CompareValues(this.units[i], other.units[i]);

                        if (compare != 0)
                            return compare;
                    }

                    return 0;
                }
                else if (this.UnitCount < other.UnitCount)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
        }

        private static int CompareValues(uint left, uint right)
        {
            if (left == right)
                return 0;
            else if (left < right)
                return -1;
            else
                return 1;
        }

        public bool Equals(Number other)
        {
            if (this.IsSimple)
            {
                return other.IsSimple && this.value == other.value;
            }
            else
            {
                if (this.UnitCount == other.UnitCount)
                {
                    for (int i = 0; i < this.UnitCount; i++)
                        if (this.units[i] != other.units[i])
                            return false;

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Number))
                return false;
            else
                return Equals((Number)obj);
        }

        public override int GetHashCode()
        {
            return IsSimple ? value.GetHashCode() : Utility.HashCodes.Default.Combine(units);
        }


        public static bool operator ==(Number left, Number right) => left.Equals(right);

        public static bool operator !=(Number left, Number right) => !left.Equals(right);

        public static bool operator <(Number left, Number right) => left.Precedes(right);

        public static bool operator >(Number left, Number right) => left.Follows(right);

        public static bool operator <=(Number left, Number right) => left.CompareTo(right) <= 0;

        public static bool operator >=(Number left, Number right) => left.CompareTo(right) >= 0;

        #endregion

        #region Arithmetic

        // Addition
        public Number Add(Number other) => Add(this, other);

        private static Number Add(Number left, Number right)
        {
            if (left.IsSimple && right.IsSimple)
            {
                ulong result = (ulong)left.value + (ulong)right.value;

                return result;
            }
            else
            {
                var units = new uint[Math.Max(left.UnitCount, right.UnitCount)];

                var carry = ulong.MinValue;

                for (int i = 0; i < units.Length; i++)
                {
                    ulong result = (ulong)left[i] + (ulong)right[i] + carry;

                    units[i] = (uint)result;

                    carry = result >> BitsInUnit;
                }

                if (carry != 0)
                    units = units.Append((uint)carry);

                return new Number(units);
            }
        }

        public Number Next => this.Add(One);


        // Subtraction
        public Number Subtract(Number other)
        {
            var result = Subtract(this, other);

            return result.Positive ? new Number(result.Value, result.Units) : Zero;
        }

        public Number CheckedSubtract(Number other)
        {
            var result = Subtract(this, other);

            if (!result.Positive)
                throw new InvalidOperationException("Cannot subtract larger number from smaller one");

            if (result.Units != null)
                TrimUnits(ref result.Units);

            return new Number(result.Value, result.Units);
        }

        public Number Delta(Number other)
        {
            var result = Subtract(this, other);

            if (result.Units != null)
            {
                if (!result.Positive)
                    AdjustNegative(result.Units);

                TrimUnits(ref result.Units);
            }

            return new Number(result.Value, result.Units);
        }

        private static (bool Positive, uint Value, uint[] Units) Subtract(Number a, Number b)
        {
            if (a.IsSimple && b.IsSimple)
            {
                if (a.value >= b.value)
                    return (true, a.value - b.value, null);
                else
                    return (false, b.value - a.value, null);
            }
            else
            {
                var count = Math.Max(a.UnitCount, b.UnitCount);

                var units = new uint[count];

                var carry = 1ul;

                for (int i = 0; i < count; i++)
                {
                    unchecked
                    {
                        var result = (ulong)a[i] + (ulong)(~b[i]) + carry;

                        units[i] = (uint)result;

                        carry = result >> BitsInUnit;
                    }
                }

                return (carry == 1, 0, units);
            }
        }

        private static void TrimUnits(ref uint[] units)
        {
            var adjusted = units.Length;

            for (int i = units.Length - 1; i >= 0; i--)
            {
                if (units[i] == 0)
                    adjusted--;
                else
                    break;
            }

            if (adjusted != units.Length)
                units = units.Resize(adjusted);
        }

        private static void AdjustNegative(uint[] units)
        {
            ulong carry = 1;

            for (int i = 0; i < units.Length; i++)
            {
                unchecked
                {
                    ulong inverse = ~units[i] + carry;

                    carry = inverse >> BitsInUnit;

                    units[i] = (uint)inverse;
                }
            }
        }

        public Number Previous => this.Subtract(One);


        // Multiplication
        public Number Multiply(Number other) => Multiply(this, other);

        private static Number Multiply(Number left, Number right)
        {
            if (left.IsSimple && right.IsSimple)
            {
                return (ulong)left.value * (ulong)right.value;
            }
            else if (left.IsZero || right.IsZero)
            {
                return Zero;
            }
            else
            {
                var units = new uint[left.UnitCount + right.UnitCount];

                for (int b = 0, column = 0; b < right.UnitCount; b++, column++)
                {
                    ulong value = 0;

                    for (var a = 0; a < left.UnitCount; a++)
                    {
                        // add (A*B) + previous results + carry
                        value = ((ulong)left[a] * (ulong)right[b]) + units[column + a] + value;

                        units[column + a] = (uint)value;

                        value >>= BitsInUnit;
                    }
                }

                TrimUnits(ref units);

                return new Number(units);
            }
        }


        // Division
        public Number Divide(Number other) => DivideWithRemainder(other, false).Quotient;

        public Number Modulus(Number other) => DivideWithRemainder(other, false).Remainder;

        public (Number Quotient, Number Remainder) DivideWithRemainder(Number other) => DivideWithRemainder(other, false);

        public Number CheckedDivide(Number other) => DivideWithRemainder(other, true).Quotient;

        public Number CheckedModulus(Number other) => DivideWithRemainder(other, true).Remainder;

        public (Number Quotient, Number Remainder) CheckedDivideWithRemainder(Number other) => DivideWithRemainder(other, true);

        public (Number Quotient, Number Remainder) DivideWithRemainder(Number other, bool checkedZero) => DivideWithRemainder(this, other, checkedZero);

        private static (Number Quotient, Number Remainder) DivideWithRemainder(Number left, Number right, bool checkedZero)
        {
            if (right.IsZero)
            {
                return checkedZero ? throw new DivideByZeroException() : (Zero, Zero);
            }
            else if (right.IsSimple)
            {
                if (left.IsSimple)
                    return (new Number(left.value / right.value), new Number(left.value % right.value));
                else
                    return DivideWithRemainder(left.units, right.value);
            }
            else
            {
                return DivideWithRemainder(left.units, right.units);
            }
        }

        private static (Number Quotient, Number Remainder) DivideWithRemainder(uint[] numerators, uint denominator)
        {
            var index = numerators.Length - 1;

            var numerator = (ulong)numerators[index];


            var remainder = numerator % denominator;

            numerator = numerator / denominator;


            var units = new uint[index + (numerator == 0 ? 0 : 1)];

            if (numerator != 0)
                units[index] = (uint)numerator;

            index--;

            while (index >= 0)
            {
                numerator = (remainder << BitsInUnit) + numerators[index];

                remainder = numerator % denominator;

                numerator = numerator / denominator;

                units[index--] = (uint)numerator;
            }

            return (new Number(units), new Number((uint)remainder));
        }

        private static (Number Quotient, Number Remainder) DivideWithRemainder(uint[] numerators, uint[] denominator)
        {
            // HACK: There is definitely a better algorithm out there...
            Number num = new Number(numerators);
            Number den = new Number(denominator);
            Number quo = new Number(0);

            while (num >= den)
            {
                num -= den;
                quo++;
            }

            return (quo, num);
        }

        #region Operators

        public static Number operator ++(Number value) => value.Next;

        public static Number operator --(Number value) => value.Previous;


        public static Number operator +(Number left, Number right) => left.Add(right);

        public static Number operator -(Number left, Number right) => left.Delta(right);

        public static Number operator *(Number left, Number right) => left.Multiply(right);

        public static Number operator /(Number left, Number right) => left.Divide(right);

        public static Number operator %(Number left, Number right) => left.Modulus(right);

        #endregion

        #endregion

        #region Conversion

        public static implicit operator Number(byte value) => new Number(value);

        public static implicit operator Number(ushort value) => new Number(value);

        public static implicit operator Number(uint value) => new Number(value);

        public static implicit operator Number(ulong value) => value <= uint.MaxValue ? new Number((uint)value) : new Number((uint)value, (uint)(value >> BitsInUnit));


        public static implicit operator Number(sbyte value) => new Number((uint)Math.Abs(value));

        public static implicit operator Number(short value) => new Number((uint)Math.Abs(value));

        public static implicit operator Number(int value) => new Number((uint)Math.Abs(value));

        public static implicit operator Number(long value) => (ulong)Math.Abs(value);


        public static explicit operator byte(Number value) => (byte)value[0];

        public static explicit operator ushort(Number value) => (ushort)value[0];

        public static explicit operator uint(Number value) => (uint)value[0];

        public static explicit operator ulong(Number value) => value.UnitCount > 1 ? ((ulong)value[1] << BitsInUnit) + value[0] : value.value;


        public static explicit operator sbyte(Number value) => (sbyte)(byte)value;

        public static explicit operator short(Number value) => (short)(ushort)value;

        public static explicit operator int(Number value) => (int)(uint)value;

        public static explicit operator long(Number value) => (long)(ulong)value;

        #endregion

        #region ToString

        public static readonly string Decimal = "0123456789";
        public static readonly string Binary = "01";
        public static readonly string Octal = "01234567";
        public static readonly string HexUpper = "0123456789ABCDEF";
        public static readonly string HexLower = "0123456789abcdef";


        public override string ToString() => ToString(Decimal, 3, ",");

        public string ToString(string symbols, int groupLength = 0, string separator = null, bool pad = false)
        {
            ExceptionHelper.VerifyNotNull(symbols, nameof(symbols));

            var builder = new StringBuilder();

            var actualLength = 0;

            Number numberBase = symbols.Length;

            if (numberBase == One)
            {
                var current = this;

                while (!this.IsZero)
                {
                    if (groupLength > 0 && actualLength != 0 && actualLength % groupLength == 0)
                        builder.Append(separator ?? "");

                    current--;

                    builder.Append(symbols[0]);

                    actualLength++;
                }
            }
            else if (this.IsZero)
            {
                builder.Append(symbols[0]);
            }
            else
            {
                var current = this;

                var rem = this;

                while (!current.IsZero)
                {
                    if (groupLength > 0 && actualLength != 0 && actualLength % groupLength == 0)
                        builder.Append(separator ?? "");

                    (current, rem) = current.DivideWithRemainder(numberBase);

                    builder.Append(symbols[(int)rem.value]);

                    actualLength++;
                }
            }

            // Pad with zero symbol
            if (pad && groupLength > 0)
            {
                while (actualLength % groupLength != 0)
                {
                    builder.Append(symbols[0]);

                    actualLength++;
                }
            }

            // Reverse string
            for (int i = 0, j = builder.Length - 1; i < builder.Length / 2; i++, j--)
                (builder[i], builder[j]) = (builder[j], builder[i]);

            return builder.ToString();
        }

        #endregion

        #region Parse

        public static Number Parse(string value, string symbols, bool ignoreInvalid = true)
        {
            var index = TryParseIndex(value, symbols, ignoreInvalid, out var result);

            if (index != value.Length)
                throw new FormatException($"Invalid character {value[index]} at position {index}.");

            return result;
        }

        public static bool TryParse(string value, string symbols, out Number result, bool ignoreInvalid = true)
        {
            return TryParseIndex(value, symbols, ignoreInvalid, out result) != value.Length;
        }

        private static int TryParseIndex(string value, string symbols, bool ignoreInvalid, out Number result)
        {
            var map = new Dictionary<char, Number>();

            Number numberBase = symbols.Length;

            for (int i = 0; i < symbols.Length; i++)
                map.Add(symbols[i], i);

            result = Zero;

            int index = 0;

            for (; index < value.Length; index++)
            {
                if (!map.TryGetValue(value[index], out var digit))
                {
                    if (ignoreInvalid)
                        continue;
                    else
                        break;
                }

                result *= numberBase;

                result += digit;
            }

            return index;
        }

        #endregion
    }
}