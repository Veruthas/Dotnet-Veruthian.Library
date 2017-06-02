using Soedeum.Dotnet.Library.Collections;

namespace _TestConsole
{
    public class TestCollections
    {
        public static void Test()
        {
            int[] a = { 1, 2, 3, 4, 5 };

            var b = new EnumeratorFetcher<int>(a);

            var c = b.GetSimpleScanner();

            c.ItemConsumed += (scanner, i) => System.Console.WriteLine(i);

            while (!c.IsEnd)
                c.Consume();
        }
    }
}