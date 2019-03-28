using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Veruthian.Library.Text.Encodings;
using Veruthian.Library.Text.Runes.Extensions;
using Veruthian.Library.Utility;

namespace Veruthian.Library.Text.Runes
{
    public class RuneBuffer : IEnumerable<Rune>, IEditableText<Rune, IEnumerable<Rune>>, IEditableText<Rune, RuneString>
    {
        List<Rune> runes;


        public RuneBuffer() => runes = new List<Rune>();

        public RuneBuffer(int capacity) => runes = new List<Rune>(capacity);

        public RuneBuffer(IEnumerable<Rune> runes) => runes = new List<Rune>(runes);


        public int Length => runes.Count;

        public Rune this[int position]
        {
            get => runes[position];
            set => runes[position] = value;
        }


        // Rune
        public RuneBuffer Append(Rune value)
        {
            runes.Add(value);

            return this;
        }

        public RuneBuffer Prepend(Rune value) => Insert(0, value);

        public RuneBuffer Insert(int position, Rune value)
        {
            runes.Insert(position, value);

            return this;
        }


        // IEnumerable<Rune>
        public RuneBuffer Append(IEnumerable<Rune> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            runes.AddRange(values);

            return this;
        }

        public RuneBuffer Prepend(IEnumerable<Rune> values) => Insert(0, values);

        public RuneBuffer Insert(int position, IEnumerable<Rune> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            runes.InsertRange(position, values);

            return this;
        }


        // RuneString
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

        public RuneBuffer Prepend(RuneString value) => Insert(0, value);

        public RuneBuffer Prepend(RuneString value, int start) => Insert(0, value, start);

        public RuneBuffer Prepend(RuneString value, int start, int length) => Insert(0, value, start, length);

        public RuneBuffer Insert(int position, RuneString value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            runes.InsertRange(position, value);

            return this;

        }

        public RuneBuffer Insert(int position, RuneString value, int start)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return Append(value, start, value.Length - start);
        }

        public RuneBuffer Insert(int position, RuneString value, int start, int length)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            for (int i = start; i < start + length; i++)
                runes.Insert(position++, value[i]);

