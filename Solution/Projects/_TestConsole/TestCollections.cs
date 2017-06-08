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
            //TestSimpleReader();
            //TestLookaheadReader();
            TestSpeculativeReader();
        }


        private static void OnRead<T>(IReader<T> scanner, T item)
        {
            System.Console.WriteLine("Read: {0}: '{1}'", scanner.Position - 1, item);
        }

        // Simple Readers
        private static void TestSimpleReader()
        {
            TestSimpleReader("Hello, world!", (i) => '~');
            TestSimpleReader("", (i) => '~');
            TestSimpleReader("Hello\r\n\0world!".GetTextElements(), (i) => i + '\0');
            TestSimpleReader(new int[] { 1, 2, 3, 4, 5 }, (i) => -1);
        }

        private static void TestSimpleReader<T>(IEnumerator<T> enumerator, Func<T, T> generateEndItem = null)
        {
            TestReader(enumerator.GetSimpleReader(generateEndItem, OnRead));
        }
        private static void TestSimpleReader<T>(IEnumerable<T> enumerable, Func<T, T> generateEndItem = null)
        {
            TestSimpleReader(enumerable.GetEnumerator(), generateEndItem);
        }

        private static void TestReader<T>(IReader<T> scanner)
        {
            while (!scanner.IsEnd)
            {
                System.Console.WriteLine("Peeked: {0}: {1}", scanner.Position, scanner.Peek());
                scanner.Read();
            }

            System.Console.WriteLine("End Item: {0}: '{1}'", scanner.Position, scanner.Peek());

            System.Console.WriteLine("----");
        }

        // LookaheadReader
        private static void TestLookaheadReader()
        {
            TestLookaheadReader("ABCDEFGHIJKLMNOP", false, 20, ((i) => { System.Console.WriteLine("Last: {0}", i); return (char)(i + 1); }));
        }


        private static void TestLookaheadReader<T>(IEnumerator<T> enumerator, bool variable, int lookahead, Func<T, T> generateEndItem = null)
        {
            ILookaheadReader<T> scanner;

            if (variable)
                scanner = enumerator.GetVariableLookaheadReader(generateEndItem, OnRead);
            else
                scanner = enumerator.GetFixedLookaheadReader(lookahead, generateEndItem, OnRead);

            TestLookaheadReader(scanner, lookahead);
        }

        private static void TestLookaheadReader<T>(IEnumerable<T> enumerable, bool variable, int lookahead, Func<T, T> generateEndItem = null)
        {
            TestLookaheadReader(enumerable.GetEnumerator(), variable, lookahead, generateEndItem);
        }

        private static void TestLookaheadReader<T>(ILookaheadReader<T> scanner, int lookahead)
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


        private static void TestSpeculativeReader()
        {
            TestSpeculativeReader("Hello, world!", 2, 7);
        }

        private static void TestSpeculativeReader<T>(IEnumerator<T> enumerator, int speculateAt, int rollbackAt, Func<T, T> generateEndItem = null)
        {
            var scanner = enumerator.GetSpeculativeReader(generateEndItem, OnRead,
                    ((s) => Console.WriteLine("Speculating at Position: {0}", s.Position)),
                    ((s) => Console.WriteLine("Committed to our speculation at Position {0}", s.Position)),
                    ((s, from, to) => Console.WriteLine("Rolledback from {0} to {1}!", from, to)));

            TestSpeculativeReader(scanner, speculateAt, rollbackAt);
        }

        private static void TestSpeculativeReader<T>(IEnumerable<T> enumerable, int speculateAt, int rollbackAt, Func<T, T> generateEndItem = null)
        {
            TestSpeculativeReader(enumerable.GetEnumerator(), speculateAt, rollbackAt, generateEndItem);
        }

        private static void TestSpeculativeReader<T>(ISpeculativeReader<T> scanner, int speculateAt, int rollbackAt)
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
                            scanner.Retract();
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