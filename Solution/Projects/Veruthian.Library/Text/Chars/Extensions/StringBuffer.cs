using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Veruthian.Library.Utility;

namespace Veruthian.Library.Text.Chars.Extensions
{
    public class StringBuffer : IEnumerable<char>, IEditableText<char>, IEditableText<char, IEnumerable<char>>, IEditableText<char, string>
    {
        StringBuilder builder;


        public StringBuffer() => this.builder = new StringBuilder();

        public StringBuffer(int capacity) => this.builder = new StringBuilder(capacity);

        public StringBuffer(IEnumerable<char> values) { this.builder = new StringBuilder(); this.builder.Append(values); }


        public int Length => builder.Length;

        public char this[int position]
        {
            get => builder[position];
            set => builder[position] = value;
        }


        // char
        public StringBuffer Append(char value)
        {
            builder.Append(value);

            return this;
        }

        public StringBuffer Prepend(char value) => Insert(0, value);

        public StringBuffer Insert(int position, char value)
        {
            builder.Insert(position, value);

            return this;
        }


        // IEnumerable<char>
        public StringBuffer Append(IEnumerable<char> values)
        {
            ExceptionHelper.VerifyNotNull(values, nameof(values));

            builder.Append(values);

            return this;
        }

        public StringBuffer Prepend(IEnumerable<char> values) => Insert(0, values);

        public StringBuffer Insert(int position, IEnumerable<char> values)
        {
            ExceptionHelper.VerifyNotNull(values, nameof(values));

            builder.Insert(position, values);

            return this;
        }


        // string
        public StringBuffer Append(string value)
        {
            ExceptionHelper.VerifyNotNull(value, nameof(value));

            builder.Append(value);

            return this;
        }

        public StringBuffer Append(string value, int start) => Append(value, start, value.Length - start);

        public StringBuffer Append(string value, int start, int length)
        {
            ExceptionHelper.VerifyNotNull(value, nameof(value));

            for (int i = start; i < start + length; i++)
                builder.Append(value[i]);

            return this;
        }

        public StringBuffer Prepend(string value) => Insert(0, value);

        public StringBuffer Prepend(string value, int start) => Insert(0, value, start);

        public StringBuffer Prepend(string value, int start, int length) => Insert(0, value, start, length);

        public StringBuffer Insert(int position, string value)
        {
            ExceptionHelper.VerifyNotNull(value, nameof(value));

            builder.Insert(position, value);

            return this;

        }

        public StringBuffer Insert(int position, string value, int start) => Insert(position, value, start, value.Length - start);

        public StringBuffer Insert(int position, string value, int start, int length)
        {
            ExceptionHelper.VerifyNotNull(value, nameof(value));

            for (int i = start; i < start + length; i++)
                builder.Insert(position++, value[i]);

            return this;
        }



        // Objects
        public StringBuffer Append(object value)
        {
            Append(value.ToString());

            return this;
        }

        public StringBuffer Prepend(object value) => Insert(0, value);

        public StringBuffer Insert(int position, object value)
        {
            Insert(position, value.ToString());

            return this;
        }


        // Formatted strings
        public StringBuffer AppendFormat(string format, object arg0)
        {
            builder.AppendFormat(format, arg0);

            return this;
        }

        public StringBuffer AppendFormat(string format, object arg0, object arg1)
        {
            builder.AppendFormat(format, arg0, arg1);

            return this;
        }

        public StringBuffer AppendFormat(string format, object arg0, object arg1, object arg2)
        {
            builder.AppendFormat(format, arg0, arg1, arg2);

            return this;
        }

        public StringBuffer AppendFormat(string format, params object[] args)
        {
            builder.AppendFormat(format, args);

            return this;
        }

        public StringBuffer AppendFormat(IFormatProvider provider, string format, object arg0)
        {
            builder.AppendFormat(provider, format, arg0);

            return this;
        }

        public StringBuffer AppendFormat(IFormatProvider provider, string format, object arg0, object arg1)
        {
            builder.AppendFormat(provider, format, arg0, arg1);

            return this;
        }

        public StringBuffer AppendFormat(IFormatProvider provider, string format, object arg0, object arg1, object arg2)
        {
            builder.AppendFormat(provider, format, arg0, arg1, arg2);

            return this;
        }

        public StringBuffer AppendFormat(IFormatProvider provider, string format, params object[] args)
        {
            builder.AppendFormat(provider, format, args);

            return this;
        }

        public StringBuffer PrependFormat(string format, object arg0) => InsertFormat(0, format, arg0);

        public StringBuffer PrependFormat(string format, object arg0, object arg1) => InsertFormat(0, format, arg0, arg1);

        public StringBuffer PrependFormat(string format, object arg0, object arg1, object arg2) => InsertFormat(0, format, arg0, arg1, arg2);

        public StringBuffer PrependFormat(string format, params object[] args) => InsertFormat(0, format, args);

        public StringBuffer PrependFormat(IFormatProvider provider, string format, object arg0) => InsertFormat(0, provider, format, arg0);

