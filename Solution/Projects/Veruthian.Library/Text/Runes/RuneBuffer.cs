using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Veruthian.Library.Text.Encodings;
using Veruthian.Library.Text.Runes.Extensions;

namespace Veruthian.Library.Text.Runes
{
    public class RuneBuffer : IEnumerable<Rune>
    {
        List<Rune> runes;


        public RuneBuffer() => runes = new List<Rune>();

        public RuneBuffer(int capacity) => runes = new List<Rune>(capacity);

        public RuneBuffer(IEnumerable<Rune> runes) => runes = new List<Rune>(runes);


        public int Length => runes.Count;

        public Rune this[int index]
        {
            get => runes[index];
            set => runes[index] = value;
        }

        // Runes
        public RuneBuffer Append(Rune value)
        {
            runes.Add(value);

            return this;
        }
        public RuneBuffer Insert(int index, Rune value)
        {
            runes.Insert(index, value);

            return this;
        }


        public RuneBuffer Append(IEnumerable<Rune> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            runes.AddRange(values);

            return this;
        }
        public RuneBuffer Append(IEnumerator<Rune> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            while (values.MoveNext())
                runes.Add(values.Current);

            return this;
        }

        public RuneBuffer Insert(int index, IEnumerable<Rune> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            runes.InsertRange(index, values);

            return this;
        }

        public RuneBuffer Insert(int index, IEnumerator<Rune> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            while (values.MoveNext())
                runes.Insert(index, values.Current);

            return this;
        }

        // CodeString
        public RuneBuffer Append(RuneString value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            runes.AddRange(value);

            return this;
        }

        public RuneBuffer Append(RuneString value, int start)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return Append(value, start, value.Length - start);
        }

        public RuneBuffer Append(RuneString value, int start, int length)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            for (int i = start; i < start + length; i++)
                runes.Add(value[i]);

