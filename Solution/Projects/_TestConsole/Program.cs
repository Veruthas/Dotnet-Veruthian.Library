using System;
using System.Collections.Generic;
using System.IO;
using _TestConsole.Numb;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Text;

namespace _TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = CharSet.FromUnion(CharSet.FromList('a', 'b'), CharSet.FromValue('X'), CharSet.FromList('1', '2', '3'));
            var b = CharSet.FromUnion(a, CharSet.FromRange('0', '9'));            
            var d = CharSet.FromUnion(b, CharSet.Letter);
            var c = CharSet.FromCompliment(b);


            TestNumbLexer("a := 1 + 2;");
            TestNumbLexer("a := -10 ^ 4.12;");
            TestNumbLexer("# Algorithms for circle formula\npi := 3.1412;\nr := -1.23e3;\r\ncircumference := 2 * pi * r;\narea := pi * r ^ 2;");

            Pause();
        }
        static int run = 0;
        static void TestNumbLexer(string program)
        {
            NumbLexer lexer = new NumbLexer((run++).ToString(), program.GetSimpleReader());

            foreach (var token in lexer.GetEnumerable())
            {
                System.Console.WriteLine(token.ToShortString());
            }
            System.Console.WriteLine("--------");
        }
        static void Pause()
        {
            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
            System.Console.WriteLine();
        }
    }
}