            return this;
        }


        // IEnumerable<char>
        public RuneBuffer Append(IEnumerable<char> values)
        {
            if (values == null)
                throw new ArgumentNullException("value");

            Append(values.ToRunes());

            return this;
        }

        public RuneBuffer Prepend(IEnumerable<char> values) => Insert(0, values);

        public RuneBuffer Insert(int position, IEnumerable<char> values)
        {
            if (values == null)
                throw new ArgumentNullException("value");

            Insert(position, values.ToRunes());

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

        public RuneBuffer Prepend(string value) => Insert(0, value);

        public RuneBuffer Prepend(string value, int start) => Insert(0, value, start);

        public RuneBuffer Prepend(string value, int start, int length) => Insert(0, value, start, length);

        public RuneBuffer Insert(int position, string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Insert(position, value.ToRunes());

            return this;
        }

        public RuneBuffer Insert(int position, string value, int start)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Insert(position, value.ToRunes(start));

            return this;
        }

        public RuneBuffer Insert(int position, string value, int start, int length)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Insert(position, value.ToRunes(start, length));

            return this;
        }


        // Objects
        public RuneBuffer Append(object value)
        {
            Append(value.ToString());

            return this;
        }

        public RuneBuffer Prepend(object value) => Insert(0, value);

        public RuneBuffer Insert(int position, object value)
        {
            Insert(position, value.ToString());

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

        public RuneBuffer PrependFormat(string format, object arg0) => InsertFormat(0, format, arg0);

        public RuneBuffer PrependFormat(string format, object arg0, object arg1) => InsertFormat(0, format, arg0, arg1);

        public RuneBuffer PrependFormat(string format, object arg0, object arg1, object arg2) => InsertFormat(0, format, arg0, arg1, arg2);

        public RuneBuffer PrependFormat(string format, params object[] args) => InsertFormat(0, format, args);

        public RuneBuffer PrependFormat(IFormatProvider provider, string format, object arg0) => InsertFormat(0, provider, format, arg0);

        public RuneBuffer PrependFormat(IFormatProvider provider, string format, object arg0, object arg1) => InsertFormat(0, provider, format, arg0, arg1);

        public RuneBuffer PrependFormat(IFormatProvider provider, string format, object arg0, object arg1, object arg2) => InsertFormat(0, provider, format, arg0, arg1, arg2);

        public RuneBuffer PrependFormat(IFormatProvider provider, string format, params object[] args) => InsertFormat(0, provider, format, args);

        public RuneBuffer InsertFormat(int position, string format, object arg0)
        {
            string value = string.Format(format, arg0);

            Insert(position, format);

            return this;
        }

        public RuneBuffer InsertFormat(int position, string format, object arg0, object arg1)
        {
            string value = string.Format(format, arg0, arg1);

            Insert(position, format);

            return this;
        }

        public RuneBuffer InsertFormat(int position, string format, object arg0, object arg1, object arg2)
        {
            string value = string.Format(format, arg0, arg1, arg2);

            Insert(position, format);

            return this;
        }

        public RuneBuffer InsertFormat(int position, string format, params object[] args)
        {
            string value = string.Format(format, args);

            Insert(position, format);

            return this;
        }

        public RuneBuffer InsertFormat(int position, IFormatProvider provider, string format, object arg0)
        {
            string value = string.Format(provider, format, arg0);

            Insert(position, format);

            return this;
        }

        public RuneBuffer InsertFormat(int position, IFormatProvider provider, string format, object arg0, object arg1)
        {
            string value = string.Format(provider, format, arg0, arg1);

            Insert(position, format);

            return this;
        }

        public RuneBuffer InsertFormat(int position, IFormatProvider provider, string format, object arg0, object arg1, object arg2)
        {
            string value = string.Format(provider, format, arg0, arg1, arg2);

            Insert(position, format);

            return this;
        }

        public RuneBuffer InsertFormat(int position, IFormatProvider provider, string format, params object[] args)
        {
            string value = string.Format(provider, format, args);

            Insert(position, format);

            return this;
        }


        // Remove
        public RuneBuffer Remove(Rune value)
        {
            runes.Remove(value);

            return this;
        }

        public RuneBuffer Remove(int position)
        {
            runes.RemoveAt(position);

            return this;
        }

        public RuneBuffer Remove(int position, int amount)
        {
            runes.RemoveRange(position, amount);

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

        public RuneBuffer Reverse(int position, int amount)
        {
            runes.Reverse(position, amount);

            return this;
        }


        // Replace
        public RuneBuffer Replace(Rune oldValue, Rune newValue) => Replace(oldValue, newValue, 0, Length);

        public RuneBuffer Replace(Rune oldValue, Rune newValue, int position, int amount)
        {
            ExceptionHelper.VerifyInRange(position, amount, 0, runes.Count, nameof(position), nameof(amount));

            for (int i = position; i < position + amount; i++)
            {
                if (runes[i] == oldValue)
                    runes[i] = newValue;
            }

            return this;
        }


        // Conversion
        public RuneString ToRuneString()
        {
            return new RuneString(runes);
        }

        public RuneString ToRuneString(int position)
        {
            return new RuneString(runes, position);
        }

        public RuneString ToRuneString(int position, int length)
        {
            return new RuneString(runes, position, length);
        }

        public Rune[] ToRunes()
        {
            return runes.ToArray();
        }

        public Rune[] ToRunes(int position) => ToRunes(position, Length - position);

        public Rune[] ToRunes(int position, int amount)
        {
            ExceptionHelper.VerifyInRange(position, amount, 0, runes.Count, nameof(position), nameof(amount));

            Rune[] result = new Rune[amount];

            runes.CopyTo(position, result, 0, amount);

            return result;
        }


        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var rune in runes)
                builder.Append(rune.ToString());

            return builder.ToString();
        }


        public string ToString(int position) => ToString(position, Length - position);


        public string ToString(int position, int amount)
        {
            ExceptionHelper.VerifyInRange(position, amount, 0, runes.Count, nameof(position), nameof(amount));

            var builder = new StringBuilder();

            for (int i = position; i < position + amount; i++)
                builder.Append(runes[i].ToString());

            return builder.ToString();
        }



        // Enumerator
        public IEnumerator<Rune> GetEnumerator() => runes.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        
        // IEditableText<Rune, IEnumerable<Rune>>
        void IEditableText<Rune, IEnumerable<Rune>>.Append(Rune value) => Append(value);

        void IEditableText<Rune, IEnumerable<Rune>>.Append(IEnumerable<Rune> values) => Append(values);

        void IEditableText<Rune, IEnumerable<Rune>>.Prepend(Rune value) => Prepend(value);

        void IEditableText<Rune, IEnumerable<Rune>>.Prepend(IEnumerable<Rune> values) => Prepend(values);

        void IEditableText<Rune, IEnumerable<Rune>>.Insert(int position, Rune value) => Insert(position, value);

        void IEditableText<Rune, IEnumerable<Rune>>.Insert(int position, IEnumerable<Rune> values) => Insert(position, values);

        void IEditableText<Rune, IEnumerable<Rune>>.Remove(int position, int amount) => Remove(position, amount);


        // IEditableText<Rune, RuneString>
        void IEditableText<Rune, RuneString>.Append(Rune value) => Append(value);
        
        void IEditableText<Rune, RuneString>.Append(RuneString values) => Append(values);

        void IEditableText<Rune, RuneString>.Prepend(Rune value) => Prepend(value);

        void IEditableText<Rune, RuneString>.Prepend(RuneString values) => Prepend(values);

        void IEditableText<Rune, RuneString>.Insert(int position, Rune value) => Insert(position, value);

        void IEditableText<Rune, RuneString>.Insert(int position, RuneString values) => Insert(position, values);

        void IEditableText<Rune, RuneString>.Remove(int position, int amount) => Remove(position, amount);
    }
}