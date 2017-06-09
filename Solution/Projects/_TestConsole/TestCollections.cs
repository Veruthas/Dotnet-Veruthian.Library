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


        private static void OnRead<T>(IReader<T> reader, T item)
        {
            System.Console.WriteLine("Read: {0}: '{1}'", reader.Position - 1, item);
        }

        // Simple Readers
        private static void TestSimpleReader()
        {
            TestSimpleReader("Hello, world!", (i) => '~');
            TestSimpleReader("", (i) => '~');
            TestSimpleReader("Hello\r\n\0world!".GetTextElements(), ((i) => i + '\0'));
            TestSimpleReader(new int[] { 1, 2, 3, 4, 5 }, (i) => -1);
        }

        private static void TestSimpleReader<T>(IEnumerator<T> enumerator, GenerateEndItem<T> generateEndItem = null)
        {
            TestReader(enumerator.GetSimpleReader(generateEndItem, OnRead));
        }
        private static void TestSimpleReader<T>(IEnumerable<T> enumerable, GenerateEndItem<T> generateEndItem = null)
        {
            TestSimpleReader(enumerable.GetEnumerator(), generateEndItem);
        }

        private static void TestReader<T>(IReader<T> reader)
        {
            while (!reader.IsEnd)
            {
                System.Console.WriteLine("Peeked: {0}: {1}", reader.Position, reader.Peek());
                reader.Read();
            }

            System.Console.WriteLine("End Item: {0}: '{1}'", reader.Position, reader.Peek());

            System.Console.WriteLine("----");
        }

        // LookaheadReader
        private static void TestLookaheadReader()
        {
            TestLookaheadReader("ABCDEFGHIJKLMNOP", false, 20, ((i) => { System.Console.WriteLine("Last: {0}", i); return (char)(i + 1); }));
        }


        private static void TestLookaheadReader<T>(IEnumerator<T> enumerator, bool variable, int lookahead, GenerateEndItem<T> generateEndItem = null)
        {
            ILookaheadReader<T> reader;

            if (variable)
                reader = enumerator.GetVariableLookaheadReader(generateEndItem, OnRead);
            else
                reader = enumerator.GetFixedLookaheadReader(lookahead, generateEndItem, OnRead);

            TestLookaheadReader(reader, lookahead);
        }

        private static void TestLookaheadReader<T>(IEnumerable<T> enumerable, bool variable, int lookahead, GenerateEndItem<T> generateEndItem = null)
        {
            TestLookaheadReader(enumerable.GetEnumerator(), variable, lookahead, generateEndItem);
        }

        private static void TestLookaheadReader<T>(ILookaheadReader<T> reader, int lookahead)
        {
            while (!reader.IsEnd)
            {
                System.Console.WriteLine("For Position {0}", reader.Position);

                for (int i = 0; i < lookahead; i++)
                {
                    System.Console.WriteLine("  [{0}]: {1}, IsEnd:{2}", i, reader.Peek(i), reader.PeekIsEnd(i));
                }

                reader.Read();
            }

            System.Console.WriteLine("At End:");
            for (int i = 0; i < lookahead; i++)
            {
                System.Console.WriteLine("  [{0}]: {1}", i, reader.Peek(i));
            }
        }


        private static void TestSpeculativeReader()
        {
            TestSpeculativeReader<char, object>("Hello, world!", 2, 7);
        }

        private static void TestSpeculativeReader<T, TState>(IEnumerator<T> enumerator, int speculateAt, int retreatAt, GenerateEndItem<T> generateEndItem = null)
        {
            var reader = enumerator.GetSpeculativeReader<T, TState>(
                    generateEndItem, 
                    OnRead,
                    ((r, s) => Console.WriteLine("Marked Position: {0}, with State: {1}", r.Position, s)),
                    ((r) => Console.WriteLine("Committed to our speculation at Position {0}", r.Position)),
                    ((r, from, to, s) => Console.WriteLine("Retreated from {0} to {1}!", from, to)));

            TestSpeculativeReader(reader, speculateAt, retreatAt);
        }

        private static void TestSpeculativeReader<T, TState>(IEnumerable<T> enumerable, int speculateAt, int rollbackAt, GenerateEndItem<T> generateEndItem = null)
        {
            TestSpeculativeReader<T, TState>(enumerable.GetEnumerator(), speculateAt, rollbackAt, generateEndItem);
        }

        private static void TestSpeculativeReader<T, TState>(ISpeculativeReader<T, TState> reader, int speculateAt, int rollbackAt)
        {
            bool speculated = false;

            while (!reader.IsEnd)
            {
                while (!reader.IsEnd)
                {
                    System.Console.WriteLine("Peeked: {0}: '{1}'", reader.Position, reader.Peek());

                    reader.Read();

                    if (!speculated)
                    {
                        if (reader.Position == speculateAt)
                        {
                            reader.Mark();
                            continue;
                        }
                        if (reader.Position == rollbackAt)
                        {
                            reader.Retreat();
                            speculated = true;
                            continue;
                        }
                    }
                    
                }

                System.Console.WriteLine("End Item: {0}: '{1}'", reader.Position, reader.Peek());

                System.Console.WriteLine("----");
            }
        }
    }
}