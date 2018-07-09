using System;
using Veruthian.Dotnet.Library.Numeric;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var suit in Suit.Items)
                Console.WriteLine("{0}: {1}", suit, suit.Name);

            Pause();
        }

        public class Suit : Enum<Suit>
        {
            readonly char symbol;

            public char Symbol => symbol;


            public static readonly Suit Spades = new Suit("Spades", '♠');

            public static readonly Suit Hearts = new Suit("Hearts", '♥');

            public static readonly Suit Clubs = new Suit("Clubs", '♣');

            public static readonly Suit Diamonds = new Suit("Diamonds", '♦');


            protected Suit(string name, char symbol) : base(name) => this.symbol = symbol;

            public override string ToString() => symbol.ToString();
        }

        static void Pause()
        {
            Console.Write("Press any key to continue...");

            while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;

            Console.WriteLine();
        }
    }
}