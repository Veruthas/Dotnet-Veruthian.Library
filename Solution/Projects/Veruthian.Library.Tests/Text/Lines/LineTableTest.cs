using System;
using System.Collections.Generic;
using System.Linq;
using Veruthian.Library.Text.Chars;
using Veruthian.Library.Text.Chars.Extensions;
using Veruthian.Library.Text.Lines;
using Veruthian.Library.Text.Lines.Extensions;
using Veruthian.Library.Text.Runes;
using Xunit;

namespace Veruthian.Library.Text.Lines.Test
{
    public class LineBuilder<U, S, L, B> : IEditableText<U, S>
        where S : IEnumerable<U>, IEditableText<U, S>
        where L : BaseLineTable<U, S>
        where B : IEditableText<U>
    {
        public S Value;

        public L Lines;

        Func<U, uint> getUtf32;

        ExtractText<S> extractor;

        B buffer;

        Func<B, S> getBufferItem;


        public LineEnding Ending => Lines.Ending;

        public LineBuilder(S value, L lines, B buffer, ExtractText<S> extractor, Func<U, uint> getUtf32, Func<B, S> getBufferItem)
        {
            this.Value = value;

            this.Lines = lines;

            this.buffer = buffer;

            this.extractor = extractor;

            this.getUtf32 = getUtf32;

            this.getBufferItem = getBufferItem;
        }


        public void Append(U value)
        {
            this.Value.Append(value);

            this.Lines.Append(value);
        }

        public void Append(S value)
        {
            this.Value.Append(value);

            this.Lines.Append(value);
        }


        public void AppendMultiple(IEnumerable<U> values)
        {
            foreach (var value in values)
                Append(value);
        }

        public void Prepend(U value)
        {
            this.Value.Prepend(value);

            this.Lines.Prepend(value);
        }

        public void Prepend(S value)
        {
            this.Value.Prepend(value);

            this.Lines.Prepend(value);
        }


        public void PrependMultiple(IEnumerable<U> values)
        {
            foreach (var value in values.Reverse())
                Prepend(value);
        }


        public void Insert(int position, U value)
        {
            this.Value.Insert(position, value);

            this.Lines.Insert(position, value);
        }

        public void Insert(int position, S value)
        {
            this.Value.Insert(position, value);

            this.Lines.Insert(position, value);
        }

        public void InsertMultiple(int position, IEnumerable<U> values)
        {
            foreach (var value in values)
            {
                Insert(position++, value);
            }
        }

        public void InsertMultipleReversed(int position, IEnumerable<U> values)
        {
            foreach (var value in values.Reverse())
                Insert(position, value);
        }

        public void Remove(int position, int amount)
        {
            this.Value.Remove(position, amount);

            this.Lines.Remove(position, amount);
        }

        public void RemoveMultiple(int position, int amount)
        {
            for (int i = 0; i < amount; i++)
                Remove(position++, 1);
        }

        public void RemoveMultipleReversed(int position, int amount)
        {

            for (int i = position + amount - 1; i >= position; i--)
                Remove(i, 1);
        }

        public void Clear()
        {
            Value.Clear();

            Lines.Clear();
        }


        public void WriteBuilder()
        {
            Console.WriteLine($"'{Value.ToString().ToPrintableString()}', {Value.ToString().Length}");

            Console.WriteLine("Split");

            foreach (var line in Value.ToString().GetLineData(Ending))
            {
                var segment = line.Segment.ToTupleString();

                Console.WriteLine("{0}{1}-> '{2}'", segment, new string(' ', 20 - segment.Length), line.Value.ToPrintableString());
            }

            Console.WriteLine("-------");

            Console.WriteLine("Table");
            foreach (var line in Lines.Lines)
            {
                Console.WriteLine(line.ToTupleString());
            }

            Console.WriteLine();
        }


        public void Compare()
        {
            var tableLines = Lines.Lines.ExtractLineData(Value, extractor).ToArray();

            var splitLines = TextSegment.GetLineData(Value, getUtf32, buffer, getBufferItem, Ending).ToArray();

            Assert.Equal(tableLines.Length, splitLines.Length);

            for (var i = 0; i < tableLines.Length; i++)
                {
                var tableLine = tableLines[i];
                var splitLine = splitLines[i];
            }
    }
    }


