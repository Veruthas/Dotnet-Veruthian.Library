using System;

namespace _Console
{
    public static class CodeWriter
    {
        public static string Process(Func<int, string> generate, int start, int end)
        {
            string value = "";

            for (int i = start; i < end; i++)
                value += generate(i);

            return value;
        }


        // Enumerate
        public static string GenerateTupleEnumerate(int size)
        {
            string value = "";
            value += "public static IEnumerable<T> Enumerate<T>(this (";

            for (int i = 0; i < size; i++)
                value += i == 0 ? "T" : ", T";

            value += ") tuple)\n";
            value += "{\n";

            for (int i = 0; i < size; i++)
                value += $"\tyield return tuple.Item{i + 1};\n";

            value += "}\n\n";

            return value;
        }

        public static string GenerateCastedTupleEnumerate(int size)
        {
            string value = "";
            value += "public static IEnumerable<T> Enumerate<T";

            for (int i = 0; i < size; i++)
                value += $", T{i + 1}";

            value += ">(this (";

            for (int i = 0; i < size; i++)
                value += i == 0 ? "T" : ", T";

            value += ") tuple)\n";

            for (int i = 0; i < size; i++)
                value += $"\twhere T{i + 1} : T\n";

            value += "{\n";

            for (int i = 0; i < size; i++)
                value += $"\tyield return (T)tuple.Item{i + 1};\n";

            value += "}\n\n";

            return value;
        }


        // Count
        public static string GenerateTupleCount(int size)
        {
            string template = 
@"public static int Count<*GENERICS*>(this (*ITEMS*) tuple)
{
    return *SIZE*;
}

";

            string genericsString = "T";

            string itemsString = "";

            for (int i = 0; i < size; i++)
            {
                if (i != 0) itemsString += ", ";

                itemsString += $"T";
            }

            string sizeString = size.ToString();


            template = template.Replace("*GENERICS*", genericsString);

            template = template.Replace("*ITEMS*", itemsString);

            template = template.Replace("*SIZE*", sizeString);

            return template;
        }

        public static string GenerateCastedTupleCount(int size)
        {
            string template = 
@"public static int Count<*GENERICS*>(this (*ITEMS*) tuple)
{
    return *SIZE*;
}

";
            string genericsString = "";

            for (int i = 0; i < size; i++)
            {
                if (i != 0) genericsString += ", ";

                genericsString += $"T{i + 1}";
            }

            string itemsString = genericsString;

            string sizeString = size.ToString();


            template = template.Replace("*GENERICS*", genericsString);

            template = template.Replace("*ITEMS*", itemsString);

            template = template.Replace("*SIZE*", sizeString);

            return template;
        }    
    }
}