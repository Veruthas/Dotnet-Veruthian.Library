using System;
using System.Collections.Generic;
using System.Linq;
using Veruthian.Library.Text.Chars;
using Veruthian.Library.Text.Lines;
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

        SliceText<S> slicer;

        B builder;

        Func<U, uint> getUtf32;

        Func<B, S> getItem;


        public LineBuilder(S value, L lines, B builder, SliceText<S> slicer, Func<U, uint> getUtf32, Func<B, S> getItem)
        {
            this.Value = value;

            this.Lines = lines;

            this.builder = builder;

            this.slicer = slicer;

            this.getUtf32 = getUtf32;

            this.getItem = getItem;
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


        public void Compare(bool keepEndings)
        {
            var tableLines = Lines.Extract(Value, slicer, keepEndings).ToArray();

            var splitLines = LineEnding.GetLines(Value, Lines.EndingType, keepEndings, builder, getUtf32, getItem).ToArray();


            Assert.Equal(tableLines.Length, splitLines.Length);

            for (int i = 0; i < tableLines.Length; i++)
                Assert.Equal(tableLines[i], splitLines[i]);
        }
    }


    public class CharLineBuilder : LineBuilder<char, EditableString, CharLineTable<EditableString>, StringBuffer>
    {
        public static EditableString Slice(EditableString value, int position, int amount) => value.Value.Substring(position, amount);

        public CharLineBuilder(LineEnding ending)
            : base(new EditableString(), new CharLineTable<EditableString>(ending), new StringBuffer(), Slice, (c => (uint)c), (b => b.ToString())) { }
    }

    public class RuneLineBuilder : LineBuilder<Rune, EditableRuneString, RuneLineTable<EditableRuneString>, RuneBuffer>
    {
        public static EditableRuneString Slice(EditableRuneString value, int position, int amount) => value.Value.Slice(position, amount);

        public RuneLineBuilder(LineEnding ending)
            : base(new EditableRuneString(), new RuneLineTable<EditableRuneString>(ending), new RuneBuffer(), Slice, (r => (uint)r), (b => b.ToRuneString())) { }
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

        [InlineData("Append", "None", true)]
        [InlineData("Append", "Cr", true)]
        [InlineData("Append", "Lf", true)]
        [InlineData("Append", "LfCr", true)]
        [InlineData("AppendMultiple", "None", true)]
        [InlineData("AppendMultiple", "Cr", true)]
        [InlineData("AppendMultiple", "Lf", true)]
        [InlineData("AppendMultiple", "LfCr", true)]
        [InlineData("Prepend", "None", true)]
        [InlineData("Prepend", "Cr", true)]
        [InlineData("Prepend", "Lf", true)]
        [InlineData("Prepend", "LfCr", true)]
        [InlineData("PrependMultiple", "None", true)]
        [InlineData("PrependMultiple", "Cr", true)]
        [InlineData("PrependMultiple", "Lf", true)]
        [InlineData("PrependMultiple", "LfCr", true)]
        [InlineData("Insert", "None", true)]
        [InlineData("Insert", "Cr", true)]
        [InlineData("Insert", "Lf", true)]
        [InlineData("Insert", "LfCr", true)]
        [InlineData("InsertMultiple", "None", true)]
        [InlineData("InsertMultiple", "Cr", true)]
        [InlineData("InsertMultiple", "Lf", true)]
        [InlineData("InsertMultiple", "LfCr", true)]
        [InlineData("InsertMultipleReversed", "None", true)]
        [InlineData("InsertMultipleReversed", "Cr", true)]
        [InlineData("InsertMultipleReversed", "Lf", true)]
        [InlineData("InsertMultipleReversed", "LfCr", true)]
        [InlineData("BreakNewLineTest", "None", true)]
        [InlineData("BreakNewLineTest", "Cr", true)]
        [InlineData("BreakNewLineTest", "Lf", true)]
        [InlineData("BreakNewLineTest", "LfCr", true)]
        [Theory]
        public static void TestLines(string action, string ending, bool keepEnd)
        {
            LineEnding.TryGetByName(ending, out var lineEnding);

            var builder = new CharLineBuilder(lineEnding);

            var actionFunc = actions[action];

            actionFunc(builder);

            builder.Compare(keepEnd);
        }
    }
}