using System;
using System.Collections.Generic;
using System.Text;
using Veruthian.Library.Collections.Extensions;

namespace Veruthian.Library.Numeric
{
    public struct Number : INumeric<Number>, ISequential<Number>
    {
        const int BitsInUnit = 32;


        public static readonly Number Default = Zero;

        public static readonly Number Zero = new Number(0);

        public static readonly Number One = new Number(1);


        readonly uint value;

        readonly uint[] values;


        private Number(uint value)
        {
            this.value = value;
            this.values = null;
        }

        private Number(params uint[] values)
        {
            if (values.Length == 0)
            {
                this = new Number();
            }
            else if (values.Length == 1)
            {
                this = new Number(values[0]);
            }
            else
            {
                this.value = default(uint);
                this.values = values;
            }
        }


        public bool IsZero => IsSimple && value == 0;

        private bool IsSimple => values == null;

        private uint this[int index] => values == null ? value : values.Length < index ? values[index] : 0;

        private int UnitCount => values != null ? values.Length : 1;



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
                        if (this.values[i] >= other.values[i])
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
                        if (this.values[i] <= other.values[i])
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
                        var compare = CompareValues(this.values[i], other.values[i]);

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
                        if (this.values[i] != other.values[i])
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
            return IsSimple ? value.GetHashCode() : Utility.HashCodes.Default.Combine(values);
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
        public Number Add(Number other)
        {
            if (this.IsSimple && other.IsSimple)
            {
                ulong result = (ulong)this.value + (ulong)other.value;

                return result;
            }
            else
            {
                var units = new uint[Math.Max(this.UnitCount, other.UnitCount)];

                var carry = ulong.MinValue;

                for (int i = 0; i < units.Length; i++)
                {
                    ulong result = (ulong)this[i] + (ulong)other[i] + carry;

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

            return result.Regular ? result.Value : new Number();
        }

        public Number CheckedSubtract(Number other)
        {
            var result = Subtract(this, other);

            if (!result.Regular)
                throw new InvalidOperationException("Cannot subtract larger number from smaller one");

            return result.Value;
        }

        public Number Delta(Number other)
        {
            var result = Subtract(this, other);

            return result.Value;
        }

        private static (bool Regular, Number Value) Subtract(Number a, Number b)
        {
            if (a.IsSimple && b.IsSimple)
            {
                return a.value >= b.value ? (true, new Number(a.value - b.value)) : (false, new Number(b.value - a.value));
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
                        var result = (ulong)a[i] + ~(ulong)b[i] + carry;

                        units[i] = (uint)result;

                        carry = result >> BitsInUnit;
                    }
                }

                // b > a
                if (carry == 0)                
                    AdjustNegative(units);                

                var adjusted = count;

                for (int i = count; i >= 0; i--)
                {
                    if (units[i] == 0)
                        adjusted--;
                    else
                        break;
                }

                if (adjusted != count)
                    units = units.Resize(adjusted);

                return (carry == 1, new Number(units));
            }
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
            else
            {
                var units = new List<uint>();

                for (var r = 0; r < right.UnitCount; r++)
                {
                    var b = (ulong)right[r];

                    var carry = (ulong)0;

                    for (int l = 0; l < left.UnitCount; l++)
                    {
                        var a = (ulong)left[l];

                        var result = (a * b) + carry;

                        var index = l + r;

                        if (units.Count >= index)
                            units.Add(0);

                        units[index] += (uint)result;

                        carry = result >> BitsInUnit;
                    }

                    if (carry != 0)
                        units.Add((uint)carry);
                }

                return new Number(units.ToArray());
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
                return checkedZero ? (Zero, Zero) : throw new DivideByZeroException();
            }
            else if (right.IsSimple)
            {
                if (left.IsSimple)
                    return (new Number(left.value / right.value), new Number(left.value % right.value));
                else
                    return DivideWithRemainder(left.values, right.value);
            }
            else
            {

            }

            throw new NotImplementedException();
        }

        private static (Number Quotient, Number Remainder) DivideWithRemainder(uint[] numerators, uint denominator)
        {
            var index = numerators.Length - 1;

            var numerator = (ulong)numerators[index--];

            var remainder = numerator % denominator;

            var result = numerator / denominator;


            var units = new uint[index + (result == 0 ? 1 : 2)];

            if (result != 0)
                units[index + 1] = (uint)numerator;

            while (index >= 0)
            {
                numerator = (remainder << BitsInUnit) + numerators[index];

                remainder = numerator % denominator;

                numerator = numerator / denominator;

                units[index--] = (uint)numerator;
            }

            return (new Number(units), new Number((uint)remainder));
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
            Utility.ExceptionHelper.VerifyNotNull(symbols, nameof(symbols));

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

                    builder.Append(symbols[0]);

                    actualLength++;

                    current--;
                }
            }
            else if (this.IsZero)
            {
                builder.Append(symbols[0]);
            }
            else
            {
                var current = this;

                while (!current.IsZero)
                {
                    var rem = current % numberBase;

                    if (groupLength > 0 && actualLength != 0 && actualLength % groupLength == 0)
                        builder.Append(separator ?? "");

                    builder.Append(symbols[(int)rem.value]);

                    actualLength++;

                    current = current / numberBase;
                }
            }


            if (pad && groupLength > 0)
            {
                while (actualLength % groupLength != 0)
                {
                    builder.Append(symbols[0]);

                    actualLength++;
                }
            }

            for (int i = 0, j = builder.Length - 1; i < builder.Length / 2; i++, j--)
            {
                (builder[i], builder[j]) = (builder[j], builder[i]);
            }

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