        public StringBuffer PrependFormat(IFormatProvider provider, string format, object arg0, object arg1) => InsertFormat(0, provider, format, arg0, arg1);

        public StringBuffer PrependFormat(IFormatProvider provider, string format, object arg0, object arg1, object arg2) => InsertFormat(0, provider, format, arg0, arg1, arg2);

        public StringBuffer PrependFormat(IFormatProvider provider, string format, params object[] args) => InsertFormat(0, provider, format, args);

        public StringBuffer InsertFormat(int position, string format, object arg0)
        {
            string value = string.Format(format, arg0);

            Insert(position, format);

            return this;
        }

        public StringBuffer InsertFormat(int position, string format, object arg0, object arg1)
        {
            string value = string.Format(format, arg0, arg1);

            Insert(position, format);

            return this;
        }

        public StringBuffer InsertFormat(int position, string format, object arg0, object arg1, object arg2)
        {
            string value = string.Format(format, arg0, arg1, arg2);

            Insert(position, format);

            return this;
        }

        public StringBuffer InsertFormat(int position, string format, params object[] args)
        {
            string value = string.Format(format, args);

            Insert(position, format);

            return this;
        }

        public StringBuffer InsertFormat(int position, IFormatProvider provider, string format, object arg0)
        {
            string value = string.Format(provider, format, arg0);

            Insert(position, format);

            return this;
        }

        public StringBuffer InsertFormat(int position, IFormatProvider provider, string format, object arg0, object arg1)
        {
            string value = string.Format(provider, format, arg0, arg1);

            Insert(position, format);

            return this;
        }

        public StringBuffer InsertFormat(int position, IFormatProvider provider, string format, object arg0, object arg1, object arg2)
        {
            string value = string.Format(provider, format, arg0, arg1, arg2);

            Insert(position, format);

            return this;
        }

        public StringBuffer InsertFormat(int position, IFormatProvider provider, string format, params object[] args)
        {
            string value = string.Format(provider, format, args);

            Insert(position, format);

            return this;
        }


        // Remove
        public StringBuffer Remove(int position)
        {
            builder.Remove(position, 1);

            return this;
        }

        public StringBuffer Remove(int position, int amount)
        {
            builder.Remove(position, amount);

            return this;
        }

        public StringBuffer Clear()
        {
            builder.Clear();

            return this;
        }


        // Reverse
        public StringBuffer Reverse()
        {
            Reverse(0, builder.Length);

            return this;
        }

        public StringBuffer Reverse(int position, int amount)
        {
            int last = position + amount - 1;

            int half = position + (amount / 2);

            for (int first = position; first <= half; first++, last--)
                (builder[first], builder[last]) = (builder[last], builder[first]);

            return this;
        }


        // Replace
        public StringBuffer Replace(char oldValue, char newValue) => Replace(oldValue, newValue, 0, Length);

        public StringBuffer Replace(char oldValue, char newValue, int position, int amount)
        {
            ExceptionHelper.VerifyInRange(position, amount, 0, builder.Length, nameof(position), nameof(amount));

            for (int i = position; i < position + amount; i++)
            {
                if (builder[i] == oldValue)
                    builder[i] = newValue;
            }

            return this;
        }


        // Conversion
        public override string ToString() => builder.ToString();

        public string ToString(int position) => builder.ToString(position, Length - position);

        public string ToString(int position, int amount) => builder.ToString(position, amount);

        public char[] ToChars()
        {
            var chars = new char[Length];

            builder.CopyTo(0, chars, 0, Length);

            return chars;
        }

        public char[] ToChars(int position) => ToChars(position, Length - position);

        public char[] ToChars(int position, int amount)
        {
            ExceptionHelper.VerifyInRange(position, amount, 0, builder.Length, nameof(position), nameof(amount));

            char[] result = new char[amount];

            builder.CopyTo(position, result, 0, amount);

            return result;
        }


        // Enumerator
        public IEnumerator<char> GetEnumerator()
        {
            for (int i = 0; i < builder.Length; i++)
                yield return builder[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        // IEditableText<char>
        void IEditableText<char>.Insert(int position, char value) => Insert(position, value);

        void IEditableText<char>.Remove(int position, int amount) => Remove(position, amount);

        void IEditableText<char>.Clear() => Clear();


        // IEditableText<char, IEnumerable<char>>
        void IEditableText<char, IEnumerable<char>>.Append(IEnumerable<char> values) => Append(values);

        void IEditableText<char, IEnumerable<char>>.Prepend(IEnumerable<char> values) => Prepend(values);

        void IEditableText<char, IEnumerable<char>>.Insert(int position, IEnumerable<char> values) => Insert(position, values);


        // IEditableText<char, string>        
        void IEditableText<char, string>.Append(string values) => Append(values);

        void IEditableText<char, string>.Prepend(string values) => Prepend(values);

        void IEditableText<char, string>.Insert(int position, string values) => Insert(position, values);

        void IEditableText<char>.Append(char value) => Append(value);

        void IEditableText<char>.Prepend(char value) => Prepend(value);
    }
}