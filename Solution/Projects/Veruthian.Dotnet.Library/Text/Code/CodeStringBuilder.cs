using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Veruthian.Dotnet.Library.Text.Code.Encodings;
using Veruthian.Dotnet.Library.Text.Code.Extensions;

namespace Veruthian.Dotnet.Library.Text.Code
{
    public class CodeStringBuilder : IEnumerable<CodePoint>
    {
        List<CodePoint> codepoints;


        public CodeStringBuilder() => codepoints = new List<CodePoint>();

        public CodeStringBuilder(int capacity) => codepoints = new List<CodePoint>(capacity);

        public CodeStringBuilder(IEnumerable<CodePoint> codepoints) => codepoints = new List<CodePoint>(codepoints);


        public int Length => codepoints.Count;

        public CodePoint this[int index]
        {
            get => codepoints[index];
            set => codepoints[index] = value;
        }

        // CodePoints
        public CodeStringBuilder Append(CodePoint value)
        {
            codepoints.Add(value);

            return this;
        }
        public CodeStringBuilder Insert(int index, CodePoint value)
        {
            codepoints.Insert(index, value);

            return this;
        }


        public CodeStringBuilder Append(IEnumerable<CodePoint> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            codepoints.AddRange(values);

            return this;
        }
        public CodeStringBuilder Append(IEnumerator<CodePoint> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            while (values.MoveNext())
                codepoints.Add(values.Current);

            return this;
        }

        public CodeStringBuilder Insert(int index, IEnumerable<CodePoint> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            codepoints.InsertRange(index, values);

            return this;
        }

        public CodeStringBuilder Insert(int index, IEnumerator<CodePoint> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            while (values.MoveNext())
                codepoints.Insert(index, values.Current);

            return this;
        }

        // CodeString
        public CodeStringBuilder Append(CodeString value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            codepoints.AddRange(value);

            return this;
        }

        public CodeStringBuilder Append(CodeString value, int start)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return Append(value, start, value.Length - start);
        }

        public CodeStringBuilder Append(CodeString value, int start, int length)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            for (int i = start; i < start + length; i++)
                codepoints.Add(value[i]);

