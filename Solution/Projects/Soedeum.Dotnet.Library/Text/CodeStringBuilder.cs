using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Soedeum.Dotnet.Library.Text.Encodings;

namespace Soedeum.Dotnet.Library.Text
{
    public class CodeStringBuilder : IEnumerable<CodePoint>
    {
        List<CodePoint> codepoints = new List<CodePoint>();


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
            codepoints.AddRange(values);

            return this;
        }
        public CodeStringBuilder Append(IEnumerator<CodePoint> values)
        {
            while (values.MoveNext())
                codepoints.Add(values.Current);

            return this;
        }

        public CodeStringBuilder Insert (int index)
        {
            
        }

        // CodeString
        public CodeStringBuilder Append(CodeString value) => Append(value);

        public CodeStringBuilder Append(CodeString value, int start) => Append(value, start, value.Length - start);

        public CodeStringBuilder Append(CodeString value, int start, int amount)
        {
            for (int i = start; i < start + amount; i++)
                codepoints.Add(value[i]);

            return this;
        }

        // Chars
        public CodeStringBuilder Append(IEnumerable<char> value)
        {
            Append(value.ToCodePoints());

            return this;
        }

        // Strings
        public CodeStringBuilder Append(string value)
        {
            Append(value.ToCodePoints());

            return this;
        }
        public CodeStringBuilder Append(string value, int start)
        {
            Append(value.ToCodePoints(start));

            return this;
        }
        public CodeStringBuilder Append(string value, int start, int amount)
        {
            Append(value.ToCodePoints(start, amount));

            return this;
        }

        // Objects
        public CodeStringBuilder Append(object value)
        {
            Append(value.ToString());

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


        public CodeString ToCodeString()
        {
            return new CodeString(codepoints);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var codepoint in codepoints)
                builder.Append(codepoint.ToString());

            return builder.ToString();
        }

        public IEnumerator<CodePoint> GetEnumerator() => codepoints.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }