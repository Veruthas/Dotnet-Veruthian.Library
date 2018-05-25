using System;
using System.Collections.Generic;
using Veruthian.Dotnet.Library.Text.Code;
using Xunit;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public class ReadersTest
    {
        [InlineData("")]
        [InlineData("Hello")]
        [Theory]
        public void TestSimpleReader(string data)
        {
            var codes = data.ToCodePoints();

            var reader = codes.GetSimpleReader();

            foreach(var c in data)
            {
                Assert.Equal(c, reader.Read());                
            }

            Assert.Equal(reader.Read(), new char());
            Assert.True(reader.IsEnd);
        }
    }
}