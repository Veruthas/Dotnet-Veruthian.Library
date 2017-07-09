using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Soedeum.Dotnet.Library;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Numerics;
using Soedeum.Dotnet.Library.Text;
using Soedeum.Dotnet.Library.Text.Encodings;

namespace _TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // string s = "𠜎𠜱𠝹𠱓𠱸𠲖𠳏𠳕𠴕𠵼𠵿𠸎𠸏𠹷𠺝𠺢𠻗𠻹𠻺𠼭𠼮𠽌𠾴𠾼𠿪𡁜𡁯𡁵𡁶𡁻𡃁𡃉𡇙𢃇𢞵𢫕𢭃𢯊𢱑𢱕𢳂𢴈𢵌𢵧𢺳𣲷𤓓𤶸𤷪𥄫𦉘𦟌𦧲𦧺𧨾𨅝𨈇𨋢𨳊𨳍𨳒𩶘";

            // byte[] b8 = Encoding.UTF8.GetBytes(s);

            // var c8 = new CodeString(b8.AsUtf8CodePoints());


            // byte[] b16 = Encoding.Unicode.GetBytes(s);

            // var c16 = new CodeString(b16.AsUtf16CodePoints(true));


            // byte[] b32 = Encoding.UTF32.GetBytes(s);

            // var c32 = new CodeString(b32.AsUtf32CodePoints(true));


            // var d = new Utf32.ByteDecoder(ByteOrder.BigEndian);

            // var e = new Utf32.ByteEncoder(ByteOrder.BigEndian);

            // CodePoint c0 = "𠱓", c1 = default(CodePoint);

            // bool result = e.TryProcess(c0, out Bits64 bits);

            // for (int i = 0; i < bits.ByteCount; i++)
            // {
            //     var value = bits.GetByte(i);

            //     result = d.TryProcess(value, out uint u1);

            //     if (result)
            //     {
            //         c1 = u1;
            //         break;
            //     }
            // }

            // Bits64 s = new Bits64(0x90, 0xAB, 0xcCD, 0x2);

            // CodePoint c0 = "𠱓";

            // var bits = c0.ToUtf32();

            // byte[] x = { 0x32, bits.GetByte(0), bits.GetByte(1), bits.GetByte(2), bits.GetByte(3) };


            // //CodePoint c1 = CodePoint.FromUtf32(bits, ByteOrder.BigEndian);

            // CodePoint c1 = CodePoint.FromUtf32(x, 1);

            // Console.WriteLine("'{0}' == '{1}' ? {2}", c0, c1, c0 == c1);

            //var a = BitTwiddler.FromLong(0x0123456789ABCDEF);

            // var b = a.ReverseShortBytes();

            // var c = a.ReverseIntBytes();

            // var d = a.ReverseIntShorts();

            // var e = a.ReverseByteNibbles();

            // var f = a.ReverseLongInts();

            // var g = a.ReverseLongShorts();

            // var h = a.ReverseNibbles();

            // var i = a.ReverseBytes();

            var a = BitTwiddler.FromByte(0xAB);

            var b = a.ReverseNibbleBits();

            var c = a >> -1;

            var d = a << 1;

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