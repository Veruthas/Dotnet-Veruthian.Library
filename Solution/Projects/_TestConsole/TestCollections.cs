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
            //TestLookaheadScanner();
            TestSpeculativeScanner();
        }


        private static void OnRead<T>(IScanner<T> scanner, T item)
        {
            System.Console.WriteLine("Read: {0}: '{1}'", scanner.Position - 1, item);
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
            TestLookaheadScanner("ABCDEFGHIJKLMNOP", false, 20, ((i) => { System.Console.WriteLine("Last: {0}", i); return (char)(i + 1); }));
        }


        private static void TestLookaheadScanner<T>(IEnumerator<T> enumerator, bool variable, int lookahead, Func<T, T> generateEndItem = null)
        {
            ILookaheadScanner<T> scanner;

            if (variable)
                scanner = enumerator.GetVariableLookaheadScanner(generateEndItem, OnRead);
            else
                scanner = enumerator.GetFixedLookaheadScanner(lookahead, generateEndItem, OnRead);

            TestLookaheadScanner(scanner, lookahead);
        }

        private static void TestLookaheadScanner<T>(IEnumerable<T> enumerable, bool variable, int lookahead, Func<T, T> generateEndItem = null)
        {
            TestLookaheadScanner(enumerable.GetEnumerator(), variable, lookahead, generateEndItem);
        }

        private static void TestLookaheadScanner<T>(ILookaheadScanner<T> scanner, int lookahead)
        {
            while (!scanner.IsEnd)
            {
                System.Console.WriteLine("For Position {0}", scanner.Position);

                for (int i = 0; i < lookahead; i++)
                {
                    System.Console.WriteLine("  [{0}]: {1}, IsEnd:{2}", i, scanner.Peek(i), scanner.PeekIsEnd(i));
                }

                scanner.Read();
            }

            System.Console.WriteLine("At End:");
            for (int i = 0; i < lookahead; i++)
            {
                System.Console.WriteLine("  [{0}]: {1}", i, scanner.Peek(i));
            }
        }


        private static void TestSpeculativeScanner()
        {
            TestSpeculativeScanner("Hello, world!", 2, 7);
        }

        private static void TestSpeculativeScanner<T>(IEnumerator<T> enumerator, int speculateAt, int rollbackAt, Func<T, T> generateEndItem = null)
        {
            var scanner = enumerator.GetSpeculativeScanner(generateEndItem, OnRead,
                    ((s) => Console.WriteLine("Speculating at Position: {0}", s.Position)),
                    ((s) => Console.WriteLine("Committed to our speculation at Position {0}", s.Position)),
                    ((s, from, to) => Console.WriteLine("Rolledback from {0} to {1}!", from, to)));

            TestSpeculativeScanner(scanner, speculateAt, rollbackAt);
        }

        private static void TestSpeculativeScanner<T>(IEnumerable<T> enumerable, int speculateAt, int rollbackAt, Func<T, T> generateEndItem = null)
        {
            TestSpeculativeScanner(enumerable.GetEnumerator(), speculateAt, rollbackAt, generateEndItem);
        }

        private static void TestSpeculativeScanner<T>(ISpeculativeScanner<T> scanner, int speculateAt, int rollbackAt)
        {
            bool speculated = false;

            while (!scanner.IsEnd)
            {
                while (!scanner.IsEnd)
                {
                    System.Console.WriteLine("Peeked: {0}: '{1}'", scanner.Position, scanner.Peek());

                    scanner.Read();

                    if (!speculated)
                    {
                        if (scanner.Position == speculateAt)
                        {
                            scanner.Speculate();
                            continue;
                        }
                        if (scanner.Position == rollbackAt)
                        {
                            scanner.Rollback();
                            speculated = true;
                            continue;
                        }
                    }
                    
                }

                System.Console.WriteLine("End Item: {0}: '{1}'", scanner.Position, scanner.Peek());

                System.Console.WriteLine("----");
            }
        }
    }
}