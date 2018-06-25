using System;
using Veruthian.Dotnet.Library.Data;

namespace Veruthian.Dotnet.Library.Numeric
{
    public struct Number8 : ISequential<Number8>, IBounded<Number8>, IFormattable
    {
        private byte value;

        public Number8(byte value) => this.value = value;

        public static implicit operator Number8(byte value) => new Number8(value);

        public static implicit operator byte(Number8 value) => value.value;


        #region Constants

        public static readonly Number8 Default = default(byte);

        public static readonly Number8 MinValue = byte.MinValue;

        public static readonly Number8 MaxValue = byte.MaxValue;


        Number8 ISequential<Number8>.Next => throw new NotImplementedException();

        Number8 ISequential<Number8>.Previous => throw new NotImplementedException();

        Number8 IBounded<Number8>.Default => Default;

        Number8 IBounded<Number8>.MinValue => MinValue;

        Number8 IBounded<Number8>.MaxValue => MaxValue;

        #endregion

        #region Comparison


        bool IOrderable<Number8>.Precedes(Number8 other)
        {
            throw new NotImplementedException();
        }

        bool IOrderable<Number8>.Follows(Number8 other)
        {
            throw new NotImplementedException();
        }

        bool IEquatable<Number8>.Equals(Number8 other)
        {
            throw new NotImplementedException();
        }

        int IComparable<Number8>.CompareTo(Number8 other)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Operators

        #endregion

        #region Strings

        // ToString
        public override string ToString() => value.ToString();

        public string ToString(string format) => value.ToString(format);

        public string ToString(IFormatProvider provider) => value.ToString(provider);

        public string ToString(string format, IFormatProvider provider) => value.ToString(format, provider);


        // Parsing

        #endregion
    }
}