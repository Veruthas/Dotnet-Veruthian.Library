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

        // Operators

        // String
        public override string ToString()
        {
            // should this be cached?
            StringBuilder builder = new StringBuilder();

            foreach (var codepoint in codepoints)
                builder.Append(codepoint.ToString());

            return builder.ToString();
        }



        // Enumerator
        public IEnumerator<CodePoint> GetEnumerator()
        {
            foreach (CodePoint codepoint in codepoints)
                yield return codepoint;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}