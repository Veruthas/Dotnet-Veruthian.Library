using System;
using Xunit;

namespace Veruthian.Dotnet.Library.Data.Tables
{
    public class TablesTest
    {
        [Fact]
        public void TestSimpleTableAccess()
        {
            var table = Tables.CreateTable<string, string>();

            table.Set("A", "a");

            table.Set("B", "b");

            string value;

            Assert.True(table.Get("A", out value));

            Assert.True(table.Get("B", out value));
        }
    }
}