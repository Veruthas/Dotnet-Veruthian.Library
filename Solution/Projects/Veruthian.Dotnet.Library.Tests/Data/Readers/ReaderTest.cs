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
        protected void TestReaderReadAndPeek(TReader reader, T[] data)
        {

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

        protected void TestReaderSkip(TReader reader, T[] data, int skipInterval)
        {
            // Negative skip intervals correspond to data.Length + |skip|
            if (skipInterval <= 0)
                skipInterval = data.Length - skipInterval;

            int current = 0;

            do
            {
                reader.Skip(skipInterval);

                current += skipInterval;

                if (!reader.IsEnd)
                {
                    // Position should equal amount we skipped
                    Assert.Equal(current, reader.Position);

                    // We should be at same item in array and reader
                    Assert.Equal(data[current], reader.Peek());
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
        private SimpleReader<CodePoint> GetReader(CodePoint[] data)
        {
            return data.GetSimpleReader();
        }

        [Theory]
        [InlineData("")]
        [InlineData("Hello, world!")]
        public void TestReadAndPeek(string data)
        {
            var codes = data.ToCodePointArray();
            TestReaderReadAndPeek(GetReader(codes), codes);
        }


        [Theory]
        [InlineData("", 0)]
        [InlineData("", 1)]
        [InlineData("Hello, world!", 0)]
        [InlineData("Hello, world!", -1)]
        [InlineData("Hello, world!", 1)]
        [InlineData("Hello, world!", 2)]
        public void TestSkip(string data, int skipInterval)
        {
            var codes = data.ToCodePointArray();
            TestReaderSkip(GetReader(codes), codes, skipInterval);
        }
    }

    public class FixedLookaheadReaderTest : LookaheadReaderTest<CodePoint, FixedLookaheadReader<CodePoint>>
    {
        private FixedLookaheadReader<CodePoint> GetReader(CodePoint[] data)
        {
            return data.GetFixedLookaheadReader(1);
        }

        [Theory]
        [InlineData("")]
        [InlineData("A")]
        [InlineData("AB")]
        [InlineData("Hello, world!")]
        public void TestReadAndPeek(string data)
        {
            var codes = data.ToCodePointArray();
            TestReaderReadAndPeek(GetReader(codes), codes);
        }


        [Theory]
        [InlineData("", 0)]
        [InlineData("", 1)]
        [InlineData("Hello, world!", 0)]
        [InlineData("Hello, world!", -1)]
        [InlineData("Hello, world!", 1)]
        [InlineData("Hello, world!", 2)]
        public void TestSkip(string data, int skipInterval)
        {
            var codes = data.ToCodePointArray();
            TestReaderSkip(GetReader(codes), codes, skipInterval);
        }
    }

    public class VariableLookaheadReaderTest : LookaheadReaderTest<CodePoint, VariableLookaheadReaderBase<CodePoint>>
    {
        private VariableLookaheadReaderBase<CodePoint> GetReader(CodePoint[] data)
        {
            return data.GetVariableLookaheadReader();
        }

        [Theory]
        [InlineData("")]
        [InlineData("Hello, world!")]
        public void TestReadAndPeek(string data)
        {
            var codes = data.ToCodePointArray();
            TestReaderReadAndPeek(GetReader(codes), codes);
        }


        [Theory]
        [InlineData("", 0)]
        [InlineData("", 1)]
        [InlineData("Hello, world!", 0)]
        [InlineData("Hello, world!", -1)]
        [InlineData("Hello, world!", 1)]
        [InlineData("Hello, world!", 2)]
        public void TestSkip(string data, int skipInterval)
        {
            var codes = data.ToCodePointArray();
            TestReaderSkip(GetReader(codes), codes, skipInterval);
        }
    }

    public class SpeculativeReaderTest : SpeculativeReaderTest<CodePoint, SpeculativeReader<CodePoint>>
    {
        private SpeculativeReader<CodePoint> GetReader(CodePoint[] data)
        {
            return data.GetSpeculativeReader();
        }

        [Theory]
        [InlineData("")]
        [InlineData("Hello, world!")]
        public void TestReadAndPeek(string data)
        {
            var codes = data.ToCodePointArray();
            TestReaderReadAndPeek(GetReader(codes), codes);
        }


        [Theory]
        [InlineData("", 0)]
        [InlineData("", 1)]
        [InlineData("Hello, world!", 0)]
        [InlineData("Hello, world!", -1)]
        [InlineData("Hello, world!", 1)]
        [InlineData("Hello, world!", 2)]
        public void TestSkip(string data, int skipInterval)
        {
            var codes = data.ToCodePointArray();
            TestReaderSkip(GetReader(codes), codes, skipInterval);
        }
    }
}