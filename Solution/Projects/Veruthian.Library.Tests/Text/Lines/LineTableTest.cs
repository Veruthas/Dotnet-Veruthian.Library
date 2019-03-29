using System;
using System.Collections.Generic;
using System.Linq;
using Veruthian.Library.Text.Chars;
using Veruthian.Library.Text.Lines;
using Veruthian.Library.Text.Runes;
using Xunit;

namespace Veruthian.Library.Text.Lines
{
    public class LineBuilder<U, S, L, B> : IEditableText<U, S>
        where S : IEnumerable<U>, IEditableText<U, S>
        where L : BaseLineTable<U, S>
        where B : IEditableText<U>
    {
        S Value;

        L Lines;

        SliceText<S> slicer;

        B builder;

        Func<U, uint> getUtf32;

        Func<B, S> getItem;


        public LineBuilder(S value, L lines, B builder, SliceText<S> slicer, Func<U, uint> getUtf32, Func<B, S> getItem)
        {
            this.Value = value;

            this.Lines = lines;

            this.builder = builder;

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


        public void Remove(int position, int amount)
        {
            this.Value.Remove(position, amount);

            this.Lines.Remove(position, amount);
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

        static CharLineTester()
        {
            actions.Add("Append", b => b.Append("Hello, world!"));
        }

        [InlineData("Append", "None", true)]
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