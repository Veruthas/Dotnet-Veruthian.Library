#define IntUnit

using System;
using System.Collections.Generic;
using System.Text;
using Veruthian.Library.Collections.Extensions;
using Veruthian.Library.Utility;

#if ByteUnit
using Unit = System.Byte;
using TwoUnit = System.UInt16;
#endif

#if ShortUnit
using Unit = System.UInt16;
using TwoUnit = System.UInt32;
#endif

#if IntUnit
using Unit = System.UInt32;
using TwoUnit = System.UInt64;
#endif

namespace Veruthian.Library.Numeric
{
    public struct Number : INumeric<Number>, ISequential<Number>
    {
        readonly TwoUnit value;

        readonly Unit[] units;


        #region Constructors

        private Number(TwoUnit value)
        {
            this.value = value;

            this.units = null;
        }

        private Number(params Unit[] units)
            : this(0, units) { }

        private Number(TwoUnit value, params Unit[] units)
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
            else if (units.Length == 2)
            {
                this.value = (TwoUnit)(units[0] | (units[1] << UnitBits));

                this.units = null;
            }
            else
            {
                this.value = 0;

                this.units = units;
            }
        }

        #endregion

        #region Constants

        const int BitsPerByte = 8;

        const int UnitBytes = sizeof(Unit);

        const int TwoUnitBytes = UnitBytes * 2;

        const int UnitBits = UnitBytes * BitsPerByte;

        const int TwoUnitBits = TwoUnitBytes * BitsPerByte;


        const TwoUnit UpperMask = (Unit.MaxValue) << UnitBits;

        const TwoUnit LowerMask = Unit.MaxValue;


        public static readonly Number Default = Zero;

        public static readonly Number Zero = new Number(0);

        public static readonly Number One = new Number(1);

        #endregion

        #region Properties

        public bool IsZero => IsSimple && value == 0;

        public bool IsOne => value == 1;

        private bool IsSimple => units == null;

        private Unit this[int index] => units == null ? (Unit)(value >> (UnitBits * index) & LowerMask) : (index < units.Length ? units[index] : (Unit)0);

        private int UnitCount => units != null ? units.Length : (value & UpperMask) == 0 ? 1 : 2;

        private Unit UpperValue => (Unit)(value >> UnitBits);

        private Unit LowerValue => (Unit)value;

        private bool IsUnit => (value & UpperMask) == 0;

