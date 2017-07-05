using System;
using Soedeum.Dotnet.Library.Text;
using Soedeum.Dotnet.Library.Text.Char;

namespace _TestConsole
{
    public static class TestText
    {
        public static void Test()
        {
            CharSet value = 'f';
            TestRangeString(value);

            CharSet range = CharSet.Range('a', 'z');
            TestRangeString(range);

            CharSet list0 = CharSet.List(" \t\r\n");
            TestRangeString(list0);   
            
            CharSet list1 = CharSet.List("Axf143efffhg2");
            TestRangeString(list1);
        }

        private static void TestRangeString(CharSet set)
        {
            Console.WriteLine("{{{0}}}", set);

            string setS = set.ToRangeString();
            Console.WriteLine("\"{0}\"", setS);

            var setRS = CharSet.FromRangeString(setS);
            Console.WriteLine("{{{0}}}", setRS);
        }

        private static void OldTest()
        {
            var all = CharSet.Range(char.MinValue, char.MaxValue);

            var none = CharSet.Empty;

            var union = CharSet.Union(all, none);

            CharSetInfo(all);
            CharSetInfo(none);
            CharSetInfo(union);
            CharSetInfo(none + "ABCD");
        }

        private static void TestEquality()
        {
            // Value
            TestEquality('x', 'x');

            // Range
            var m = CharSet.Range('a', 'z');
            var n = CharSet.Range('a', 'z');

            TestEquality(m, n);

            // List
            var x = CharSet.List("Ar$");
            var y = CharSet.List("$Ar");

            TestEquality(x, y);

            var p = CharSet.List("Arst$");
            var q = CharSet.List("$Arst");

            TestEquality(p, q);

            // Union
            var a = CharSet.List('A', 'B', 'C', 'D');
            var b = CharSet.Range('E', 'H');

            var ab = CharSet.Union(a, b);

            var c = CharSet.Value('I');
            var d = CharSet.Range('J', 'W');
            var e = CharSet.Range('V', 'Z');

            var ce = CharSet.Union(c, e);

            var union0 = CharSet.Union(a, b, c, d, e);
            var union1 = CharSet.Union(ab, ce, d);

            TestEquality(union0, union1);
        }

        private static void CharSetInfo(CharSet set)
        {
            System.Console.WriteLine("{{{0}}}", set);
            System.Console.WriteLine(" Type: {0}", set.GetType().Name);
            System.Console.WriteLine(" Size: {0}", set.Size);
            System.Console.WriteLine(" Hashcode: {0}\n", set.GetHashCode());
        }
        private static void TestEquality(CharSet union0, CharSet union1)
        {
            System.Console.WriteLine("{0} {2} {1}", union0, union1, union0 == union1 ? "==" : "!=");
            System.Console.WriteLine(" Hashcode for union0: {0}", union0.GetHashCode());
            System.Console.WriteLine(" Hashcode for union1: {0}", union1.GetHashCode());
            System.Console.WriteLine(" Type 0: {0}", union0.GetType().Name);
            System.Console.WriteLine(" Type 1: {0}", union1.GetType().Name);
        }

        private static void TestUnionFind()
        {
            CharSet alpha = CharSet.LetterOrDigitOrUnderscore;

            Console.WriteLine("Set: {{{0}}}", alpha);

            foreach (char c in "Hello, I am '31' years old_.")
            {
                Console.WriteLine("  {0} [{1}].", c, alpha.Contains(c) ? "YES" : "NO");
            }
        }

        private static void TestUnion()
        {
            var a = CharSet.List('A', 'B', 'C', 'D');
            var b = CharSet.Range('E', 'H');
            Unionize(a, b);
            var ab = CharSet.Union(a, b);
            var c = CharSet.Value('I');
            var d = CharSet.Range('J', 'W');
            var e = CharSet.Range('V', 'Z');
            var ce = CharSet.Union(c, e);
            Unionize(c, e);
            var abcde = CharSet.Union(ab, ce, d);
            Unionize(ab, ce, d);
        }

        private static void TestEnumerable(CharSet a)
        {
            System.Console.WriteLine("Enumerating chars in {{{0}}}", a);
            // foreach (char c in a)
            //     System.Console.WriteLine("  {0}", c);
        }

        private static void Unionize(params CharSet[] sets)
        {
            System.Console.WriteLine("Union of sets:");

            bool initialized = false;

            foreach (var set in sets)
            {
                System.Console.WriteLine("{0} {1}", (initialized ? " +" : "  "), set);
                initialized = true;
            }

            var union = CharSet.Union(sets);

            System.Console.WriteLine(" = {0}", union);
        }

        private static void UnionizePairs(params CharSet[] sets)
        {
            for (int i = 0; i < sets.Length; i++)
            {
                Console.WriteLine("Set combination {0}", i);

                foreach (var b in sets)
                {
                    var a = sets[i];

                    var c = CharSet.Union(a, b);

                    Console.WriteLine("{0} + {1} = {2}", a, b, c);
                }
            }
        }
    }
}