using System.Text;

namespace Veruthian.Dotnet.Library.Numeric
{
    public struct Number : INumeric<Number>
    {
        // other || values
        readonly uint value;

        readonly uint[] values;


        #region Construction

        private Number(params uint[] values)
        {
            this.value = 0;

            this.values = values;
        }

        public Number(uint value)
        {
            this.value = value;

            this.values = null;
        }

        public Number(int value)
        {
            if (value < 0)
                throw new System.OverflowException("Cannot create Number from negative value.");

            this.value = (uint)value;

            this.values = null;
        }

        public Number(ulong value)
        {
            if (value <= uint.MaxValue)
                this = new Number((uint)value);
            else
                this = new Number((uint)value, (uint)(value >> 32));
        }

        public Number(long value)
        {
            if (value < 0)
                throw new System.OverflowException("Cannot create Number from negative value.");

            if (value <= uint.MaxValue)
                this = new Number((uint)value);
            else
                this = new Number((uint)value, (uint)(value >> 32));
        }

        public static implicit operator Number(uint value) => new Number(value);

        public static explicit operator Number(int value) => new Number(value);

        public static implicit operator Number(ulong value) => new Number(value);

        public static explicit operator Number(long value) => new Number(value);

        #endregion


        private bool IsLight => values == null;

        private bool IsHeavy => values != null;


        #region Comparison

        public override bool Equals(object obj)
        {
            if (obj is Number)
            {
                return this.Equals((Number)obj);
            }
            else
            {
                return false;
            }
        }

        public static bool Equals(Number left, Number right)
        {
            if (left.IsLight)
            {
                if (right.IsLight)
                    return left.value == right.value;
            }
            else
            {
                if (right.IsHeavy)
                {
                    if (left.values.Length == right.values.Length)
                    {
                        for (int i = 0; i < left.values.Length; i++)
                            if (left.values[i] != right.values[i])
                                return false;

                        return true;
                    }
                }
            }

            return false;
        }

        public bool Equals(Number other) => Equals(this, other);

        public static bool operator ==(Number left, Number right) => Equals(left, right);

        public static bool operator !=(Number left, Number right) => !Equals(left, right);


        public override int GetHashCode()
        {
            if (IsLight)
                return value.GetHashCode();
            else
                return HashCodes.Default.Combine(values);
        }

        public int CompareTo(Number other) => Compare(this, other);

        public static int Compare(Number left, Number right)
        {
            if (left.IsLight)
            {
                if (right.IsLight)
                {
                    if (left.value < right.value)
                        return -1;
                    else if (left.value > right.value)
                        return 1;
                    else
                        return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (right.IsLight)
                {
                    return -1;
                }
                else
                {
                    if (left.values.Length < right.values.Length)
                    {
                        return -1;
                    }
                    else if (left.values.Length > right.values.Length)
                    {
                        return 1;
                    }
                    else
                    {
                        for (int i = 0; i < left.values.Length; i++)
                        {
                            if (left.values[i] < right.values[i])
                                return -1;
                            else if (left.values[i] > right.values[i])
                                return 1;
                        }

                        return 0;
                    }
                }
            }
        }

        public bool Precedes(Number other) => CompareTo(other) < 0;

        public bool Follows(Number other) => CompareTo(other) > 0;


        public static bool operator <(Number left, Number right) => Compare(left, right) == -1;

        public static bool operator >(Number left, Number right) => Compare(left, right) == 1;

        public static bool operator <=(Number left, Number right) => Compare(left, right) != 1;

        public static bool operator >=(Number left, Number right) => Compare(left, right) != -1;


        #endregion

        #region Operations

        #region Addition

        public static Number Add(Number left, Number right)
        {
            if (left.IsLight)
            {
                if (right.IsLight)
                {
                    ulong result = (ulong)left.value + (ulong)right.value;

                    return new Number(result);
                }
                else
                {
                    return Add(left.value, right.values);
                }
            }
            else
            {
                if (right.IsLight)
                {
                    return Add(right.value, left.values);
                }
                else
                {
                    return Add(left.values, right.values);
                }
            }
        }

        private static Number Add(uint value, uint[] values)
        {
            int oldSize = values.Length;

            bool overflow = (values[oldSize - 1] == uint.MaxValue);

            var newValues = new uint[overflow ? oldSize : oldSize + 1];

            uint carry = value;

            for (int i = 0; i < oldSize; i++)
                (newValues[i], carry) = NumericUtility.Add(values[i], carry);

            if (overflow)
                newValues[oldSize] = carry;

            return new Number(newValues);
        }

        private static Number Add(uint[] left, uint[] right)
        {
            return new Number();
        }


        public static Number Subtract(Number left, Number right)
        {
            return default(Number);
        }

        public static Number Delta(Number left, Number right)
        {
            return default(Number);
        }

        public Number Add(Number other) => Add(this, other);

        public Number Subtract(Number other) => Subtract(this, other);

        public Number Delta(Number other) => Delta(this, other);

        public Number Increment()
        {
            if (this.IsLight)
            {
                if (this.value != uint.MaxValue)
                {
                    return new Number(this.value + 1);
                }
                else
                {
                    return new Number(0, 1);
                }
            }
            else
            {
                return default(Number);
            }
        }

        public Number Decrement()
        {
            if (this.IsLight)
            {
                if (this.value == 0)
                {
                    throw new System.OverflowException("Operation resulted in underflow.");
                }
                else
                {
                    return new Number(this.value - 1);
                }
            }
            else
            {
                return default(Number);
            }
        }


        public static Number operator +(Number left, Number right) => Add(left, right);

        public static Number operator -(Number left, Number right) => Subtract(left, right);

        public static Number operator ++(Number value) => value.Increment();

        public static Number operator --(Number value) => value.Decrement();

        #endregion

        #region Multiplication

        public Number Multiply(Number other) => throw new System.NotImplementedException();

        public Number Divide(Number other) => throw new System.NotImplementedException();

        public Number Divide(Number other, out Number remainder) => throw new System.NotImplementedException();

        public Number Modulus(Number other) => throw new System.NotImplementedException();

        #endregion

        #endregion

        #region Strings

        public override string ToString()
        {
            if (this.IsLight)
            {
                return value.ToString();
            }
            else
            {
                var builder = new StringBuilder();

                for (int i = values.Length - 1; i >= 0; i--)
                    builder.Append(values[i]);

                return builder.ToString();
            }
        }

        #endregion
    }
}