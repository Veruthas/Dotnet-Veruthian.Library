using System;
using Soedeum.Dotnet.Library.Data.Ranges;
using Soedeum.Dotnet.Library.Data.Readers;

namespace Soedeum.Dotnet.Library.Data.Patterns
{
    public abstract class Matcher<T, TReader, TState> : IMatcher<T, TReader, TState>
        where TReader : IReader<T>
    {
        readonly Preprocess preprocess;
        readonly Postprocess postprocess;

        public Matcher(Preprocess preprocess = null, Postprocess postprocess = null)
        {
            this.preprocess = preprocess;

            this.postprocess = postprocess;
        }


        public virtual bool Match(TReader reader, TState state)
        {
            if (preprocess != null)
                preprocess(reader, state);

            bool result = Process(reader, state);

            if (postprocess != null)
                postprocess(reader, state, result);

            return result;
        }

        public abstract bool Process(TReader reader, TState state);

        public delegate void Preprocess(TReader reader, TState state);

        public delegate void Postprocess(TReader reader, TState state, bool success);
    }

    public class EqualityMatcher<T, TReader, TState> : Matcher<T, TReader, TState>
        where TReader : IReader<T>
        where T : IEquatable<T>
    {
        readonly T item;

        public EqualityMatcher(T item, Preprocess preprocess = null, Postprocess postprocess = null)
            : base(preprocess, postprocess)
        {
            this.item = item;
        }

        public override bool Process(TReader reader, TState state)
        {
            var current = reader.Peek();

            return item != null ? item.Equals(current) : false;
        }
    }

    public class RangeMatcher<T, TReader, TState> : Matcher<T, TReader, TState>
        where TReader : IReader<T>
        where T : IOrderable<T>, new()
    {
        readonly Range<T> range;

        public RangeMatcher(Range<T> range, Preprocess preprocess = null, Postprocess postprocess = null)
            : base(preprocess, postprocess)
        {
            this.range = range;
        }

        public override bool Process(TReader reader, TState state)
        {
            var current = reader.Peek();

            var result = current != null ? range.Contains(current) : false;

            reader.Read();
        }
    }

    public class RangeSetMatcher<T, TReader, TState> : Matcher<T, TReader, TState>
        where TReader : IReader<T>
        where T : IOrderable<T>, new()
    {
        readonly RangeSet<T> set;

        public RangeSetMatcher(RangeSet<T> set, Preprocess preprocess = null, Postprocess postprocess = null)
            : base(preprocess, postprocess)
        {
            this.set = set;
        }

        public override bool Process(TReader reader, TState state)
        {
            var current = reader.Peek();

            return current != null ? set.Contains(current) : false;
        }
    }
}