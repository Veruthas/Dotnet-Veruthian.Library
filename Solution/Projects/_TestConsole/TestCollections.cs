using System;
using System.Collections.Generic;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Text;

namespace _TestConsole
{
    public static class TestCollections
    {
        public static void Test()
        {
            TestLookaheadScanner();
        }


        private static void OnRead<S, T>(S scanner, T item)
            where S : BaseScanner<T, S>
        {
            System.Console.WriteLine("Read: {0}: {1}", scanner.Position - 1, item);
        }

        // Simple Scanners
        private static void TestSimpleScanner()
        {
            TestSimpleScanner("Hello, world!");
            TestSimpleScanner("");
            TestSimpleScanner("Hello\r\n\0world!".GetTextElements());
            TestSimpleScanner(new int[] { 1, 2, 3, 4, 5 });
        }

        private static void TestSimpleScanner<T>(IEnumerator<T> enumerator)
        {
            TestScanner(enumerator.GetSimpleScanner(OnRead));
        }
        private static void TestSimpleScanner<T>(IEnumerable<T> enumerable)
        {
            TestSimpleScanner(enumerable.GetEnumerator());
        }

        private static void TestScanner<T>(IScanner<T> scanner)
        {
            while (!scanner.IsEnd)
            {
                System.Console.WriteLine("Peeked: {0}: {1}", scanner.Position, scanner.Peek());
                scanner.Read();
            }

            System.Console.WriteLine("----");
        }

        // LookaheadScanner
        private static void TestLookaheadScanner()
        {
            TestLookaheadScanner("ABCDEF", 8, ((i) => '!'));
        }


        private static void TestLookaheadScanner<T>(IEnumerator<T> enumerator, int lookahead, Func<T, T> generateEndItem = null)
        {
            TestLookaheadScanner(enumerator.GetLookaheadScanner(lookahead, generateEndItem, OnRead), lookahead);
        }

        private static void TestLookaheadScanner<T>(IEnumerable<T> enumerable, int lookahead, Func<T, T> generateEndItem = null)
        {
            TestLookaheadScanner(enumerable.GetEnumerator(), lookahead, generateEndItem);
        }

        private static void TestLookaheadScanner<T>(ILookaheadScanner<T> scanner, int lookahead)
        {
            while (!scanner.IsEnd)
            {
                System.Console.WriteLine("For Position {0}", scanner.Position);

                for (int i = 0; i < lookahead; i++)
                {
                    System.Console.WriteLine("  [{0}]: {1}", i, scanner.Peek(i));
                }

                scanner.Read();
            }
        }
    }
}