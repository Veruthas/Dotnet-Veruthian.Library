using System;
using System.Collections.Generic;
using Veruthian.Dotnet.Library.Text.Code;
using Veruthian.Dotnet.Library.Data.Enumeration;
using Xunit;
using Xunit.Abstractions;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public class ReadersTest
    {
        private readonly ITestOutputHelper Output;

        public ReadersTest(ITestOutputHelper output) => Output = output;


        static List<Func<CodeString, IReader<CodePoint>>> IReaderCreators = new List<Func<CodeString, IReader<CodePoint>>>{
            (codes) => codes.GetSimpleReader(),
            (codes) => codes.GetFixedLookaheadReader(),
            (codes) => codes.GetVariableLookaheadReader(),
            (codes) => codes.GetSpeculativeReader(),
            (codes) => codes.GetSpeculativeWithStateReader<CodePoint, Object>(),
        };

        public static TheoryData<IReader<CodePoint>, CodeString> GenerateIReaders(string value)
        {
            var data = new TheoryData<IReader<CodePoint>, CodeString>();

            var codes = value.ToCodeString();

            foreach (var creator in IReaderCreators)
            {
                data.Add(creator(codes), codes);
            }

            return data;
        }

        [Theory]
        [MemberData(nameof(GenerateIReaders), parameters: "")]
        [MemberData(nameof(GenerateIReaders), parameters: "Hello")]
        public void TestIReaderReadAndPeek(IReader<CodePoint> reader, CodeString data)
        {
            foreach (var c in data)
            {
                // Every character should equal on peek                
                Assert.Equal(c, reader.Peek());

                // Every character should equal on read
                Assert.Equal(c, reader.Read());
            }

            // End of reader reached
            Assert.Equal(new char(), reader.Peek());

            Assert.Equal(new char(), reader.Read());

            // Assert is end of reader
            Assert.True(reader.IsEnd);
        }

        [Theory]
        [MemberData(nameof(GenerateIReaders), parameters: "")]
        [MemberData(nameof(GenerateIReaders), parameters: "Hello")]
        public void TestIReaderSkipAll(IReader<CodePoint> reader, CodeString data)
        {
            Assert.True(true);
        }
    }
}