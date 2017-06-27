using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Soedeum.Dotnet.Library.Utility;

namespace Soedeum.Dotnet.Library.Text
{
    public class CodeString : IEquatable<CodeString>, IComparable<CodeString>, IEnumerable<CodePoint>
    {
        readonly int hashcode;

        readonly CodePoint[] codepoints;


        private CodeString(CodePoint[] codepoints, bool clone)
        {
            this.hashcode = HashCodeCreator.Combiner.Combine(codepoints);

            if (clone)
                this.codepoints = (CodePoint[])codepoints.Clone();
            else
                this.codepoints = codepoints;
        }

        public CodeString(params CodePoint[] codepoints)
            : this(codepoints, true) { }

        public CodeString(IEnumerable<CodePoint> codepoints)
            : this(System.Linq.Enumerable.ToArray(codepoints), false) { }

        public CodeString(CodePoint value, int count)
            : this(ReplicateCodePoint(value, count), false) { }

        private static CodePoint[] ReplicateCodePoint(CodePoint value, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("amount", count, "Amount cannot be a negative number.");

            CodePoint[] codepoints = new CodePoint[count];

            for (int i = 0; i < count; i++)
                codepoints[i] = value;

            return codepoints;
        }


        // Indexer
        public CodePoint this[int index] => codepoints[index];

        public int Length => codepoints.Length;





        /* Operators */
        #region Operators

        // Equality
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

        public static bool operator ==(CodeString left, CodeString right) => left.Equals(right);

        public static bool operator !=(CodeString left, CodeString right) => !left.Equals(right);

        // Comparison
        public int CompareTo(CodeString other)
        {
            throw new NotImplementedException();
        }

        // Concatenation
        public static CodeString operator +(CodeString left, CodeString right) => Combine(left, right);

        public static CodeString operator +(CodeString left, CodePoint right) => Combine(left, right);

        public static CodeString operator +(CodePoint left, CodeString right) => Combine(left, right);


        public static CodeString Combine(CodeString left, CodeString right)
        {
            if (left == null)
                return right;

            if (right == null)
                return left;

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

        public static CodeString Combine(params CodeString[] values)
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

        public static CodeString operator *(CodeString value, int count) => Replicate(value, count);

        public static CodeString Replicate(CodeString value, int count)
        {
            var values = new CodeString[count];

            for (int i = 0; i < count; i++)
                values[i] = value;

            return Combine(values);
        }

        // String
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

        // Enumerator
        public IEnumerator<CodePoint> GetEnumerator()
        {
            foreach (CodePoint codepoint in codepoints)
                yield return codepoint;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}