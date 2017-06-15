using System;
using Soedeum.Dotnet.Library.Text;

namespace _TestConsole
{
    public static class TestText
    {
        public static void Test()
        {
            var all = CharacterSet.Range(char.MinValue, char.MaxValue);

            var none = CharacterSet.None;

            var union = CharacterSet.Union(all, none);
            
            CharacterSetInfo(all);
            CharacterSetInfo(none);
            CharacterSetInfo(union);
            CharacterSetInfo();
        }

        private static void TestEquality()
        {
            // Value
            TestEquality('x', 'x');

            // Range
            var m = CharacterSet.Range('a', 'z');
            var n = CharacterSet.Range('a', 'z');

            TestEquality(m, n);

            // List
            var x = CharacterSet.List("Ar$");
            var y = CharacterSet.List("$Ar");

            TestEquality(x, y);

            var p = CharacterSet.List("Arst$");
            var q = CharacterSet.List("$Arst");

            TestEquality(p, q);

            // Union
            var a = CharacterSet.List('A', 'B', 'C', 'D');
            var b = CharacterSet.Range('E', 'H');

            var ab = CharacterSet.Union(a, b);

            var c = CharacterSet.Value('I');
            var d = CharacterSet.Range('J', 'W');
            var e = CharacterSet.Range('V', 'Z');

            var ce = CharacterSet.Union(c, e);

            var union0 = CharacterSet.Union(a, b, c, d, e);
            var union1 = CharacterSet.Union(ab, ce, d);

            TestEquality(union0, union1);
        }

        private static void CharacterSetInfo(CharacterSet set)
        {
            System.Console.WriteLine("{{{0}}}", set);
            System.Console.WriteLine(" Type: {0}", set.GetType().Name);
            System.Console.WriteLine(" Size: {0}", set.Size);
            System.Console.WriteLine(" Hashcode: {0}\n", set.GetHashCode());            
        }
        private static void TestEquality(CharacterSet union0, CharacterSet union1)
        {
            System.Console.WriteLine("{0} {2} {1}", union0, union1, union0 == union1 ? "==" : "!=");
            System.Console.WriteLine(" Hashcode for union0: {0}", union0.GetHashCode());
            System.Console.WriteLine(" Hashcode for union1: {0}", union1.GetHashCode());
            System.Console.WriteLine(" Type 0: {0}", union0.GetType().Name);
            System.Console.WriteLine(" Type 1: {0}", union1.GetType().Name);
        }

        private static void TestUnionFind()
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
            var a = CharacterSet.List('A', 'B', 'C', 'D');
            var b = CharacterSet.Range('E', 'H');
            Unionize(a, b);
            var ab = CharacterSet.Union(a, b);
            var c = CharacterSet.Value('I');
            var d = CharacterSet.Range('J', 'W');
            var e = CharacterSet.Range('V', 'Z');
            var ce = CharacterSet.Union(c, e);
            Unionize(c, e);
            var abcde = CharacterSet.Union(ab, ce, d);
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

            var union = CharacterSet.Union(sets);

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

                    var c = CharacterSet.Union(a, b);

                    Console.WriteLine("{0} + {1} = {2}", a, b, c);
                }
            }
        }
    }
}