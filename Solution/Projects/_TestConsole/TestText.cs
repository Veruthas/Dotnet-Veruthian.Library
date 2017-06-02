using System;
using Soedeum.Dotnet.Library.Text;

namespace _TestConsole
{
    public static class TestText
    {
        public static void Test()
        {
            TestTextSpan();
        }

        public static void TestTextSpan()
        {
            string value = "Hello\nWorld";

            var span = new TextSpan(value);

            Console.WriteLine(span);
            
            foreach (var e in span)
                Console.WriteLine(e);
        }
    }
}