            return this;
        }

        public RuneBuffer Insert(int index, RuneString value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            runes.InsertRange(index, value);

            return this;

        }

        public RuneBuffer Insert(int index, RuneString value, int start)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return Append(value, start, value.Length - start);
        }

        public RuneBuffer Insert(int index, RuneString value, int start, int length)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            for (int i = start; i < start + length; i++)
                runes.Insert(index++, value[i]);

            return this;
        }

        // Chars
        public RuneBuffer Append(IEnumerable<char> value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Append(value.ToRunes());

            return this;
        }

        public RuneBuffer Insert(int index, IEnumerable<char> value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Insert(index, value.ToRunes());

            return this;
        }

        // Strings
        public RuneBuffer Append(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Append(value.ToRunes());

            return this;
        }
        public RuneBuffer Append(string value, int start)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Append(value.ToRunes(start));

            return this;
        }
        public RuneBuffer Append(string value, int start, int length)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Append(value.ToRunes(start, length));

            return this;
        }

        public RuneBuffer Insert(int index, string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Insert(index, value.ToRunes());

            return this;
        }
        public RuneBuffer Insert(int index, string value, int start)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Insert(index, value.ToRunes(start));

            return this;
        }
        public RuneBuffer Insert(int index, string value, int start, int length)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Insert(index, value.ToRunes(start, length));

            return this;
        }


        // Objects
        public RuneBuffer Append(object value)
        {
            Append(value.ToString());

            return this;
        }

        public RuneBuffer Insert(int index, object value)
        {
            Insert(index, value.ToString());

            return this;
        }

        // Formatted strings
        public RuneBuffer AppendFormat(string format, object arg0)
        {
            string value = string.Format(format, arg0);

            Append(format);

            return this;
        }
        public RuneBuffer AppendFormat(string format, object arg0, object arg1)
        {
            string value = string.Format(format, arg0, arg1);

            Append(format);

            return this;
        }
        public RuneBuffer AppendFormat(string format, object arg0, object arg1, object arg2)
        {
            string value = string.Format(format, arg0, arg1, arg2);

            Append(format);

            return this;
        }
        public RuneBuffer AppendFormat(string format, params object[] args)
        {
            string value = string.Format(format, args);

            Append(format);

            return this;
        }

        public RuneBuffer AppendFormat(IFormatProvider provider, string format, object arg0)
        {
            string value = string.Format(provider, format, arg0);

            Append(format);

            return this;
        }
        public RuneBuffer AppendFormat(IFormatProvider provider, string format, object arg0, object arg1)
        {
            string value = string.Format(provider, format, arg0, arg1);

            Append(format);

            return this;
        }
        public RuneBuffer AppendFormat(IFormatProvider provider, string format, object arg0, object arg1, object arg2)
        {
            string value = string.Format(provider, format, arg0, arg1, arg2);

            Append(format);

            return this;
        }
        public RuneBuffer AppendFormat(IFormatProvider provider, string format, params object[] args)
        {
            string value = string.Format(provider, format, args);

            Append(format);

            return this;
        }

        public RuneBuffer InsertFormat(int index, string format, object arg0)
        {
            string value = string.Format(format, arg0);

            Insert(index, format);

            return this;
        }
        public RuneBuffer InsertFormat(int index, string format, object arg0, object arg1)
        {
            string value = string.Format(format, arg0, arg1);

            Insert(index, format);

            return this;
        }
        public RuneBuffer InsertFormat(int index, string format, object arg0, object arg1, object arg2)
        {
            string value = string.Format(format, arg0, arg1, arg2);

            Insert(index, format);

            return this;
        }
        public RuneBuffer InsertFormat(int index, string format, params object[] args)
        {
            string value = string.Format(format, args);

            Insert(index, format);

            return this;
        }

        public RuneBuffer InsertFormat(int index, IFormatProvider provider, string format, object arg0)
        {
            string value = string.Format(provider, format, arg0);

            Insert(index, format);

            return this;
        }
        public RuneBuffer InsertFormat(int index, IFormatProvider provider, string format, object arg0, object arg1)
        {
            string value = string.Format(provider, format, arg0, arg1);

            Insert(index, format);

            return this;
        }
        public RuneBuffer InsertFormat(int index, IFormatProvider provider, string format, object arg0, object arg1, object arg2)
        {
            string value = string.Format(provider, format, arg0, arg1, arg2);

            Insert(index, format);

            return this;
        }
        public RuneBuffer InsertFormat(int index, IFormatProvider provider, string format, params object[] args)
        {
            string value = string.Format(provider, format, args);

            Insert(index, format);

            return this;
        }

        // Remove
        public RuneBuffer Remove(Rune value)
        {
            runes.Remove(value);

            return this;
        }

        public RuneBuffer Remove(int index)
        {
            runes.RemoveAt(index);

            return this;
        }

        public RuneBuffer Remove(int index, int length)
        {
            runes.RemoveRange(index, length);

            return this;
        }

        public RuneBuffer Remove(Predicate<Rune> match)
        {
            runes.RemoveAll(match);

            return this;
        }

        public RuneBuffer Clear()
        {
            runes.Clear();

            return this;
        }


        // Reverse
        public RuneBuffer Reverse()
        {
            runes.Reverse();

            return this;
        }

        public RuneBuffer Reverse(int index, int length)
        {
            runes.Reverse(index, length);

            return this;
        }

        // Replace
        public RuneBuffer Replace(Rune oldValue, Rune newValue) => Replace(oldValue, newValue, 0, Length);

        public RuneBuffer Replace(Rune oldValue, Rune newValue, int index, int length)
        {
            VerifyInRange(index, length);

            for (int i = index; i < index + length; i++)
            {
                if (runes[i] == oldValue)
                    runes[i] = newValue;
            }

            return this;
        }

        
        // Conversion
        public RuneString ToCodeString()
        {
            return new RuneString(runes);
        }
        public RuneString ToCodeString(int index)
        {
            return new RuneString(runes, index);
        }
        public RuneString ToCodeString(int index, int length)
        {
            return new RuneString(runes, index, length);
        }

        public Rune[] ToRunes()
        {
            return runes.ToArray();
        }
        public Rune[] ToRunes(int index) => ToRunes(index, Length - index);

        public Rune[] ToRunes(int index, int length)
        {
            VerifyInRange(index, length);

            Rune[] result = new Rune[length];

            runes.CopyTo(index, result, 0, length);

            return result;
        }


        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var rune in runes)
                builder.Append(rune.ToString());

            return builder.ToString();
        }

        public string ToString(int index) => ToString(index, Length - index);

        public string ToString(int index, int length)
        {
            VerifyInRange(index, length);

            var builder = new StringBuilder();

            for (int i = index; i < index + length; i++)
                builder.Append(runes[i].ToString());

            return builder.ToString();
        }
        

        private void VerifyInRange(int index, int length)
        {
            if (index < 0 || index > runes.Count)
                throw new ArgumentOutOfRangeException("index");
            if (length < 0 || index + length > runes.Count)
                throw new ArgumentOutOfRangeException("length");
        }

        public IEnumerator<Rune> GetEnumerator() => runes.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}