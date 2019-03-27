using System.Collections.Generic;
using Veruthian.Library.Text.Chars;

namespace Veruthian.Library.Text.Lines
{
    public class CharLineBuilder
    {
        public string value;

        public CharLineTable lines;


        public CharLineBuilder(LineEnding ending)
        {
            value = string.Empty;

            lines = new CharLineTable(ending);
        }


        public void Append(char value)
        {
            this.value += value;

            this.lines.Append(value);
        }

        public void Append(string value)
        {
            this.value += value;

            this.lines.Append(value);
        }


        public void Prepend(char value)
        {
            this.value = value + this.value;

            this.lines.Prepend(value);
        }

        public void Prepend(string value)
        {
            this.value = value + this.value;

            this.lines.Prepend(value);
        }


        public void Insert(int position, char value)
        {
            this.value = this.value.Insert(position, value.ToString());

            this.lines.Insert(position, value);
        }

        public void Insert(int position, string value)
        {
            this.value = this.value.Insert(position, value);

            this.lines.Insert(position, value);
        }


        public void Remove(int position, int amount)
        {
            this.value = this.value.Remove(position, amount);

            this.lines.Remove(position, amount);
        }
    }

    public enum LineCommandType
    {
        Append,

        Prepend,

        Insert,

        Remove
    }

    public class LineCommand
    {
        LineCommandType Type;

        int position;

        int amount;

        char value;

        string values;

        string[] ExpectedLines;
    }
}