        #endregion

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
                    return CompareValues(this.value, other.value);
                else
                    return -1;
            }
            else
            {
                if (this.UnitCount < other.UnitCount)
                {
                    return -1;
                }
                else if (this.UnitCount > other.UnitCount)
                {
                    return 1;
                }
                else
                {
                    for (int i = this.UnitCount - 1; i >= 0; i--)
                    {
                        var compare = CompareValues(this.units[i], other.units[i]);

                        if (compare != 0)
                            return compare;
                    }

                    return 0;
                }
            }
        }

        private static int CompareValues(Unit left, Unit right)
        {
            if (left == right)
                return 0;
            else if (left < right)
                return -1;
            else
                return 1;
        }

        private static int CompareValues(TwoUnit left, TwoUnit right)
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
            else if (this.UnitCount != other.UnitCount)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < this.UnitCount; i++)
                    if (this.units[i] != other.units[i])
                        return false;

                return true;
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


        public static Number Max(Number a, Number b) => a > b ? a : b;

        public static Number Min(Number a, Number b) => a < b ? a : b;


        public static bool operator ==(Number left, Number right) => left.Equals(right);

        public static bool operator !=(Number left, Number right) => !left.Equals(right);

        public static bool operator <(Number left, Number right) => left.Precedes(right);

        public static bool operator >(Number left, Number right) => left.Follows(right);

        public static bool operator <=(Number left, Number right) => left.CompareTo(right) <= 0;

        public static bool operator >=(Number left, Number right) => left.CompareTo(right) >= 0;

        #endregion

        #region Helpers

        private static void TrimUnits(ref Unit[] units)
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

        #endregion

        #region Arithmetic

        // Adjustment
        private static void AdjustNegative(Unit[] units)
        {
            var carry = (TwoUnit)1;

            for (int i = 0; i < units.Length; i++)
            {
                unchecked
                {
                    TwoUnit inverse = (TwoUnit)(((Unit)~units[i]) + carry);

                    carry = (TwoUnit)(inverse >> UnitBits);

                    units[i] = (Unit)inverse;
                }
            }
        }


        // Addition
        public Number Sum(Number addend) => Sum(this, addend);

        private static Number Sum(Number left, Number right)
        {
            if (left.IsSimple && right.IsSimple)
            {
                var upperLeft = left.UpperValue;

                var upperRight = right.UpperValue;

                if (upperLeft == 0 && upperRight == 0)
                {
                    return new Number((TwoUnit)(left.value + right.value));
                }
                else
                {
                    var lowerLeft = left.value & LowerMask;

                    var lowerRight = right.value & LowerMask;

                    var lower = lowerLeft + lowerRight;

                    var carry = lower >> UnitBits;

                    var upper = upperLeft + upperRight + carry;

                    if ((upper & UpperMask) == 0)
                        return new Number((TwoUnit)((lower & LowerMask) + (upper << UnitBits)));
                    else
                        return new Number((Unit)lower, (Unit)upper, (Unit)(upper >> UnitBits));
                }
            }
            else
            {
                var units = new Unit[Math.Max(left.UnitCount, right.UnitCount)];

                var carry = new TwoUnit();

                for (int i = 0; i < units.Length; i++)
                {
                    var result = (TwoUnit)left[i] + (TwoUnit)right[i] + carry;

                    units[i] = (Unit)result;

                    carry = (TwoUnit)(result >> UnitBits);
                }

                if (carry != 0)
                    units = units.Appended((Unit)carry);

                return new Number(units);
            }
        }

        public Number Next => this.Sum(One);


        // Subtraction
        public Number Difference(Number subtrahend)
        {
            var result = Difference(this, subtrahend);

            if (!result.Positive)
                return Zero;

            if (result.Units != null)
                TrimUnits(ref result.Units);

            return new Number(result.Value, result.Units);
        }

        public Number CheckedDifference(Number subtrahend)
        {
            var result = Difference(this, subtrahend);

            if (!result.Positive)
                throw new InvalidOperationException("Cannot subtract larger number from smaller one");

            if (result.Units != null)
                TrimUnits(ref result.Units);

            return new Number(result.Value, result.Units);
        }

        public Number Delta(Number subtrahend)
        {
            var result = Delta(this, subtrahend);

            return result.Value;
        }

        private static (Number Value, bool Positive) Delta(Number a, Number b)
        {
            var result = Difference(a, b);

            if (result.Units != null)
            {
                if (!result.Positive)
                    AdjustNegative(result.Units);

                TrimUnits(ref result.Units);
            }

            return (new Number(result.Value, result.Units), result.Positive);
        }


        private static (bool Positive, TwoUnit Value, Unit[] Units) Difference(Number a, Number b)
        {
            if (a.IsSimple && b.IsSimple)
            {
                if (a.value >= b.value)
                    return (true, (TwoUnit)(a.value - b.value), null);
                else
                    return (false, (TwoUnit)(b.value - a.value), null);
            }
            else
            {
                var count = Math.Max(a.UnitCount, b.UnitCount);

                var units = new Unit[count];

                var carry = (TwoUnit)1;

                for (var i = 0; i < count; i++)
                {
                    unchecked
                    {
                        var result = (TwoUnit)a[i] + (TwoUnit)((Unit)(~b[i])) + carry;

                        units[i] = (Unit)result;

                        carry = (TwoUnit)(result >> UnitBits);
                    }
                }

                return (carry == 1, 0, units);
            }
        }

        public Number Previous => this.Difference(One);


        // Multiplication
        public Number Product(Number multiplicand) => Product(this, multiplicand);

        private static Number Product(Number left, Number right)
        {
            if (left.IsSimple && right.IsSimple && ((left.value | right.value) & UpperMask) == 0)
            {
                return new Number((TwoUnit)(left.value * right.value));
            }
            else if (left.IsZero || right.IsZero)
            {
                return Zero;
            }
            else if (left.IsOne)
            {
                return right;
            }
            else if (right.IsOne)
            {
                return left;
            }
            else
            {
                var units = new Unit[left.UnitCount + right.UnitCount];

                for (int b = 0, column = 0; b < right.UnitCount; b++, column++)
                {
                    var value = new ulong();

                    for (var a = 0; a < left.UnitCount; a++)
                    {
                        // add (A*B) + previous results + carry
                        value = ((ulong)left[a] * (ulong)right[b]) + (ulong)units[column + a] + (ulong)value;

                        units[column + a] = (Unit)value;

                        value >>= UnitBits;
                    }

                    if (value != 0)
                        units[column + left.UnitCount] = (Unit)value;
                }

                TrimUnits(ref units);

                return new Number(units);
            }
        }


        // Power
        public Number Power(Number exponent)
        {
            if (exponent.IsSimple)
                return Power(this, exponent.value);
            else
                return Power(this, exponent);
        }

        private static Number Power(Number value, TwoUnit exponent)
        {
            var result = One;

            var last = value;

            while (exponent != 0)
            {
                if ((exponent & 0x1) == 1)
                    result *= last;

                exponent >>= 1;

                last *= last;
            }

            return result;
        }

        private static Number Power(Number value, Number exponent)
        {
            var result = One;

            var last = value;

            var bitIndex = 0;

            var unitIndex = 0;

            var exponentUnit = exponent.units[unitIndex];

            while (true)
            {
                if (unitIndex == exponent.UnitCount - 1)
                {
                    if (exponentUnit == 0)
                        break;
                }

                if (bitIndex == UnitBits)
                {
                    unitIndex++;

                    bitIndex = 0;

                    exponentUnit = exponent.units[unitIndex];
                }

                if ((exponentUnit & 0x1) == 1)
                    result *= last;

                exponentUnit >>= 1;

                bitIndex++;

                last *= last;
            }

            return result;
        }

        // Division
        public Number Quotient(Number divisor) => Division(divisor, false).Quotient;

        public Number Remainder(Number divisor) => Division(divisor, false).Remainder;

        public Number CheckedQuotient(Number divisor) => Division(divisor, true).Quotient;

        public Number CheckedRemainder(Number divisor) => Division(divisor, true).Remainder;


        public (Number Quotient, Number Remainder) Division(Number divisor) => Division(divisor, false);

        public (Number Quotient, Number Remainder) CheckedDivision(Number divisor) => Division(divisor, true);


        private (Number Quotient, Number Remainder) Division(Number divisor, bool checkedDivisor) => Division(this, divisor, checkedDivisor);


        private static (Number Quotient, Number Remainder) Division(Number left, Number right, bool checkedDivisor)
        {
            if (right.IsZero)
            {
                return checkedDivisor ? throw new DivideByZeroException() : (Zero, Zero);
            }
            else if (right.IsSimple && right.IsUnit)
            {
                if (left.IsSimple)
                    return (new Number((TwoUnit)(left.value / right.value)), new Number((TwoUnit)(left.value % right.value)));
                else
                    return Division(left, (Unit)right.value);
            }
            else
            {
                return Division(left, right);
            }
        }

        private static (Number Quotient, Number Remainder) Division(Number numerators, Unit denominator, int offset = 0)
        {
            var index = numerators.UnitCount - 1;

            var numerator = (ulong)numerators[index];


            var remainder = numerator % denominator;

            numerator = numerator / denominator;


            var units = new Unit[index + (numerator == 0 ? 0 : 1) - offset];

            if (numerator != 0)
                units[index - offset] = (Unit)numerator;

            index--;

            while (index >= offset)
            {
                numerator = (remainder << UnitBits) + numerators[index];

                remainder = numerator % denominator;

                numerator = numerator / denominator;

                units[index - offset] = (Unit)numerator;

                index--;
            }

            return (new Number(units), new Number((Unit)remainder));
        }

        private static (Number Quotient, Number Remainder) Division(Number numerator, Number denominator)
        {
            if (numerator < denominator)
            {
                return (Zero, numerator);
            }
            else
            {
                var offset = denominator.UnitCount - 1;

                var simpleDenominator = denominator[offset];


                var original = numerator;


                var result = Division(numerator, simpleDenominator, offset);

                var quotient = result.Quotient;


                var estimated = result.Quotient * denominator;

                var error = Delta(numerator, estimated);

                var positive = error.Positive;


                while (error.Value >= denominator)
                {
                    result = Division(error.Value, simpleDenominator, offset);

                    if (positive)
                        quotient += result.Quotient;
                    else
                        quotient -= result.Quotient;

                    estimated = quotient * denominator;

                    error = Delta(numerator, estimated);

                    positive = error.Positive;
                }

                return (quotient, error.Value);
            }
        }


        #endregion

        #region Operators

        public static Number operator ++(Number value) => value.Next;

        public static Number operator --(Number value) => value.Previous;


        public static Number operator +(Number left, Number right) => left.Sum(right);

        public static Number operator -(Number left, Number right) => left.Difference(right);

        public static Number operator *(Number left, Number right) => left.Product(right);

        public static Number operator /(Number left, Number right) => left.Quotient(right);

        public static Number operator %(Number left, Number right) => left.Remainder(right);

        public static Number operator ^(Number left, Number right) => left.Power(right);

        #endregion

        #region Conversion

        private static Number FromByte(byte value)
            => new Number((TwoUnit)value);

        private static Number FromShort(ushort value)
            => new Number((TwoUnit)value);

        private static Number FromInt(uint value)
        {
#if ByteUnit
            if ((value & 0xFF_FF_FF_00) == 0)
                return new Number((byte)value);
            else if ((value & 0xFF_FF_00_00) == 0)
                return new Number((ushort)value);
            else if ((value & 0xFF_00_00_00) == 0)
                return new Number(
                    (Unit)(value & 0xFF),
                    (Unit)((value >> 8) & 0xFF),
                    (Unit)((value >> 16) & 0xFF)
                );
            else
                return new Number(
                    (Unit)(value & 0xFF),
                    (Unit)((value >> 8) & 0xFF),
                    (Unit)((value >> 16) & 0xFF),
                    (Unit)((value >> 24) & 0xFF)
                );
#else
            return new Number(value);
#endif
        }

        private static Number FromLong(ulong value)
        {
#if ByteUnit
            if ((value & 0xFF_FF_FF_FF_FF_FF_FF_00) == 0)
                return new Number((byte)value);
            else if ((value & 0xFF_FF_FF_FF_FF_FF_00_00) == 0)
                return new Number((ushort)value);
            else if ((value & 0xFF_FF_FF_FF_FF_00_00_00) == 0)
                return new Number(
                    (Unit)(value & 0xFF),
                    (Unit)((value >> 8) & 0xFF),
                    (Unit)((value >> 16) & 0xFF)
                );
            else if ((value & 0xFF_FF_FF_FF_00_00_00_00) == 0)
                return new Number(
                    (Unit)(value & 0xFF),
                    (Unit)((value >> 8) & 0xFF),
                    (Unit)((value >> 16) & 0xFF),
                    (Unit)((value >> 24) & 0xFF)
                );
            else if ((value & 0xFF_FF_FF_00_00_00_00_00) == 0)
                return new Number(
                    (Unit)(value & 0xFF),
                    (Unit)((value >> 8) & 0xFF),
                    (Unit)((value >> 16) & 0xFF),
                    (Unit)((value >> 24) & 0xFF),
                    (Unit)((value >> 32) & 0xFF)
                );
            else if ((value & 0xFF_FF_00_00_00_00_00_00) == 0)
                return new Number(
                    (Unit)(value & 0xFF),
                    (Unit)((value >> 8) & 0xFF),
                    (Unit)((value >> 16) & 0xFF),
                    (Unit)((value >> 24) & 0xFF),
                    (Unit)((value >> 32) & 0xFF),
                    (Unit)((value >> 40) & 0xFF)
                );
            else if ((value & 0xFF_00_00_00_00_00_00_00) == 0)
                return new Number(
                    (Unit)(value & 0xFF),
                    (Unit)((value >> 8) & 0xFF),
                    (Unit)((value >> 16) & 0xFF),
                    (Unit)((value >> 24) & 0xFF),
                    (Unit)((value >> 32) & 0xFF),
                    (Unit)((value >> 40) & 0xFF),
                    (Unit)((value >> 48) & 0xFF)
                );
            else
                return new Number(
                    (Unit)(value & 0xFF),
                    (Unit)((value >> 8) & 0xFF),
                    (Unit)((value >> 16) & 0xFF),
                    (Unit)((value >> 24) & 0xFF),
                    (Unit)((value >> 32) & 0xFF),
                    (Unit)((value >> 40) & 0xFF),
                    (Unit)((value >> 48) & 0xFF),
                    (Unit)((value >> 56) & 0xFF)
                );
#elif ShortUnit
            if ((value & 0xFF_FF_FF_FF_FF_FF_FF_00) == 0)
                return new Number((byte)value);
            else if ((value & 0xFF_FF_FF_FF_FF_FF_00_00) == 0)
                return new Number((ushort)value);
            else if ((value & 0xFF_FF_FF_FF_00_00_00_00) == 0)
                return new Number((uint)value);
            else if ((value ^ 0xFF_FF_00_00_00_00_00_00) == 0)
                return new Number(
                    (Unit)(value & 0xFFFF),
                    (Unit)((value >> 16) & 0xFFFF),
                    (Unit)((value >> 32) & 0xFFFF)
                );
            else
                return new Number(
                    (Unit)(value & 0xFFFF),
                    (Unit)((value >> 16) & 0xFFFF),
                    (Unit)((value >> 32) & 0xFFFF),
                    (Unit)((value >> 48) & 0xFFFF),
                );
#else
            return new Number(value);
#endif
        }


        public static implicit operator Number(byte value) => FromByte(value);

        public static implicit operator Number(ushort value) => FromShort(value);

        public static implicit operator Number(uint value) => FromInt(value);

        public static implicit operator Number(ulong value) => FromLong(value);


        public static implicit operator Number(sbyte value) => FromByte((byte)Math.Abs(value));

        public static implicit operator Number(short value) => FromShort((ushort)Math.Abs(value));

        public static implicit operator Number(int value) => FromInt((uint)Math.Abs(value));

        public static implicit operator Number(long value) => FromLong((ulong)Math.Abs(value));



        public byte ToByte()
        {
            return (byte)value;
        }

        public ushort ToShort()
        {
            return (ushort)value;
        }

        public uint ToInt()
        {
#if ByteUnit
            return (uint)units[0]
                  | ((uint)units[1] << 8)
                  | ((uint)units[2] << 16)
                  | ((uint)units[3] << 24)
                  ;
#elif ShortUnit
            return (uint)units[0]
                  | ((uint)units[1] << 16)
                  ;
#else
            return (uint)value;
#endif
        }

        public ulong ToLong()
        {
#if ByteUnit
            return (ulong)units[0]
                  | ((ulong)units[1] << 8)
                  | ((ulong)units[2] << 16)
                  | ((ulong)units[3] << 24)
                  | ((ulong)units[4] << 32)
                  | ((ulong)units[5] << 40)
                  | ((ulong)units[6] << 48)
                  | ((ulong)units[7] << 56)
                  ;
#elif ShortUnit
            return (ulong)units[0]
                  | ((ulong)units[1] << 16)
                  | ((ulong)units[2] << 32)
                  | ((ulong)units[3] << 48)
                  ;                  
#else
            return (ulong)value;
#endif
        }


        public sbyte ToSignedByte() => (sbyte)(ToByte() & 0x7F);

        public short ToSignedShort() => (short)(ToShort() & 0x7FFF);

        public int ToSignedInt() => (int)(ToInt() & 0x7FFF_FFFF);

        public long ToSignedLong() => (long)(ToLong() & 0x7FFF_FFFF_FFFF_FFFF);


        public byte ToCheckedByte()
        {
            if (!IsSimple || value > byte.MaxValue)
                throw new InvalidCastException($"Number is larger than Byte Maximum {byte.MaxValue}");

            return ToByte();
        }

        public ushort ToCheckedShort()
        {
#if ByteUnit
            if (!IsSimple)
#else
            if (!IsSimple || value > ushort.MaxValue)
#endif
                throw new InvalidCastException($"Number is larger than short Maximum {ushort.MaxValue}");

            return ToShort();
        }

        public uint ToCheckedInt()
        {
#if ByteUnit
            if (!IsSimple || units.Length > 4)
#elif ShortUnit
            if (!IsSimple)
#else
            if (!IsSimple || value > uint.MaxValue)
#endif
                throw new InvalidCastException($"Number is larger than Int Maximum {uint.MaxValue}");

            return ToShort();
        }

        public ulong ToCheckedLong()
        {
#if ByteUnit
            if (!IsSimple || units.Length > 8)
#elif ShortUnit
            if (!IsSimple || units.Length > 4)
#else
            if (!IsSimple)
#endif
                throw new InvalidCastException($"Number is larger than Long Maximum {ulong.MaxValue}");

            return ToShort();
        }


        public sbyte ToCheckedSignedByte()
        {
            if (!IsSimple || value > (byte)sbyte.MaxValue)
                throw new InvalidCastException($"Number is larger than Byte Maximum {byte.MaxValue}");

            return ToSignedByte();
        }

        public short ToCheckedSignedShort()
        {
#if ByteUnit
            if (!IsSimple)
#else
            if (!IsSimple || value > (ushort)short.MaxValue)
#endif
                throw new InvalidCastException($"Number is larger than short Maximum {short.MaxValue}");

            return ToSignedShort();
        }

        public int ToCheckedSignedInt()
        {
#if ByteUnit
            if (!IsSimple || units.Length > 4 || units[UnitCount - 1] > 0x7F)
#else
            if (!IsSimple || value > (uint)int.MaxValue)
#endif
                throw new InvalidCastException($"Number is larger than Int Maximum {int.MaxValue}");

            return ToSignedInt();
        }

        public long ToCheckedSignedLong()
        {
#if ByteUnit
            if (!IsSimple || units.Length > 8 || units[UnitCount - 1] > 0x7F)
#elif ShortUnit
            if (!IsSimple || units.Length > 4 || units[UnitCount - 1] > 0x7F)
#else
            if (!IsSimple || value > (ulong)long.MaxValue)
#endif
                throw new InvalidCastException($"Number is larger than Long Maximum {long.MaxValue}");

            return ToSignedLong();
        }


        public static explicit operator byte(Number value) => value.ToByte();

        public static explicit operator ushort(Number value) => value.ToShort();

        public static explicit operator uint(Number value) => value.ToInt();

        public static explicit operator ulong(Number value) => value.ToLong();


        public static explicit operator sbyte(Number value) => value.ToSignedByte();

        public static explicit operator short(Number value) => value.ToSignedShort();

        public static explicit operator int(Number value) => value.ToSignedInt();

        public static explicit operator long(Number value) => value.ToSignedLong();

        #endregion

        #region ToString

        public static readonly string Decimal = "0123456789";
        public static readonly string Binary = "01";
        public static readonly string Octal = "01234567";
        public static readonly string HexUpper = "0123456789ABCDEF";
        public static readonly string HexLower = "0123456789abcdef";


        public override string ToString() => ToDecimalString(3, ",", false); // ToHexString(true, 4, "_", true);

        public string ToBinaryString(int groupLength = 0, string separator = null, bool pad = false)
            => ToString(Binary, groupLength, separator, pad);

        public string ToOctalString(int groupLength = 0, string separator = null, bool pad = false)
            => ToString(Octal, groupLength, separator, pad);

        public string ToDecimalString(int groupLength = 0, string separator = null, bool pad = false)
            => ToString(Decimal, groupLength, separator, pad);

        public string ToHexString(bool upper = true, int groupLength = 0, string separator = null, bool pad = false)
            => ToString(upper ? HexUpper : HexLower, groupLength, separator, pad);

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

                    (current, rem) = current.Division(numberBase);

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

        const int BinaryBase = 2;

        const int OctalBase = 8;

        const int DecimalBase = 10;

        const int HexBase = 16;


        static readonly Dictionary<char, Number> BinaryNumeralMap = new Dictionary<char, Number>
        {
            ['0'] = 0,
            ['1'] = 1
        };

        static readonly Dictionary<char, Number> OctalNumeralMap = new Dictionary<char, Number>
        {
            ['0'] = 0,
            ['1'] = 1,
            ['2'] = 2,
            ['3'] = 3,
            ['4'] = 4,
            ['5'] = 5,
            ['6'] = 6,
            ['7'] = 7,
        };

        static readonly Dictionary<char, Number> DecimalNumeralMap = new Dictionary<char, Number>
        {
            ['0'] = 0,
            ['1'] = 1,
            ['2'] = 2,
            ['3'] = 3,
            ['4'] = 4,
            ['5'] = 5,
            ['6'] = 6,
            ['7'] = 7,
            ['8'] = 8,
            ['9'] = 9,
        };

        static readonly Dictionary<char, Number> HexNumeralMap = new Dictionary<char, Number>
        {
            ['0'] = 0,
            ['1'] = 1,
            ['2'] = 2,
            ['3'] = 3,
            ['4'] = 4,
            ['5'] = 5,
            ['6'] = 6,
            ['7'] = 7,
            ['8'] = 8,
            ['9'] = 9,
            ['A'] = 10,
            ['B'] = 11,
            ['C'] = 12,
            ['D'] = 13,
            ['E'] = 14,
            ['F'] = 15,
            ['a'] = 10,
            ['b'] = 11,
            ['c'] = 12,
            ['d'] = 13,
            ['e'] = 14,
            ['f'] = 15,
        };


        public static Number ParseBinary(string value, bool ignoreInvalid = true)
            => Parse(value, BinaryNumeralMap, BinaryBase, ignoreInvalid);

        public static bool TryParseBinary(string value, out Number result, bool ignoreInvalid = true)
            => TryParse(value, BinaryNumeralMap, BinaryBase, out result, ignoreInvalid);


        public static Number ParseOctal(string value, bool ignoreInvalid = true)
            => Parse(value, OctalNumeralMap, OctalBase, ignoreInvalid);

        public static bool TryParseOctal(string value, out Number result, bool ignoreInvalid = true)
            => TryParse(value, OctalNumeralMap, OctalBase, out result, ignoreInvalid);


        public static Number ParseDecimal(string value, bool ignoreInvalid = true)
            => Parse(value, DecimalNumeralMap, DecimalBase, ignoreInvalid);

        public static bool TryParseDecimal(string value, out Number result, bool ignoreInvalid = true)
            => TryParse(value, DecimalNumeralMap, DecimalBase, out result, ignoreInvalid);


        public static Number ParseHex(string value, bool ignoreInvalid = true)
            => Parse(value, HexNumeralMap, HexBase, ignoreInvalid);

        public static bool TryParseHex(string value, out Number result, bool ignoreInvalid = true)
            => TryParse(value, HexNumeralMap, HexBase, out result, ignoreInvalid);


        public static Number Parse<T>(IEnumerable<T> value, IEnumerable<T> symbols, bool ignoreInvalid = true)
        {
            var error = TryParseIndex(value, symbols, ignoreInvalid, out var result);

            if (error.Position != -1)
                throw new FormatException($"Invalid character {error.Value} at position {error.Position}.");

            return result;
        }

        public static Number Parse<T>(IEnumerable<T> value, Dictionary<T, Number> map, Number numberBase, bool ignoreInvalid = true)
        {
            var error = TryParseIndex(value, map, numberBase, ignoreInvalid, out var result);

            if (error.Position != -1)
                throw new FormatException($"Invalid character {error.Value} at position {error.Position}.");

            return result;
        }


        public static bool TryParse<T>(IEnumerable<T> value, IEnumerable<T> symbols, out Number result, bool ignoreInvalid = true)
            => TryParseIndex(value, symbols, ignoreInvalid, out result).Position != -1;

        public static bool TryParse<T>(IEnumerable<T> value, Dictionary<T, Number> symbolMap, Number numberBase, out Number result, bool ignoreInvalid = true)
             => TryParseIndex(value, symbolMap, numberBase, ignoreInvalid, out result).Position != -1;


        private static (int Position, T Value) TryParseIndex<T>(IEnumerable<T> value, IEnumerable<T> symbols, bool ignoreInvalid, out Number result)
        {
            ExceptionHelper.VerifyNotNull(symbols, nameof(symbols));

            var map = new Dictionary<T, Number>();

            int numberBase = 0;

            foreach (var symbol in symbols)
                map.Add(symbol, numberBase++);

            return TryParseIndex(value, map, numberBase, ignoreInvalid, out result);
        }

        private static (int Position, T Value) TryParseIndex<T>(IEnumerable<T> value, Dictionary<T, Number> symbolMap, Number numberBase, bool ignoreInvalid, out Number result)
        {
            ExceptionHelper.VerifyNotNull(symbolMap, nameof(symbolMap));

            result = Zero;

            var index = 0;

            foreach (var numeral in value)
            {
                index++;

                if (!symbolMap.TryGetValue(numeral, out var digit))
                {
                    if (ignoreInvalid)
                        continue;
                    else
                        return (index, numeral);
                }

                result *= numberBase;

                result += digit;
            }

            return (-1, default(T));
        }

        #endregion

        #region Bytes

        public static Number ByteCount(Number value)
        {
            if (value.IsSimple)
            {
                var result = 0;

                var units = value.value;

                do
                {
                    result++;

                    units >>= 8;

                } while (units != 0);

                return result;
            }
            else
            {
                var result = (value.units.Length - 1) * UnitBytes;

                var unit = value.units[value.units.Length - 1];

                while (unit != 0)
                {
                    result++;

                    unit >>= 8;
                }

                return result;
            }
        }

        // To
        public static IEnumerable<byte> ToBytes(Number value)
        {
            if (value.IsZero)
            {
                yield return 0;
            }
            else if (value.IsSimple)
            {
                var units = value.value;

                for (int i = 0; i < TwoUnitBytes; i++)
                {
                    var chunk = (byte)(units & 0xFF);

                    yield return chunk;

                    units >>= 8;

                    if (units == 0)
                        break;
                }
            }
            else
            {
                for (var i = 0; i < value.units.Length - 1; i++)
                {
                    var unit = value.units[i];

                    for (int j = 0; j < UnitBytes; j++)
                    {
                        var chunk = (byte)(unit & 0xFF);

                        yield return chunk;

                        unit >>= 8;
                    }
                }

                // Only return bytes on highest unit when unit is non zero
                {
                    var unit = value.units[value.units.Length - 1];

                    for (var j = 0; j < UnitBytes; j++)
                    {
                        var chunk = (byte)(unit & 0xFF);

                        yield return chunk;

                        unit >>= 8;

                        if (unit == 0)
                            break;
                    }
                }
            }
        }

        public static IEnumerable<byte> ToBytes(Number value, Number step)
        {
            if (value.IsSimple)
            {
                var units = value.value;

                for (var i = Zero; i < step; i++)
                {
                    var chunk = (byte)(units & 0xFF);

                    yield return chunk;

                    units >>= 8;
                }
            }
            else
            {
                var units = value.units;

                var sent = step;

                for (var i = 0; i < units.Length; i++)
                {
                    var unit = units[units.Length - 1];

                    for (var j = 0; j < UnitBytes; j++)
                    {
                        if (sent.IsZero)
                            yield break;

                        var chunk = (byte)(unit & 0xFF);

                        yield return chunk;

                        unit >>= 8;

                        sent--;
                    }
                }
            }
        }

        public static IEnumerable<byte> ToBytes(IEnumerable<Number> values, Number step)
        {
            foreach (var value in values)
            {
                if (value.IsSimple)
                {
                    var units = value.value;

                    for (var i = Zero; i < step; i++)
                    {
                        var chunk = (byte)(units & 0xFF);

                        yield return chunk;

                        units >>= 8;
                    }
                }
                else
                {
                    var units = value.units;

                    var current = step;

                    for (var i = 0; i < units.Length; i++)
                    {
                        var unit = units[units.Length - 1];

                        for (var j = 0; j < UnitBytes; j++)
                        {
                            if (current.IsZero)
                                yield break;

                            var chunk = (byte)(unit & 0xFF);

                            yield return chunk;

                            unit >>= 8;

                            current--;
                        }
                    }
                }
            }

        }


        // From
        private static (Unit Value, bool Done) GetUnit(IEnumerator<byte> enumerator)
        {
            var unit = new Unit();

            for (int i = 0; i < UnitBytes; i++)
            {
                unit |= (Unit)(enumerator.Current << (8 * i));

                if (!enumerator.MoveNext())
                    return (unit, true);
            }

            return (unit, false);
        }

        private static (Number Value, bool Done) GetNumber(IEnumerator<byte> items, Number step)
        {
            if (step <= TwoUnitBytes)
            {
                var value = new TwoUnit();

                var done = false;

                for (int i = 0; i < step.ToInt(); i++)
                {
                    value |= (Unit)(items.Current << (8 * i));

                    done = items.MoveNext();

                    if (done)
                        break;
                }

                return (new Number(value), done);
            }
            else
            {
                var checkedStep = step.ToCheckedInt();

                var unitRemainder = checkedStep % UnitBytes;

                var unitCount = (checkedStep / UnitBytes) + (unitRemainder == 0 ? 0 : 1);


                var units = new Unit[unitCount];

                var done = false;

                for (int i = 0; i < unitCount; i++)
                {
                    var unit = new Unit();

                    var bytes = i == unitCount - 1 ? unitRemainder : UnitBytes;

                    for (int j = 0; j < bytes; j++)
                    {
                        unit |= (Unit)(items.Current << (j * 8));

                        done = items.MoveNext();

                        if (done)
                            break;
                    }

                    units[i] = unit;

                    if (done)
                        break;
                }

                TrimUnits(ref units);

                return (new Number(units), done);
            }
        }


        public static Number FromBytes(IEnumerable<byte> items)
        {
            var enumerator = items.GetEnumerator();

            if (!enumerator.MoveNext())
                return Zero;

            var twoUnits = new TwoUnit();

            for (int i = 0; i < TwoUnitBytes; i++)
            {
                twoUnits |= (Unit)(enumerator.Current << (8 * i));

                if (!enumerator.MoveNext())
                    return new Number(twoUnits);
            }

            var units = new Unit[4];

            units[0] = (Unit)(twoUnits);

            units[1] = (Unit)(twoUnits >> UnitBits);

            var index = 2;

            while (true)
            {
                var result = GetUnit(enumerator);

                if (index == units.Length)
                    units.Resize(result.Done ? index + 1 : index * 2);

                units[index++] = result.Value;

                if (result.Done)
                    break;
            }

            TrimUnits(ref units);

            return new Number(units);
        }

        public static Number FromBytes(IEnumerator<byte> items, Number step)
        {
            return GetNumber(items, step).Value;
        }

        public static IEnumerable<Number> FromBytes(IEnumerable<byte> items, Number step)
        {
            var enumerator = items.GetEnumerator();

            enumerator.MoveNext();

            var result = GetNumber(enumerator, step);

            yield return result.Value;

            while (!result.Done)
            {
                result = GetNumber(enumerator, step);

                yield return result.Value;
            }
        }

        #endregion
    }
}