using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Veruthian.Dotnet.Library.Data;
using Veruthian.Dotnet.Library.Data.Collections;

namespace Veruthian.Dotnet.Library.Text.Code
{
    public class CodeString : IEquatable<CodeString>, IComparable<CodeString>, IEnumerable<CodePoint>
    {
        readonly int hashcode;

        readonly CodePoint[] codepoints;


        #region Constructors

        private CodeString(CodePoint[] codepoints, bool clone)
        {
            this.hashcode = HashCodes.Default.Combine(codepoints);

            if (clone)
                this.codepoints = (CodePoint[])codepoints.Clone();
            else
                this.codepoints = codepoints;
        }

        public CodeString(CodePoint codepoint)
            : this(new[] { codepoint }, false) { }

        public CodeString(params CodePoint[] codepoints)
            : this(codepoints, true) { }


        public CodeString(IEnumerable<CodePoint> codepoints)
            : this(System.Linq.Enumerable.ToArray(codepoints), false) { }

        private static CodePoint[] GetFromEnumerator(IEnumerable<CodePoint> codepoints)
        {
            var result = new List<CodePoint>();

            foreach(var codepoint in codepoints)
                result.Add(codepoint);

            return result.ToArray();
        }

        public CodeString(ICollection<CodePoint> codepoints)
                : this(GetFromCollection(codepoints), false) { }
        public CodeString(IList<CodePoint> codepoints, int index)
            : this(GetFromList(codepoints, index, codepoints.Count - index), false) { }
        public CodeString(IList<CodePoint> codepoints, int index, int length)
            : this(GetFromList(codepoints, index, length), false) { }
        private static CodePoint[] GetFromCollection(ICollection<CodePoint> codepoints)
        {
            if (codepoints == null)
                throw new ArgumentNullException("codepoints");

            CodePoint[] values = new CodePoint[codepoints.Count];

            codepoints.CopyTo(values, 0);

            return values;
        }
        private static CodePoint[] GetFromList(IList<CodePoint> codepoints, int index, int length)
        {
            if (codepoints == null)
                throw new ArgumentNullException("codepoints");
            if (index < 0 || index > codepoints.Count)
                throw new ArgumentOutOfRangeException("index");
            if (length < 0 || index + length > codepoints.Count)
                throw new ArgumentOutOfRangeException("length");

            CodePoint[] values = new CodePoint[length];

            if (codepoints is List<CodePoint>)
            {
                var list = codepoints as List<CodePoint>;
                list.CopyTo(index, values, 0, length);
            }
            else
            {
                for (int i = 0; i < length; i++)
                    values[i] = codepoints[index + i];
            }

            return values;
        }

        public CodeString(CodePoint value, int count)
            : this(ReplicateCodePoint(value, count), false) { }

        public CodeString(string value)
            : this(value.ToCodePointArray(), false) { }

        public CodeString(string value, int start)
            : this(value.ToCodePointArray(start), false) { }

        public CodeString(string value, int start, int length)
            : this(value.ToCodePointArray(start, length), false) { }


        #endregion

        
        public CodePoint this[int index] => codepoints[index];

        public int Length => codepoints.Length;

        public bool IsEmpty => Length == 0;


        #region Operations

        #region Validation

        public bool IsValid
        {
            get
            {
                foreach (var codepoint in codepoints)
                    if (codepoint.IsInvalid)
                        return false;

                return true;
            }
        }

        public void Validate()
        {
            foreach (var codepoint in codepoints)
                codepoint.VerifyIsValid();
        }

        #endregion

        #region Equality

        public override int GetHashCode() => hashcode;

        public override bool Equals(object obj) => (obj is CodeString) ? Equals(obj as CodeString) : false;


