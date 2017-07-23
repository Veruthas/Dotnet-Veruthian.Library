using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Soedeum.Dotnet.Library;
using Soedeum.Dotnet.Library.Data;
using Soedeum.Dotnet.Library.Data.Ranges;
using Soedeum.Dotnet.Library.Numeric;
using Soedeum.Dotnet.Library.Text;
using Soedeum.Dotnet.Library.Text.Encodings;

namespace _TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // string s = "𠜎𠜱𠝹𠱓𠱸𠲖𠳏𠳕𠴕𠵼𠵿𠸎𠸏𠹷𠺝𠺢𠻗𠻹𠻺𠼭𠼮𠽌𠾴𠾼𠿪𡁜𡁯𡁵𡁶𡁻𡃁𡃉𡇙𢃇𢞵𢫕𢭃𢯊𢱑𢱕𢳂𢴈𢵌𢵧𢺳𣲷𤓓𤶸𤷪𥄫𦉘𦟌𦧲𦧺𧨾𨅝𨈇𨋢𨳊𨳍𨳒𩶘";            

            Range<OrderableChar> r = new Range<OrderableChar>((char)(char.MaxValue - 1), char.MaxValue);

            foreach (char c in r)
                Console.WriteLine(Convert.ToString((int)c, 16));
                
            Pause();
        }

        // Comment
        static void Pause()
        {
            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
            System.Console.WriteLine();
        }
    }    
}