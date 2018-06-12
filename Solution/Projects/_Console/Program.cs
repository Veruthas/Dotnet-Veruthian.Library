using System;
using System.Text;
using Veruthian.Dotnet.Library.Data;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 2; i <= 10; i++)
                Console.WriteLine(MakeType(i));
            Pause();
        }


        private static string MakeType(int count)
        {
            /*
                public class TypeSet<T0, T1> : TypeSet<T0>, HasType<T1>
                {
                    T1 t1;

                    public TypeSet(T0 t0, T1 t1) : base(t0) =>this.t1 = t1;

                    public void Get(out T1 value) => value = t1;
                }
            */
            StringBuilder builder = new StringBuilder();


            // Declaration
            builder.Append("public class TypeSet<");

            for (int i = 0; i < count; i++)
            {
                if (i != 0)
                    builder.Append(", ");

                builder.Append('T').Append(i);
            }

            builder.Append("> : TypeSet<");

            for (int i = 0; i < count - 1; i++)
            {
                if (i != 0)
                    builder.Append(", ");

                builder.Append('T').Append(i);
            }

            builder.AppendLine($">, HasType<T{count - 1}>");

            builder.AppendLine("{");

            // Variable
            builder.AppendLine($"\tT{count - 1} t{count - 1};");
            builder.AppendLine();

            // Constructor
            builder.Append($"\tpublic TypeSet(");

            for (int i = 0; i < count; i++)
            {
                if (i != 0)
                    builder.Append(", ");

                builder.Append($"T{i} t{i}");
            }

            builder.Append(") : base(");
            for (int i = 0; i < count - 1; i++)
            {
                if (i != 0)
                    builder.Append(", ");

                builder.Append('t').Append(i);
            }

            builder.AppendLine($") => this.t{count - 1} = t{count - 1};");
            builder.AppendLine();

            // Get
            builder.AppendLine($"\tpublic void Get(out T{count - 1} value) => value = t{count - 1};");

            builder.AppendLine("}");


            return builder.ToString();
        }

        static void Pause()
        {
            Console.Write("Press any key to continue...");

            while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;

            Console.WriteLine();
        }
    }
}