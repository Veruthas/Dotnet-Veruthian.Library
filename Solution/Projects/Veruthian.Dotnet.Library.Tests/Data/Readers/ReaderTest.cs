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
        protected void BaseTestReadAndPeek(TReader reader, T[] data)
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

        protected void BaseTestSkip(TReader reader, T[] data, int skipInterval)
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
        public void BaseTestLookahead(TReader reader, T[] data, int lookahead)
        {
            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < lookahead; j++)
                {
                    var dataItem = i + j >= data.Length ? default(T) : data[i + j];

                    var readerItem = reader.Peek(j);

                    Assert.Equal(dataItem, readerItem);
                }

                reader.Read();
            }
        }
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
        [InlineData("ABCDEFGHIJKLM")]
        public void TestReadAndPeek(string data)
        {
            var codes = data.ToCodePointArray();
            BaseTestReadAndPeek(GetReader(codes), codes);
        }


        [Theory]
        [InlineData("", 0)]
        [InlineData("", 1)]
        [InlineData("ABCDEFGHIJKLM", 0)]
        [InlineData("ABCDEFGHIJKLM", -1)]
        [InlineData("ABCDEFGHIJKLM", 1)]
        [InlineData("ABCDEFGHIJKLM", 2)]
        public void TestSkip(string data, int skipInterval)
        {
            var codes = data.ToCodePointArray();
            BaseTestSkip(GetReader(codes), codes, skipInterval);
        }
    }

    public class FixedLookaheadReaderTest : LookaheadReaderTest<CodePoint, FixedLookaheadReader<CodePoint>>
    {
        private FixedLookaheadReader<CodePoint> GetReader(CodePoint[] data, int lookahead)
        {
            return data.GetFixedLookaheadReader(lookahead);
        }

        [Fact]
        public void TestZeroLookahead()
        {
            Assert.ThrowsAny<Exception>(() => GetReader("".ToCodePointArray(), 0));
        }


        [Theory]
        [InlineData("", 2)]
        [InlineData("A", 1)]
        [InlineData("AB", 1)]
        [InlineData("ABCDEFGHIJKLM", 2)]
        [InlineData("ABCDEFGHIJKLM", 20)]
        public void TestReadAndPeek(string data, int lookahead)
        {
            var codes = data.ToCodePointArray();
            BaseTestReadAndPeek(GetReader(codes, lookahead), codes);
        }


        [Theory]
        [InlineData("", 2, 0)]
        [InlineData("", 2, 1)]
        [InlineData("ABCDEFGHIJKLM", 1, 1)]
        [InlineData("ABCDEFGHIJKLM", 2, -1)]
        [InlineData("ABCDEFGHIJKLM", 2, 0)]
        [InlineData("ABCDEFGHIJKLM", 2, 1)]
        [InlineData("ABCDEFGHIJKLM", 2, 2)]
        [InlineData("ABCDEFGHIJKLM", 2, 3)]
        public void TestSkip(string data, int lookahead, int skipInterval)
        {
            var codes = data.ToCodePointArray();
            BaseTestSkip(GetReader(codes, lookahead), codes, skipInterval);
        }

        [Theory]
        [InlineData("", 4)]
        [InlineData("ABCD", 4)]
        [InlineData("ABCDEFGHIJKLM", 4)]
        public void TestLookahead(string data, int lookahead)
        {
            var codes = data.ToCodePointArray();

            var reader = GetReader(codes, lookahead);

            BaseTestLookahead(reader, codes, lookahead);
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
        [InlineData("ABCDEFGHIJKLM")]
        public void TestReadAndPeek(string data)
        {
            var codes = data.ToCodePointArray();
            BaseTestReadAndPeek(GetReader(codes), codes);
        }


        [Theory]
        [InlineData("", 0)]
        [InlineData("", 1)]
        [InlineData("ABCDEFGHIJKLM", 0)]
        [InlineData("ABCDEFGHIJKLM", -1)]
        [InlineData("ABCDEFGHIJKLM", 1)]
        [InlineData("ABCDEFGHIJKLM", 2)]
        public void TestSkip(string data, int skipInterval)
        {
            var codes = data.ToCodePointArray();
            BaseTestSkip(GetReader(codes), codes, skipInterval);
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
        [InlineData("ABCDEFGHIJKLM")]
        public void TestReadAndPeek(string data)
        {
            var codes = data.ToCodePointArray();
            BaseTestReadAndPeek(GetReader(codes), codes);
        }


        [Theory]
        [InlineData("", 0)]
        [InlineData("", 1)]
        [InlineData("ABCDEFGHIJKLM", 0)]
        [InlineData("ABCDEFGHIJKLM", -1)]
        [InlineData("ABCDEFGHIJKLM", 1)]
        [InlineData("ABCDEFGHIJKLM", 2)]
        public void TestSkip(string data, int skipInterval)
        {
            var codes = data.ToCodePointArray();
            BaseTestSkip(GetReader(codes), codes, skipInterval);
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData("", 1)]
        [InlineData("ABCDEFGHIJKLM", 0)]
        [InlineData("ABCDEFGHIJKLM", -1)]
        [InlineData("ABCDEFGHIJKLM", 1)]
        [InlineData("ABCDEFGHIJKLM", 2)]
        public void TestRollbackSkip(string data, int skipInterval)
        {
            var codes = data.ToCodePointArray();

            var reader = GetReader(codes);

            reader.Mark();

            while (!reader.IsEnd)
                reader.Read();

            reader.Rollback();

            BaseTestSkip(reader, codes, skipInterval);
        }
    }
}