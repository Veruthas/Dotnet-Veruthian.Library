using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Veruthian.Dotnet.Library.Collections;
using Veruthian.Dotnet.Library.Text.Runes.Extensions;

namespace Veruthian.Dotnet.Library.Text.Runes
{
    public class RuneString : IEquatable<RuneString>, IComparable<RuneString>, IEnumerable<Rune>
    {
        readonly int hashcode;

        readonly Rune[] runes;


        #region Constructors

        private RuneString(Rune[] runes, bool clone)
        {
            this.hashcode = HashCodes.Default.Combine(runes);

            if (clone)
                this.runes = (Rune[])runes.Clone();
            else
                this.runes = runes;
        }

        public RuneString(Rune rune)
            : this(new[] { rune }, false) { }

        public RuneString(params Rune[] runes)
            : this(runes, true) { }


        public RuneString(IEnumerable<Rune> runes)
            : this(System.Linq.Enumerable.ToArray(runes), false) { }

        private static Rune[] GetFromEnumerator(IEnumerable<Rune> runes)
        {
            var result = new List<Rune>();

            foreach(var rune in runes)
                result.Add(rune);

            return result.ToArray();
        }

        public RuneString(ICollection<Rune> runes)
                : this(GetFromCollection(runes), false) { }

        public RuneString(IList<Rune> runes, int index)
            : this(GetFromList(runes, index, runes.Count - index), false) { }

        public RuneString(IList<Rune> runes, int index, int length)
            : this(GetFromList(runes, index, length), false) { }

        private static Rune[] GetFromCollection(ICollection<Rune> ruins)
        {
            if (ruins == null)
                throw new ArgumentNullException("runes");

            Rune[] values = new Rune[ruins.Count];

            ruins.CopyTo(values, 0);

            return values;
        }

        private static Rune[] GetFromList(IList<Rune> runes, int index, int length)
        {
            if (runes == null)
                throw new ArgumentNullException("runes");
            if (index < 0 || index > runes.Count)
                throw new ArgumentOutOfRangeException("index");
            if (length < 0 || index + length > runes.Count)
                throw new ArgumentOutOfRangeException("length");

            Rune[] values = new Rune[length];

            if (runes is List<Rune>)
            {
                var list = runes as List<Rune>;
                list.CopyTo(index, values, 0, length);
            }
            else
            {
                for (int i = 0; i < length; i++)
                    values[i] = runes[index + i];
            }

            return values;
        }

        private static Rune[] GetFromIndex(IIndex<int, Rune> runes, int index, int length)
        {
            if (runes == null)
                throw new ArgumentNullException("runes");
            if ((uint)index > runes.Count)
                throw new ArgumentOutOfRangeException("index");
            if ((uint)index + (uint)length > runes.Count)
                throw new ArgumentOutOfRangeException("length");

            Rune[] values = new Rune[length];

            if (runes is List<Rune>)
            {
                var list = runes as List<Rune>;
                list.CopyTo(index, values, 0, length);
            }
            else
            {
                for (int i = 0; i < length; i++)
                    values[i] = runes[index + i];
            }

            return values;
        }

        public RuneString(Rune value, int count)
            : this(ReplicateRune(value, count), false) { }

        public RuneString(string value)
            : this(value.ToRuneArray(), false) { }

        public RuneString(string value, int start)
            : this(value.ToRuneArray(start), false) { }

        public RuneString(string value, int start, int length)
            : this(value.ToRuneArray(start, length), false) { }


        #endregion

        
        public Rune this[int index] => runes[index];

        public int Length => runes.Length;

        public bool IsEmpty => Length == 0;


        #region Operations

        #region Validation

        public bool IsValid
        {
            get
            {
                foreach (var rune in runes)
                    if (rune.IsInvalid)
                        return false;

                return true;
            }
        }

        public void Validate()
        {
            foreach (var rune in runes)
                rune.VerifyIsValid();
        }

        #endregion

        #region Equality

        public override int GetHashCode() => hashcode;

        public override bool Equals(object obj) => (obj is RuneString) ? Equals(obj as RuneString) : false;


