using System.Collections.Generic;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Text;

namespace _TestConsole
{
    public class TestCollections
    {
        public static void Test()
        {
            //TestEnumerators();
            TestTextElementFetcher();
        }

        private static void TestScanner<T>(IScanner<T> scanner)
        {
            scanner.ItemConsumed += (s, i) => System.Console.WriteLine("Consumed: {0}: {1}", s.Position - 1, i);

            while (!scanner.IsEnd)
            {
                //System.Console.WriteLine("Peeked: {0}: {1}", scanner.Position, scanner.Peek());
                scanner.Consume();
            }

            System.Console.WriteLine("----");
        }

        private static void TestEnumerators()
        {
            int[] a = { 1, 2, 3, 4, 5 };

            string b = "Hello, world";

            double[] c = { 1.2, 2.3, 3.4, 4.5 };

            TestEnumerator(a);

            TestEnumerator(b);

            TestEnumerator(c);
        }

        public static void TestEnumerator<T>(IEnumerable<T> list)
        {
            var fetcher = new EnumeratorFetcher<T>(list);

            var scanner = fetcher.GetSimpleScanner();

            TestScanner(scanner);
        }


        public static void TestTextElementFetcher()
        {
            string data = "Hello, world!\n\tMy name is Levi.\r\nWhat is your name?";

            var charfetcher = CharFetcher.FromString(data);

            var texelfetcher = new TextElementFetcher(charfetcher);

            var scanner = texelfetcher.GetSimpleScanner();

            TestScanner(scanner);
        }
    }
}