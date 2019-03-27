using System.Collections.Generic;
using System.Linq;
using Veruthian.Library.Text.Chars;
using Xunit;

namespace Veruthian.Library.Text.Lines
{
    public class LineBuilder<U, S, L> : IEditableText<U, S>
        where S : IEnumerable<U>, IEditableText<U, S>
        where L : BaseLineTable<U, S>
    {
        public readonly S Value;

        public readonly L Lines;

        public LineBuilder(S value, L lines)
        {
            this.Value = value;

            this.Lines = lines;
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
    }

    public enum LineCommandType
    {
        AppendUnit,

        AppendMultiple,

        AppendString,

        PrependUnit,

        PrependMultiple,

        PrependString,

        InsertUnit,

        InsertMultiple,

        InsertString,

        Remove
    }

    public class LineCommand<U, S>
        where S : IEnumerable<U>, IEditableText<U, S>
    {
        LineCommandType type;

        int position;

        int amount;

        U unit;

        S multiple;

        S[] expected;


        public LineCommand(S[] expected, LineCommandType type, int position = 0, int amount = 0, U unit = default(U), S multiple = default(S))
        {
            this.expected = expected;

            this.type = type;

            this.position = position;

            this.amount = amount;

            this.unit = unit;

            this.multiple = multiple;
        }

        public void Test<L>(LineBuilder<U, S, L> builder, SliceText<S> slice)
            where L : BaseLineTable<U, S>
        {
            switch (type)
            {
                case LineCommandType.AppendUnit:
                    builder.Append(unit);
                    break;
                case LineCommandType.AppendMultiple:
                    builder.Append(multiple);
                    break;
                case LineCommandType.PrependUnit:
                    builder.Prepend(unit);
                    break;
                case LineCommandType.PrependMultiple:
                    builder.Prepend(multiple);
                    break;
                case LineCommandType.InsertUnit:
                    builder.Insert(position, unit);
                    break;
                case LineCommandType.InsertMultiple:
                    builder.Insert(position, multiple);
                    break;
                case LineCommandType.Remove:
                    builder.Remove(position, amount);
                    break;
            }

            S[] result = builder.Lines.Extract(builder.Value, slice).ToArray();

            Assert.Equal(expected.Length, result.Length);

            for (int i = 0; i < expected.Length; i++)
                Assert.Equal(expected[i], result[i]);
        }


        public LineCommand<U, S> Append(S[] expected, U value) => new LineCommand<U, S>(expected, LineCommandType.AppendUnit, unit: value);

        public LineCommand<U, S> Append(S[] expected, S value) => new LineCommand<U, S>(expected, LineCommandType.AppendMultiple, multiple: value);


        public LineCommand<U, S> Prepend(S[] expected, U value) => new LineCommand<U, S>(expected, LineCommandType.PrependUnit, unit: value);

        public LineCommand<U, S> Prepend(S[] expected, S value) => new LineCommand<U, S>(expected, LineCommandType.PrependMultiple, multiple: value);



        public LineCommand<U, S> Insert(S[] expected, int position, U value) => new LineCommand<U, S>(expected, LineCommandType.InsertUnit, position: position, unit: value);

        public LineCommand<U, S> Insert(S[] expected, int position, S value) => new LineCommand<U, S>(expected, LineCommandType.InsertMultiple, position: position, multiple: value);


        public LineCommand<U, S> Remove(S[] expected, int position, int amount) => new LineCommand<U, S>(expected, LineCommandType.Remove, position: position, amount: amount);
    }

    public class ExpectedResults
    {

    }

}