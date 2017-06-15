using System;
using Soedeum.Dotnet.Library.Text;

namespace _TestConsole
{
    public static class TestText
    {
        public static void Test()
        {
            CharacterSet alpha = CharacterSet.LetterOrDigitOrUnderscore;

            Console.WriteLine("Set: {{{0}}}", alpha);

            foreach (char c in "Hello, I am '31' years old_.")
            {
                Console.WriteLine("  {0} [{1}].", c, alpha.Includes(c) ? "YES" : "NO");
            }
        }

        private static void TestUnion()
        {
            var a = CharacterSet.FromList('A', 'B', 'C', 'D');
            var b = CharacterSet.FromRange('E', 'H');
            Unionize(a, b);
            var ab = CharacterSet.FromUnion(a, b);
            var c = CharacterSet.FromValue('I');
            var d = CharacterSet.FromRange('J', 'W');
            var e = CharacterSet.FromRange('V', 'Z');
            var ce = CharacterSet.FromUnion(c, e);
            Unionize(c, e);
            var abcde = CharacterSet.FromUnion(ab, ce, d);
            Unionize(ab, ce, d);
        }

        private static void TestEnumerable(CharacterSet a)
        {
            System.Console.WriteLine("Enumerating chars in {{{0}}}", a);
            foreach (char c in a)
                System.Console.WriteLine("  {0}", c);
        }

        private static void Unionize(params CharacterSet[] sets)
        {
            System.Console.WriteLine("Union of sets:");

            bool initialized = false;

            foreach (var set in sets)
            {
                System.Console.WriteLine("{0} {1}", (initialized ? " +" : "  "), set);
                initialized = true;
            }

            var union = CharacterSet.FromUnion(sets);

            System.Console.WriteLine(" = {0}", union);
        }

        private static void UnionizePairs(params CharacterSet[] sets)
        {
            for (int i = 0; i < sets.Length; i++)
            {
                Console.WriteLine("Set combination {0}", i);

                foreach (var b in sets)
                {
                    var a = sets[i];

                    var c = CharacterSet.FromUnion(a, b);

                    Console.WriteLine("{0} + {1} = {2}", a, b, c);
                }
            }
        }
    }
}