        public bool Equals(RuneString other)
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
                for (int i = 0; i < runes.Length; i++)
                {
                    if (runes[i] != other.runes[i])
                        return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsEqualTo(RuneString left, RuneString right)
        {
            if (left.IsNull())
                return right.IsNull();
            else
                return left.Equals(right);
        }

        public static bool operator ==(RuneString left, RuneString right) => IsEqualTo(left, right);

        public static bool operator !=(RuneString left, RuneString right) => !IsEqualTo(left, right);

        public static bool IsNullOrEmpty(RuneString value) => value == null || value.IsEmpty;

        #endregion

        #region Comparison
        
        public int CompareTo(RuneString other)
        {
            // Treat null like empty
            if (other == null)
                return 1;

            int length = Math.Min(this.Length, other.Length);

            for (int i = 0; i < length; i++)
            {
                var thisRune = this.runes[i];
                var otherRune = other.runes[i];

                var result = thisRune.CompareTo(otherRune);

                if (result != 0)
                    return result;
            }

            // All runes that line up match up, so we have to sort by length
            if (this.Length < other.Length)
                return -1;
            else if (this.Length > other.Length)
                return +1;
            else
                return 0;
        }

        public static int Compare(RuneString left, RuneString right)
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

        public static bool operator <(RuneString left, RuneString right) => Compare(left, right) == -1;

        public static bool operator >(RuneString left, RuneString right) => Compare(left, right) == +1;

        public static bool operator <=(RuneString left, RuneString right) => Compare(left, right) != +1;

        public static bool operator >=(RuneString left, RuneString right) => Compare(left, right) != -1;

        #endregion

        #region Combination

        public static RuneString operator +(RuneString left, RuneString right) => Combine(left, right);

        public static RuneString operator +(RuneString left, Rune right) => Combine(left, right);

        public static RuneString operator +(Rune left, RuneString right) => Combine(left, right);


        public static RuneString Combine(RuneString left, RuneString right)
        {
            if (left == null)
                return right;

            if (right == null)
                return null;

            var combined = new Rune[left.Length + right.Length];

            Array.Copy(left.runes, 0, combined, 0, left.Length);

            Array.Copy(right.runes, left.Length, combined, 0, right.Length);

            return new RuneString(combined, false);
        }

        public static RuneString Combine(RuneString left, Rune right)
        {
            if (left == null)
                return new RuneString(right);

            var combined = new Rune[left.Length + 1];

            Array.Copy(left.runes, 0, combined, 0, left.Length);

            combined[left.Length] = right;

            return new RuneString(combined, false);
        }

        public static RuneString Combine(Rune left, RuneString right)
        {
            if (right == null)
                return new RuneString(left);


            var combined = new Rune[right.Length + 1];

            combined[0] = left;

            Array.Copy(right.runes, 1, combined, 0, right.Length);

            return new RuneString(combined, false);
        }

        public static RuneString Combine(params RuneString[] values) => Combine(values);

        public static RuneString Combine(IEnumerable<RuneString> values)
        {
            int length = 0;

            foreach (var value in values)
            {
                if (value != null)
                    length += value.Length;
            }

            var combined = new Rune[length];

            var index = 0;

            foreach (var value in values)
            {
                if (value != null)
                {
                    Array.Copy(value.runes, 0, combined, index, value.Length);
                    index += value.Length;
                }
            }

            return new RuneString(combined, false);
        }

        #endregion

        #region Unoptimized
        
        public RuneString Normalize()
        {
            string converted = this.ToString().Normalize();

            return new RuneString(converted);
        }

        public RuneString Normalize(NormalizationForm form)
        {
            string converted = this.ToString().Normalize(form);

            return new RuneString(converted);
        }

        public RuneString ToUpper()
        {
            string converted = this.ToString().ToUpper();

            return new RuneString(converted);
        }

        public RuneString ToUpper(CultureInfo culture)
        {
            string converted = this.ToString().ToUpper(culture);

            return new RuneString(converted);
        }

        public RuneString ToUpperInvariant()
        {
            string converted = this.ToString().ToUpperInvariant();

            return new RuneString(converted);
        }

        public RuneString ToLower()
        {
            string converted = this.ToString().ToLower();

            return new RuneString(converted);
        }

        public RuneString ToLower(CultureInfo culture)
        {
            string converted = this.ToString().ToLower(culture);

            return new RuneString(converted);
        }

        public RuneString ToLowerInvariant()
        {
            string converted = this.ToString().ToLowerInvariant();

            return new RuneString(converted);
        }

        #endregion

        #region Replicate

        private static Rune[] ReplicateRune(Rune value, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("length", count, "length cannot be a negative number.");

            Rune[] runes = new Rune[count];

            for (int i = 0; i < count; i++)
                runes[i] = value;

            return runes;
        }

        public static RuneString operator *(RuneString value, int count) => Replicate(value, count);

        public static RuneString Replicate(RuneString value, int count)
        {
            var values = new RuneString[count];

            for (int i = 0; i < count; i++)
                values[i] = value;

            return Combine(values);
        }

        #endregion

        #region Join
        public static RuneString Join(RuneString separator, params RuneString[] values) => Join(separator, values);

        public static RuneString Join(RuneString separator, IEnumerable<RuneString> values) => Combine(GetEnumerable(separator, values));

        public static RuneString Join(RuneString separator, IEnumerable<object> values) => Combine(GetEnumerable(separator, values));


        private static IEnumerable<RuneString> GetEnumerable(RuneString separator, IEnumerable<RuneString> values)
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

        private static IEnumerable<RuneString> GetEnumerable(RuneString separator, IEnumerable<object> values)
        {
            bool initialized = false;

            foreach (var value in values)
            {
                if (initialized)
                    yield return separator;
                else
                    initialized = true;

                var result = value == null ? Empty : new RuneString(value.ToString());

                yield return result;
            }
        }

        #endregion

        #region SubString

        public RuneString Substring(int start) => Substring(start, Length - start);

        public RuneString Substring(int start, int length)
        {
            if (start < 0 || start > Length)
                throw new ArgumentOutOfRangeException("start");

            if (start + length > Length)
                throw new ArgumentOutOfRangeException("length");

            var subpoints = new Rune[length];

            for (int i = 0, s = start; i < length; i++, s++)
                subpoints[i] = runes[s];

            return new RuneString(subpoints, false);
        }

        public RuneString Reverse()
        {
            var reversed = new Rune[Length];

            for (int i = 0, r = Length - 1; i < Length; i++, r--)
                reversed[i] = runes[r];

            return new RuneString(reversed, false);
        }

        #endregion

        #region String

        public override string ToString()
        {
            // should this be cached?
            StringBuilder builder = new StringBuilder();

            foreach (var rune in runes)
                builder.Append(rune.ToString());

            return builder.ToString();
        }

        public static implicit operator string(RuneString value) => value.IsNull() ? null : value.ToString();

        #endregion

        #region Enumerator

        public Rune[] ToRuneArray() => (Rune[])runes.Clone();

        public IEnumerator<Rune> GetEnumerator()
        {
            foreach (Rune rune in runes)
                yield return rune;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #endregion


        // Constants
        public static readonly RuneString Empty = new RuneString(new Rune[] { }, false);
    }
}