    public class CharLineBuilder : LineBuilder<char, EditableString, CharLineTable<EditableString>, StringBuffer>
    {
        public static EditableString Extract(EditableString value, int position, int amount) => value.Value.Substring(position, amount);

        public CharLineBuilder(LineEnding ending)
            : base(new EditableString(), new CharLineTable<EditableString>(ending), new StringBuffer(), Extract, (c => (uint)c), (b => b.ToString())) { }
    }

    public class RuneLineBuilder : LineBuilder<Rune, EditableRuneString, RuneLineTable<EditableRuneString>, RuneBuffer>
    {
        public static EditableRuneString Extract(EditableRuneString value, int position, int amount) => value.Value.Extract(position, amount);

        public RuneLineBuilder(LineEnding ending)
            : base(new EditableRuneString(), new RuneLineTable<EditableRuneString>(ending), new RuneBuffer(), Extract, (r => (uint)r), (b => b.ToRuneString())) { }
    }


    public static class CharLineTester
    {
        static Dictionary<string, Action<CharLineBuilder>> actions = new Dictionary<string, Action<CharLineBuilder>>();

        static string SimpleTestString = "Hello, world!\r\nHow are you?\nI am fine\rThat is good";
        static CharLineTester()
        {
            actions.Add("Append", b => b.Append(SimpleTestString));

            actions.Add("AppendMultiple", b => b.AppendMultiple(SimpleTestString));

            actions.Add("Prepend", b => b.Prepend(SimpleTestString));

            actions.Add("PrependMultiple", b => b.PrependMultiple(SimpleTestString));

            actions.Add("Insert", b => b.Insert(0, SimpleTestString));

            actions.Add("InsertMultiple", b => b.InsertMultiple(0, SimpleTestString));

            actions.Add("InsertMultipleReversed", b => b.InsertMultipleReversed(0, SimpleTestString));

            actions.Add("BreakNewLineTest", b => { b.Append("Hello\rWorld\nMy\r\n"); b.Insert(15, "name is Veruthas!"); b.Insert(11, "!!\r"); b.Insert(6, "\n, "); });
        }

        [InlineData("Append", "None")]
        [InlineData("Append", "Cr")]
        [InlineData("Append", "Lf")]
        [InlineData("Append", "LfCr")]
        [InlineData("AppendMultiple", "None")]
        [InlineData("AppendMultiple", "Cr")]
        [InlineData("AppendMultiple", "Lf")]
        [InlineData("AppendMultiple", "LfCr")]
        [InlineData("Prepend", "None")]
        [InlineData("Prepend", "Cr")]
        [InlineData("Prepend", "Lf")]
        [InlineData("Prepend", "LfCr")]
        [InlineData("PrependMultiple", "None")]
        [InlineData("PrependMultiple", "Cr")]
        [InlineData("PrependMultiple", "Lf")]
        [InlineData("PrependMultiple", "LfCr")]
        [InlineData("Insert", "None")]
        [InlineData("Insert", "Cr")]
        [InlineData("Insert", "Lf")]
        [InlineData("Insert", "LfCr")]
        [InlineData("InsertMultiple", "None")]
        [InlineData("InsertMultiple", "Cr")]
        [InlineData("InsertMultiple", "Lf")]
        [InlineData("InsertMultiple", "LfCr")]
        [InlineData("InsertMultipleReversed", "None")]
        [InlineData("InsertMultipleReversed", "Cr")]
        [InlineData("InsertMultipleReversed", "Lf")]
        [InlineData("InsertMultipleReversed", "LfCr")]
        [InlineData("BreakNewLineTest", "None")]
        [InlineData("BreakNewLineTest", "Cr")]
        [InlineData("BreakNewLineTest", "Lf")]
        [InlineData("BreakNewLineTest", "LfCr")]
        [Theory]
        public static void TestLines(string action, string ending)
        {
            LineEnding.TryGetByName(ending, out var lineEnding);

            var builder = new CharLineBuilder(lineEnding);

            var actionFunc = actions[action];

            actionFunc(builder);

            builder.Compare();
        }
    }
}