        public bool Equals(CodeString other)
        {
            if (other.IsNull())
            {
                return false;
            }
            else if (base.Equals(other))
            {
                return true;
            }
            else if (this.hashcode == other.hashcode && this.Length == other.Length)
            {
                for (int i = 0; i < codepoints.Length; i++)
                {
                    if (codepoints[i] != other.codepoints[i])
                        return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsEqualTo(CodeString left, CodeString right)
        {
            if (left.IsNull())
                return right.IsNull();
            else
                return left.Equals(right);
        }

        public static bool operator ==(CodeString left, CodeString right) => IsEqualTo(left, right);

        public static bool operator !=(CodeString left, CodeString right) => !IsEqualTo(left, right);

        public static bool IsNullOrEmpty(CodeString value) => value == null || value.IsEmpty;

        #endregion

        #region Comparison
        
        public int CompareTo(CodeString other)
        {
            // Treat null like empty
            if (other == null)
                return 1;

            int length = Math.Min(this.Length, other.Length);

            for (int i = 0; i < length; i++)
            {
                var thispoint = this.codepoints[i];
                var otherpoint = other.codepoints[i];

                var result = thispoint.CompareTo(otherpoint);

                if (result != 0)
                    return result;
            }

            // All codepoints that line up match up, so we have to sort by length
            if (this.Length < other.Length)
                return -1;
            else if (this.Length > other.Length)
                return +1;
            else
                return 0;
        }

        public static int Compare(CodeString left, CodeString right)
        {
            if (left.IsNull())
            {
                if (right.IsNull())
                    return 0;
                else
                    return -1;
            }
            else
            {
                return left.CompareTo(right);
            }
        }

        public static bool operator <(CodeString left, CodeString right) => Compare(left, right) == -1;

        public static bool operator >(CodeString left, CodeString right) => Compare(left, right) == +1;

        public static bool operator <=(CodeString left, CodeString right) => Compare(left, right) != +1;

        public static bool operator >=(CodeString left, CodeString right) => Compare(left, right) != -1;

        #endregion

        #region Combination

        public static CodeString operator +(CodeString left, CodeString right) => Combine(left, right);

        public static CodeString operator +(CodeString left, CodePoint right) => Combine(left, right);

        public static CodeString operator +(CodePoint left, CodeString right) => Combine(left, right);


        public static CodeString Combine(CodeString left, CodeString right)
        {
            if (left == null)
                return right;

            if (right == null)
                return null;

            var combined = new CodePoint[left.Length + right.Length];

            Array.Copy(left.codepoints, 0, combined, 0, left.Length);

            Array.Copy(right.codepoints, left.Length, combined, 0, right.Length);

            return new CodeString(combined, false);
        }

        public static CodeString Combine(CodeString left, CodePoint right)
        {
            if (left == null)
                return new CodeString(right);

            var combined = new CodePoint[left.Length + 1];

            Array.Copy(left.codepoints, 0, combined, 0, left.Length);

            combined[left.Length] = right;

            return new CodeString(combined, false);
        }

        public static CodeString Combine(CodePoint left, CodeString right)
        {
            if (right == null)
                return new CodeString(left);


            var combined = new CodePoint[right.Length + 1];

            combined[0] = left;

            Array.Copy(right.codepoints, 1, combined, 0, right.Length);

            return new CodeString(combined, false);
        }

        public static CodeString Combine(params CodeString[] values) => Combine(values);

        public static CodeString Combine(IEnumerable<CodeString> values)
        {
            int length = 0;

            foreach (var value in values)
            {
                if (value != null)
                    length += value.Length;
            }

            var combined = new CodePoint[length];

            var index = 0;

            foreach (var value in values)
            {
                if (value != null)
                {
                    Array.Copy(value.codepoints, 0, combined, index, value.Length);
                    index += value.Length;
                }
            }

            return new CodeString(combined, false);
        }

        #endregion

        #region Unoptimized
        
        public CodeString Normalize()
        {
            string converted = this.ToString().Normalize();

            return new CodeString(converted);
        }

        public CodeString Normalize(NormalizationForm form)
        {
            string converted = this.ToString().Normalize(form);

            return new CodeString(converted);
        }

        public CodeString ToUpper()
        {
            string converted = this.ToString().ToUpper();

            return new CodeString(converted);
        }

        public CodeString ToUpper(CultureInfo culture)
        {
            string converted = this.ToString().ToUpper(culture);

            return new CodeString(converted);
        }

        public CodeString ToUpperInvariant()
        {
            string converted = this.ToString().ToUpperInvariant();

            return new CodeString(converted);
        }

        public CodeString ToLower()
        {
            string converted = this.ToString().ToLower();

            return new CodeString(converted);
        }

        public CodeString ToLower(CultureInfo culture)
        {
            string converted = this.ToString().ToLower(culture);

            return new CodeString(converted);
        }

        public CodeString ToLowerInvariant()
        {
            string converted = this.ToString().ToLowerInvariant();

            return new CodeString(converted);
        }

        #endregion

        #region Replicate

        private static CodePoint[] ReplicateCodePoint(CodePoint value, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("length", count, "length cannot be a negative number.");

            CodePoint[] codepoints = new CodePoint[count];

            for (int i = 0; i < count; i++)
                codepoints[i] = value;

            return codepoints;
        }

        public static CodeString operator *(CodeString value, int count) => Replicate(value, count);

        public static CodeString Replicate(CodeString value, int count)
        {
            var values = new CodeString[count];

            for (int i = 0; i < count; i++)
                values[i] = value;

            return Combine(values);
        }

        #endregion

        #region Join
        public static CodeString Join(CodeString separator, params CodeString[] values) => Join(separator, values);

        public static CodeString Join(CodeString separator, IEnumerable<CodeString> values) => Combine(GetEnumerable(separator, values));

        public static CodeString Join(CodeString separator, IEnumerable<object> values) => Combine(GetEnumerable(separator, values));


        private static IEnumerable<CodeString> GetEnumerable(CodeString separator, IEnumerable<CodeString> values)
        {
            bool initialized = false;

            foreach (var value in values)
            {
                if (initialized)
                    yield return separator;
                else
                    initialized = true;

                yield return value;
            }
        }

        private static IEnumerable<CodeString> GetEnumerable(CodeString separator, IEnumerable<object> values)
        {
            bool initialized = false;

            foreach (var value in values)
            {
                if (initialized)
                    yield return separator;
                else
                    initialized = true;

                var result = value == null ? Empty : new CodeString(value.ToString());

                yield return result;
            }
        }

        #endregion

        #region SubString

        public CodeString Substring(int start) => Substring(start, Length - start);

        public CodeString Substring(int start, int length)
        {
            if (start < 0 || start > Length)
                throw new ArgumentOutOfRangeException("start");

            if (start + length > Length)
                throw new ArgumentOutOfRangeException("length");

            var subpoints = new CodePoint[length];

            for (int i = 0, s = start; i < length; i++, s++)
                subpoints[i] = codepoints[s];

            return new CodeString(subpoints, false);
        }

        public CodeString Reverse()
        {
            var reversed = new CodePoint[Length];

            for (int i = 0, r = Length - 1; i < Length; i++, r--)
                reversed[i] = codepoints[r];

            return new CodeString(reversed, false);
        }

        #endregion

        #region String

        public override string ToString()
        {
            // should this be cached?
            StringBuilder builder = new StringBuilder();

            foreach (var codepoint in codepoints)
                builder.Append(codepoint.ToString());

            return builder.ToString();
        }

        public static implicit operator string(CodeString value) => value.IsNull() ? null : value.ToString();

        #endregion

        #region Enumerator

        public CodePoint[] ToCodePointArray() => (CodePoint[])codepoints.Clone();

        public IEnumerator<CodePoint> GetEnumerator()
        {
            foreach (CodePoint codepoint in codepoints)
                yield return codepoint;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #endregion


        // Constants
        public static readonly CodeString Empty = new CodeString(new CodePoint[] { }, false);
    }
}