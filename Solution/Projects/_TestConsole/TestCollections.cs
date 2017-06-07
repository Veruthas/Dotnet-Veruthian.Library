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
            //TestSimpleScanner();
            TestLookaheadScanner();
        }


        private static void OnRead<S, T>(S scanner, T item)
            where S : ScannerBase<T, S>
        {
            System.Console.WriteLine("Read: {0}: {1}", scanner.Position - 1, item);
        }

        // Simple Scanners
        private static void TestSimpleScanner()
        {
            TestSimpleScanner("Hello, world!", (i) => '~');
            TestSimpleScanner("", (i) => '~');
            TestSimpleScanner("Hello\r\n\0world!".GetTextElements(), (i) => i + '\0');
            TestSimpleScanner(new int[] { 1, 2, 3, 4, 5 }, (i) => -1);
        }

        private static void TestSimpleScanner<T>(IEnumerator<T> enumerator, Func<T, T> generateEndItem = null)
        {
            TestScanner(enumerator.GetSimpleScanner(generateEndItem, OnRead));
        }
        private static void TestSimpleScanner<T>(IEnumerable<T> enumerable, Func<T, T> generateEndItem = null)
        {
            TestSimpleScanner(enumerable.GetEnumerator(), generateEndItem);
        }

        private static void TestScanner<T>(IScanner<T> scanner)
        {
            while (!scanner.IsEnd)
            {
                System.Console.WriteLine("Peeked: {0}: {1}", scanner.Position, scanner.Peek());
                scanner.Read();
            }

            System.Console.WriteLine("End Item: {0}: '{1}'", scanner.Position, scanner.Peek());

            System.Console.WriteLine("----");
        }

        // LookaheadScanner
        private static void TestLookaheadScanner()
        {
            TestLookaheadScanner("ABCDEFGHIJKLMNOP", 20, ((i) => { System.Console.WriteLine("Last: {0}", i); return (char)(i + 1); }));
        }


        private static void TestLookaheadScanner<T>(IEnumerator<T> enumerator, int lookahead, Func<T, T> generateEndItem = null)
        {
            TestLookaheadScanner(enumerator.GetVariableLookaheadScanner(generateEndItem, OnRead), lookahead);
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

            System.Console.WriteLine("At End:");
            for (int i = 0; i < lookahead; i++)
            {
                System.Console.WriteLine("  [{0}]: {1}", i, scanner.Peek(i));
            }
        }
    }
}