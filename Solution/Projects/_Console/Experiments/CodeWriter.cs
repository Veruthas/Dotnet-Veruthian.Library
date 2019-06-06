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
            string template =
@"public static IEnumerable<T> Enumerate<T>(this(*ITEMS*) tuple)
{
*YIELDS*}
";

            string itemsString = "";

            for (int i = 0; i < size; i++)
            {
                if (i != 0) itemsString += ", ";

                itemsString += "T";
            }

            string yieldsString = "";

            for (int i = 0; i < size; i++)
            {
                yieldsString += $"\tyield return tuple.Item{i + 1};\n";
            }


            template = template.Replace("*ITEMS*", itemsString);

            template = template.Replace("*YIELDS*", yieldsString);

            return template;
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


        // Get
        public static string GenerateTupleGet(int size)
        {
            string template =
@"public static T Get<T*GENERICS*>(this (*ITEMS*) tuple, int index)
{
    switch (index)
    {
*SWITCH*
        default:
            return default(T);
    }
}

";
            string genericsString = "";

            string itemsString = "";

            for (int i = 0; i < size; i++)
            {
                if (i != 0) itemsString += ", ";

                itemsString += $"T";
            }

            string switchString = "";

            for (int i = 0; i < size; i++)
            {
                switchString += $"\t\tcase {i}:\n";
                switchString += $"\t\t\treturn tuple.Item{i + 1};\n";
            }

            template = template.Replace("*GENERICS*", genericsString);

            template = template.Replace("*ITEMS*", itemsString);

            template = template.Replace("*SWITCH*", switchString);

            return template;
        }


        // TypeSetCreate
        public static string GenerateTypeSetCreate(int size)
        {
            string template =
@"public static TypeSet<*GENERICS*> Create<*GENERICS*>() => new TypeSet<*GENERICS*>();
";


            string genericsString = "";

            for (int i = 0; i < size; i++)
            {
                if (i != 0) genericsString += ", ";

                genericsString += $"T{i}";
            }

            template = template.Replace("*GENERICS*", genericsString);

            return template;
        }


        // TypeSetCreateWithValues
        public static string GenerateTypeSetCreateWithValues(int size)
        {
            string template =
@"public static TypeSet<*GENERICS*> Create<*GENERICS*>(*ITEMS*) => new TypeSet<*GENERICS*>(*IDENTIFIERS*);
";
            const string separator = ", ";


            string genericsString = "";

            string itemsString = "";

            string identifiersString = "";

            for (int i = 0; i < size; i++)
            {
                if (i != 0)
                {
                    genericsString += separator;
                    itemsString += separator;
                    identifiersString += separator;
                }

                genericsString += $"T{i}";
                itemsString += $"T{i} t{i}";
                identifiersString += $"t{i}";
            }

            template = template.Replace("*GENERICS*", genericsString);
            template = template.Replace("*ITEMS*", itemsString);
            template = template.Replace("*IDENTIFIERS*", identifiersString);

            return template;
        }
    }
}