            return this;
        }

        public CodeStringBuilder Insert(int index, CodeString value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            codepoints.InsertRange(index, value);

            return this;

        }

        public CodeStringBuilder Insert(int index, CodeString value, int start)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return Append(value, start, value.Length - start);
        }

        public CodeStringBuilder Insert(int index, CodeString value, int start, int length)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            for (int i = start; i < start + length; i++)
                codepoints.Insert(index++, value[i]);

            return this;
        }

        // Chars
        public CodeStringBuilder Append(IEnumerable<char> value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Append(value.ToCodePoints());

            return this;
        }

        public CodeStringBuilder Insert(int index, IEnumerable<char> value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Insert(index, value.ToCodePoints());

            return this;
        }

        // Strings
        public CodeStringBuilder Append(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Append(value.ToCodePoints());

            return this;
        }
        public CodeStringBuilder Append(string value, int start)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Append(value.ToCodePoints(start));

            return this;
        }
        public CodeStringBuilder Append(string value, int start, int length)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Append(value.ToCodePoints(start, length));

            return this;
        }

        public CodeStringBuilder Insert(int index, string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Insert(index, value.ToCodePoints());

            return this;
        }
        public CodeStringBuilder Insert(int index, string value, int start)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Insert(index, value.ToCodePoints(start));

            return this;
        }
        public CodeStringBuilder Insert(int index, string value, int start, int length)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Insert(index, value.ToCodePoints(start, length));

            return this;
        }


        // Objects
        public CodeStringBuilder Append(object value)
        {
            Append(value.ToString());

            return this;
        }

        public CodeStringBuilder Insert(int index, object value)
        {
            Insert(index, value.ToString());

            return this;
        }

        // Formatted strings
        public CodeStringBuilder AppendFormat(string format, object arg0)
        {
            string value = string.Format(format, arg0);

            Append(format);

            return this;
        }
        public CodeStringBuilder AppendFormat(string format, object arg0, object arg1)
        {
            string value = string.Format(format, arg0, arg1);

            Append(format);

            return this;
        }
        public CodeStringBuilder AppendFormat(string format, object arg0, object arg1, object arg2)
        {
            string value = string.Format(format, arg0, arg1, arg2);

            Append(format);

            return this;
        }
        public CodeStringBuilder AppendFormat(string format, params object[] args)
        {
            string value = string.Format(format, args);

            Append(format);

            return this;
        }

        public CodeStringBuilder AppendFormat(IFormatProvider provider, string format, object arg0)
        {
            string value = string.Format(provider, format, arg0);

            Append(format);

            return this;
        }
        public CodeStringBuilder AppendFormat(IFormatProvider provider, string format, object arg0, object arg1)
        {
            string value = string.Format(provider, format, arg0, arg1);

            Append(format);

            return this;
        }
        public CodeStringBuilder AppendFormat(IFormatProvider provider, string format, object arg0, object arg1, object arg2)
        {
            string value = string.Format(provider, format, arg0, arg1, arg2);

            Append(format);

            return this;
        }
        public CodeStringBuilder AppendFormat(IFormatProvider provider, string format, params object[] args)
        {
            string value = string.Format(provider, format, args);

            Append(format);

            return this;
        }

        public CodeStringBuilder InsertFormat(int index, string format, object arg0)
        {
            string value = string.Format(format, arg0);

            Insert(index, format);

            return this;
        }
        public CodeStringBuilder InsertFormat(int index, string format, object arg0, object arg1)
        {
            string value = string.Format(format, arg0, arg1);

            Insert(index, format);

            return this;
        }
        public CodeStringBuilder InsertFormat(int index, string format, object arg0, object arg1, object arg2)
        {
            string value = string.Format(format, arg0, arg1, arg2);

            Insert(index, format);

            return this;
        }
        public CodeStringBuilder InsertFormat(int index, string format, params object[] args)
        {
            string value = string.Format(format, args);

            Insert(index, format);

            return this;
        }

        public CodeStringBuilder InsertFormat(int index, IFormatProvider provider, string format, object arg0)
        {
            string value = string.Format(provider, format, arg0);

            Insert(index, format);

            return this;
        }
        public CodeStringBuilder InsertFormat(int index, IFormatProvider provider, string format, object arg0, object arg1)
        {
            string value = string.Format(provider, format, arg0, arg1);

            Insert(index, format);

            return this;
        }
        public CodeStringBuilder InsertFormat(int index, IFormatProvider provider, string format, object arg0, object arg1, object arg2)
        {
            string value = string.Format(provider, format, arg0, arg1, arg2);

            Insert(index, format);

            return this;
        }
        public CodeStringBuilder InsertFormat(int index, IFormatProvider provider, string format, params object[] args)
        {
            string value = string.Format(provider, format, args);

            Insert(index, format);

            return this;
        }

        // Remove
        public CodeStringBuilder Remove(CodePoint value)
        {
            codepoints.Remove(value);

            return this;
        }

        public CodeStringBuilder Remove(int index)
        {
            codepoints.RemoveAt(index);

            return this;
        }

        public CodeStringBuilder Remove(int index, int length)
        {
            codepoints.RemoveRange(index, length);

            return this;
        }

        public CodeStringBuilder Remove(Predicate<CodePoint> match)
        {
            codepoints.RemoveAll(match);

            return this;
        }

        public CodeStringBuilder Clear()
        {
            codepoints.Clear();

            return this;
        }


        // Reverse
        public CodeStringBuilder Reverse()
        {
            codepoints.Reverse();

            return this;
        }

        public CodeStringBuilder Reverse(int index, int length)
        {
            codepoints.Reverse(index, length);

            return this;
        }

        // Replace
        public CodeStringBuilder Replace(CodePoint oldValue, CodePoint newValue) => Replace(oldValue, newValue, 0, Length);

        public CodeStringBuilder Replace(CodePoint oldValue, CodePoint newValue, int index, int length)
        {
            VerifyInRange(index, length);

            for (int i = index; i < index + length; i++)
            {
                if (codepoints[i] == oldValue)
                    codepoints[i] = newValue;
            }

            return this;
        }

        
        // Conversion
        public CodeString ToCodeString()
        {
            return new CodeString(codepoints);
        }
        public CodeString ToCodeString(int index)
        {
            return new CodeString(codepoints, index);
        }
        public CodeString ToCodeString(int index, int length)
        {
            return new CodeString(codepoints, index, length);
        }

        public CodePoint[] ToCodePoints()
        {
            return codepoints.ToArray();
        }
        public CodePoint[] ToCodePoints(int index) => ToCodePoints(index, Length - index);

        public CodePoint[] ToCodePoints(int index, int length)
        {
            VerifyInRange(index, length);

            CodePoint[] result = new CodePoint[length];

            codepoints.CopyTo(index, result, 0, length);

            return result;
        }


        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var codepoint in codepoints)
                builder.Append(codepoint.ToString());

            return builder.ToString();
        }

        public string ToString(int index) => ToString(index, Length - index);

        public string ToString(int index, int length)
        {
            VerifyInRange(index, length);

            var builder = new StringBuilder();

            for (int i = index; i < index + length; i++)
                builder.Append(codepoints[i].ToString());

            return builder.ToString();
        }
        

        private void VerifyInRange(int index, int length)
        {
            if (index < 0 || index > codepoints.Count)
                throw new ArgumentOutOfRangeException("index");
            if (length < 0 || index + length > codepoints.Count)
                throw new ArgumentOutOfRangeException("length");
        }

        public IEnumerator<CodePoint> GetEnumerator() => codepoints.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}