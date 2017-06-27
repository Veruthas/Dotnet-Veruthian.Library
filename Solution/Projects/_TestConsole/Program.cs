using System;
using System.Collections.Generic;
using System.IO;
using Soedeum.Dotnet.Library;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Text;

namespace _TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
<<<<<<< HEAD
            var s = new CodeString('H', 'e', 'l', 'l', 'o');
            var t = new CodeString('H', 'e', 'l', 'l', 'o');
            CodePoint u = 's';
            var b = u * 4;
            var c = b * 4;
            
            s = null;

            string y = s;

=======
>>>>>>> 67f3c30c1001bc603989912cc878b6bdeef58b03
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