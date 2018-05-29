using System;
using System.Collections.Generic;
using Veruthian.Dotnet.Library.Text.Code;
using Veruthian.Dotnet.Library.Data.Enumeration;
using Xunit;
using Xunit.Abstractions;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public abstract class ReaderTest<T, TReader>
        where TReader : IReader<T>
    {
        protected abstract TReader GetReader(T[] data);

        protected void TestReaderReadAndPeek(T[] data)
        {
            var reader = GetReader(data);

            for (int i = 0; i < data.Length; i++)
            {
                var c = data[i];

                // Make sure positions are the same
                Assert.Equal(i, reader.Position);

                // Every item should equal on peek                
                Assert.Equal(c, reader.Peek());

                // Every item should equal on read
                Assert.Equal(c, reader.Read());
            }

            // End of reader reached
            Assert.Equal(data.Length, reader.Position);

            Assert.Equal(default(T), reader.Peek());

            Assert.Equal(default(T), reader.Read());

            Assert.Equal(data.Length, reader.Position);

            // Assert is end of reader
            Assert.True(reader.IsEnd);
        }

        protected void TestReaderSkip(T[] data, int skipInterval)
        {
            // Negative skip intervals correspond to data.Length + |skip|
            if (skipInterval <= 0)
                skipInterval = data.Length - skipInterval;

            var reader = GetReader(data);

            int current = 0;

            do
            {
                var skipped = reader.Skip(skipInterval);
                
                current += skipInterval;

                if (!reader.IsEnd)
                {
                    // Confirm amount skipped was actually skilled
                    Assert.Equal(skipInterval, skipped);

                    // Position should equal amount we skipped
                    Assert.Equal(current, reader.Position);

                    // We should be at same item in array and reader
                    Assert.Equal(data[current], reader.Peek());
                }
                else
                {
                    // Get the skip attempt difference
                    var skipDelta = current - data.Length;

                    // Make sure the amount skipped was as it should be
                    Assert.Equal(skipInterval - skipDelta, skipped);
                }

            } while (!reader.IsEnd);

            // Make sure we are at end of reader
            Assert.Equal(data.Length, reader.Position);

            Assert.Equal(default(T), reader.Peek());
        }
    }

    public abstract class LookaheadReaderTest<T, TReader> : ReaderTest<T, TReader>
        where TReader : ILookaheadReader<T>
    {
    }

    public abstract class SpeculativeReaderTest<T, TReader> : LookaheadReaderTest<T, TReader>
        where TReader : ISpeculativeReader<T>
    {
    }



    public class SimpleReaderTest : ReaderTest<CodePoint, SimpleReader<CodePoint>>
    {
        protected override SimpleReader<CodePoint> GetReader(CodePoint[] data)
        {
            return data.GetSimpleReader();
        }

        [Theory]
        [InlineData("")]
        [InlineData("Hello, world!")]
        public void TestReadAndPeek(string data) => TestReaderReadAndPeek(data.ToCodePointArray());


        [Theory]
        [InlineData("", 0)]
        [InlineData("", 1)]
        [InlineData("Hello, world!", 0)]
        [InlineData("Hello, world!", -1)]
        [InlineData("Hello, world!", 1)]
        [InlineData("Hello, world!", 2)]
        public void TestSkip(string data, int skipInterval) => TestReaderSkip(data.ToCodePointArray(), skipInterval);
    }

    public class FixedLookaheadReaderTest : LookaheadReaderTest<CodePoint, FixedLookaheadReader<CodePoint>>
    {
        protected override FixedLookaheadReader<CodePoint> GetReader(CodePoint[] data)
        {
            return data.GetFixedLookaheadReader();
        }

        [Theory]
        [InlineData("")]
        [InlineData("A")]
        [InlineData("AB")]
        [InlineData("Hello, world!")]
        public void TestReadAndPeek(string data) => TestReaderReadAndPeek(data.ToCodePointArray());

        [Theory]
        [InlineData("", 0)]
        [InlineData("", 1)]
        [InlineData("Hello, world!", 0)]
        [InlineData("Hello, world!", -1)]
        [InlineData("Hello, world!", 1)]
        [InlineData("Hello, world!", 2)]
        public void TestSkip(string data, int skipInterval) => TestReaderSkip(data.ToCodePointArray(), skipInterval);
    }

    public class VariableLookaheadReaderTest : LookaheadReaderTest<CodePoint, VariableLookaheadReaderBase<CodePoint>>
    {
        protected override VariableLookaheadReaderBase<CodePoint> GetReader(CodePoint[] data)
        {
            return data.GetVariableLookaheadReader();
        }

        [Theory]
        [InlineData("")]
        [InlineData("Hello, world!")]
        public void TestReadAndPeek(string data) => TestReaderReadAndPeek(data.ToCodePointArray());

        [Theory]
        [InlineData("", 0)]
        [InlineData("", 1)]
        [InlineData("Hello, world!", 0)]
        [InlineData("Hello, world!", -1)]
        [InlineData("Hello, world!", 1)]
        [InlineData("Hello, world!", 2)]
        public void TestSkip(string data, int skipInterval) => TestReaderSkip(data.ToCodePointArray(), skipInterval);
    }

    public class SpeculativeReaderTest : SpeculativeReaderTest<CodePoint, SpeculativeReader<CodePoint>>
    {
        protected override SpeculativeReader<CodePoint> GetReader(CodePoint[] data)
        {
            return data.GetSpeculativeReader();
        }

        [Theory]
        [InlineData("")]
        [InlineData("Hello, world!")]
        public void TestReadAndPeek(string data) => TestReaderReadAndPeek(data.ToCodePointArray());

        [Theory]
        [InlineData("", 0)]
        [InlineData("", 1)]
        [InlineData("Hello, world!", 0)]
        [InlineData("Hello, world!", -1)]
        [InlineData("Hello, world!", 1)]
        [InlineData("Hello, world!", 2)]
        public void TestSkip(string data, int skipInterval) => TestReaderSkip(data.ToCodePointArray(), skipInterval);
    }
}