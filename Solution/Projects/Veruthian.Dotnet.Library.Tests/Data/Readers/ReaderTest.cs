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
            TestIsAtEnd(reader, data);
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
            TestIsAtEnd(reader, data);
        }

        protected void BaseTestReadEnumerable(TReader reader, T[] data)
        {
            int i = 0;

            foreach (var item in reader.Read(data.Length))
            {
                Assert.Equal(data[i++], item);
            }

            TestIsAtEnd(reader, data);
        }

        protected void TestIsAtEnd(TReader reader, T[] data)
        {
            Assert.Equal(data.Length, reader.Position);

            Assert.Equal(default(T), reader.Peek());

            Assert.Equal(default(T), reader.Read());

            Assert.Equal(data.Length, reader.Position);

            Assert.True(reader.IsEnd);
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


        public void BaseTestPeekEnumerable(TReader reader, T[] data, int lookahead = 0)
        {
            int index = 0;

            while (!reader.IsEnd)
            {
                foreach (var item in reader.Peek(0, lookahead))
                {
                    if (index >= data.Length)
                        break;

                    Assert.Equal(data[index++], item);
                }

                reader.Skip(lookahead);
            }

            TestIsAtEnd(reader, data);
        }
    }

    public abstract class SpeculativeReaderTest<T, TReader> : LookaheadReaderTest<T, TReader>
        where TReader : ISpeculativeReader<T>
    {
        protected void BaseTestPeekFromMark(TReader reader, T[] data)
        {
            reader.Mark();

            reader.Skip(data.Length);

            for (int i = 0; i < data.Length; i++)
            {
                var dataItem = data[i];

                var readerItem = reader.PeekFromMark(i);

                Assert.Equal(dataItem, readerItem);
            }

            int index = 0;

            foreach (var readerItem in reader.PeekFromMark(0, data.Length))
            {
                var dataItem = data[index++];

                Assert.Equal(dataItem, readerItem);
            }
        }
    }



    public class ReaderTest : ReaderTest<CodePoint, Reader<CodePoint>>
    {
        private Reader<CodePoint> GetReader(CodePoint[] data)
        {
            return data.GetReader();
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
        [InlineData("")]
        [InlineData("ABCDEFGHIJKLM")]
        public void TestReadEnumerable(string data)
        {
            var codes = data.ToCodePointArray();
            BaseTestReadEnumerable(GetReader(codes), codes);
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
        [InlineData("", 2)]
        [InlineData("ABCDEFGHIJKLM", 2)]
        public void TestReadEnumerable(string data, int lookahead)
        {
            var codes = data.ToCodePointArray();
            BaseTestReadEnumerable(GetReader(codes, lookahead), codes);
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

        [Theory]
        [InlineData("", 2, 0)]
        [InlineData("ABCD", 2, 2)]
        [InlineData("ABCDE", 4, 4)]
        [InlineData("ABCDE", 6, 5)]
        public void TestPeekEnumerable(string data, int lookahead, int chunkSize)
        {
            var codes = data.ToCodePointArray();

            var reader = GetReader(codes, lookahead);

            BaseTestPeekEnumerable(reader, codes, chunkSize);
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

        [Theory]
        [InlineData("")]
        [InlineData("ABCDEFGHIJKLM")]
        public void TestReadEnumerable(string data)
        {
            var codes = data.ToCodePointArray();
            BaseTestReadEnumerable(GetReader(codes), codes);
        }

        [Theory]
        [InlineData("", 4)]
        [InlineData("ABCD", 4)]
        [InlineData("ABCDEFGHIJKLM", 4)]
        public void TestLookahead(string data, int lookahead)
        {
            var codes = data.ToCodePointArray();

            var reader = GetReader(codes);

            BaseTestLookahead(reader, codes, lookahead);
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData("ABCD", 2)]
        [InlineData("ABCDE", 4)]
        [InlineData("ABCDE", 5)]
        public void TestPeekEnumerable(string data, int lookahead)
        {
            var codes = data.ToCodePointArray();

            var reader = GetReader(codes);

            BaseTestPeekEnumerable(reader, codes, lookahead);
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
        [InlineData("")]
        [InlineData("ABCDEFGHIJKLM")]
        public void TestReadEnumerable(string data)
        {
            var codes = data.ToCodePointArray();

            BaseTestReadEnumerable(GetReader(codes), codes);
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

        [Theory]
        [InlineData("", 4)]
        [InlineData("ABCD", 4)]
        [InlineData("ABCDEFGHIJKLM", 4)]
        public void TestLookahead(string data, int lookahead)
        {
            var codes = data.ToCodePointArray();

            var reader = GetReader(codes);

            BaseTestLookahead(reader, codes, lookahead);
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData("ABCD", 2)]
        [InlineData("ABCDE", 4)]
        [InlineData("ABCDE", 5)]
        public void TestPeekEnumerable(string data, int lookahead)
        {
            var codes = data.ToCodePointArray();

            var reader = GetReader(codes);

            BaseTestPeekEnumerable(reader, codes, lookahead);
        }


        [Theory]
        [InlineData("")]
        [InlineData("ABCDE")]
        public void TestPeekFromMark(string data)
        {
            var codes = data.ToCodePointArray();

            var reader = GetReader(codes);
            
            BaseTestPeekFromMark(reader, codes);
        }
    }
}