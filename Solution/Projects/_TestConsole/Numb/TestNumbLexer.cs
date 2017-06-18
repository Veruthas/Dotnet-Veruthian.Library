using Soedeum.Dotnet.Library.Collections;

namespace _TestConsole.Numb
{
    public static class TestNumbLexer
    {

        public static void Test()
        {
            
            TestLexer("a := 1 + 2;");
            TestLexer("a := -10 ^ 4.12;");
            TestLexer("# Algorithms for circle formula\npi := 3.1412;\nr := -1.23e3;\r\ncircumference := 2 * pi * r;\narea := pi * r ^ 2;");
        }

        static int run = 0;
        static void TestLexer(string program)

        {
            // NumbLexer lexer = new NumbLexer((run++).ToString(), program.GetSimpleReader());

            // foreach (var token in lexer.GetEnumerable())
            // {
            //     System.Console.WriteLine(token.ToShortString());
            // }
            // System.Console.WriteLine("--------");
        }
    }
}