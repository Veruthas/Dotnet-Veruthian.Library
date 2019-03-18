using System.Collections.Generic;

namespace Veruthian.Library.Types.Extensions
{
    public static class TupleExtensions
    {
        #region Enumerate

        public static IEnumerable<T> Enumerate<T>(this (T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
            yield return tuple.Item8;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
            yield return tuple.Item8;
            yield return tuple.Item9;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T, T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
            yield return tuple.Item8;
            yield return tuple.Item9;
            yield return tuple.Item10;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
            yield return tuple.Item8;
            yield return tuple.Item9;
            yield return tuple.Item10;
            yield return tuple.Item11;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
            yield return tuple.Item8;
            yield return tuple.Item9;
            yield return tuple.Item10;
            yield return tuple.Item11;
            yield return tuple.Item12;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
            yield return tuple.Item8;
            yield return tuple.Item9;
            yield return tuple.Item10;
            yield return tuple.Item11;
            yield return tuple.Item12;
            yield return tuple.Item13;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
            yield return tuple.Item8;
            yield return tuple.Item9;
            yield return tuple.Item10;
            yield return tuple.Item11;
            yield return tuple.Item12;
            yield return tuple.Item13;
            yield return tuple.Item14;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
            yield return tuple.Item8;
            yield return tuple.Item9;
            yield return tuple.Item10;
            yield return tuple.Item11;
            yield return tuple.Item12;
            yield return tuple.Item13;
            yield return tuple.Item14;
            yield return tuple.Item15;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
            yield return tuple.Item8;
            yield return tuple.Item9;
            yield return tuple.Item10;
            yield return tuple.Item11;
            yield return tuple.Item12;
            yield return tuple.Item13;
            yield return tuple.Item14;
            yield return tuple.Item15;
            yield return tuple.Item16;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
            yield return tuple.Item8;
            yield return tuple.Item9;
            yield return tuple.Item10;
            yield return tuple.Item11;
            yield return tuple.Item12;
            yield return tuple.Item13;
            yield return tuple.Item14;
            yield return tuple.Item15;
            yield return tuple.Item16;
            yield return tuple.Item17;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
            yield return tuple.Item8;
            yield return tuple.Item9;
            yield return tuple.Item10;
            yield return tuple.Item11;
            yield return tuple.Item12;
            yield return tuple.Item13;
            yield return tuple.Item14;
            yield return tuple.Item15;
            yield return tuple.Item16;
            yield return tuple.Item17;
            yield return tuple.Item18;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
            yield return tuple.Item8;
            yield return tuple.Item9;
            yield return tuple.Item10;
            yield return tuple.Item11;
            yield return tuple.Item12;
            yield return tuple.Item13;
            yield return tuple.Item14;
            yield return tuple.Item15;
            yield return tuple.Item16;
            yield return tuple.Item17;
            yield return tuple.Item18;
            yield return tuple.Item19;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
            yield return tuple.Item8;
            yield return tuple.Item9;
            yield return tuple.Item10;
            yield return tuple.Item11;
            yield return tuple.Item12;
            yield return tuple.Item13;
            yield return tuple.Item14;
            yield return tuple.Item15;
            yield return tuple.Item16;
            yield return tuple.Item17;
            yield return tuple.Item18;
            yield return tuple.Item19;
            yield return tuple.Item20;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
            yield return tuple.Item8;
            yield return tuple.Item9;
            yield return tuple.Item10;
            yield return tuple.Item11;
            yield return tuple.Item12;
            yield return tuple.Item13;
            yield return tuple.Item14;
            yield return tuple.Item15;
            yield return tuple.Item16;
            yield return tuple.Item17;
            yield return tuple.Item18;
            yield return tuple.Item19;
            yield return tuple.Item20;
            yield return tuple.Item21;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
            yield return tuple.Item8;
            yield return tuple.Item9;
            yield return tuple.Item10;
            yield return tuple.Item11;
            yield return tuple.Item12;
            yield return tuple.Item13;
            yield return tuple.Item14;
            yield return tuple.Item15;
            yield return tuple.Item16;
            yield return tuple.Item17;
            yield return tuple.Item18;
            yield return tuple.Item19;
            yield return tuple.Item20;
            yield return tuple.Item21;
            yield return tuple.Item22;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
            yield return tuple.Item8;
            yield return tuple.Item9;
            yield return tuple.Item10;
            yield return tuple.Item11;
            yield return tuple.Item12;
            yield return tuple.Item13;
            yield return tuple.Item14;
            yield return tuple.Item15;
            yield return tuple.Item16;
            yield return tuple.Item17;
            yield return tuple.Item18;
            yield return tuple.Item19;
            yield return tuple.Item20;
            yield return tuple.Item21;
            yield return tuple.Item22;
            yield return tuple.Item23;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
            yield return tuple.Item8;
            yield return tuple.Item9;
            yield return tuple.Item10;
            yield return tuple.Item11;
            yield return tuple.Item12;
            yield return tuple.Item13;
            yield return tuple.Item14;
            yield return tuple.Item15;
            yield return tuple.Item16;
            yield return tuple.Item17;
            yield return tuple.Item18;
            yield return tuple.Item19;
            yield return tuple.Item20;
            yield return tuple.Item21;
            yield return tuple.Item22;
            yield return tuple.Item23;
            yield return tuple.Item24;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
            yield return tuple.Item8;
            yield return tuple.Item9;
            yield return tuple.Item10;
            yield return tuple.Item11;
            yield return tuple.Item12;
            yield return tuple.Item13;
            yield return tuple.Item14;
            yield return tuple.Item15;
            yield return tuple.Item16;
            yield return tuple.Item17;
            yield return tuple.Item18;
            yield return tuple.Item19;
            yield return tuple.Item20;
            yield return tuple.Item21;
            yield return tuple.Item22;
            yield return tuple.Item23;
            yield return tuple.Item24;
            yield return tuple.Item25;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
            yield return tuple.Item8;
            yield return tuple.Item9;
            yield return tuple.Item10;
            yield return tuple.Item11;
            yield return tuple.Item12;
            yield return tuple.Item13;
            yield return tuple.Item14;
            yield return tuple.Item15;
            yield return tuple.Item16;
            yield return tuple.Item17;
            yield return tuple.Item18;
            yield return tuple.Item19;
            yield return tuple.Item20;
            yield return tuple.Item21;
            yield return tuple.Item22;
            yield return tuple.Item23;
            yield return tuple.Item24;
            yield return tuple.Item25;
            yield return tuple.Item26;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
            yield return tuple.Item8;
            yield return tuple.Item9;
            yield return tuple.Item10;
            yield return tuple.Item11;
            yield return tuple.Item12;
            yield return tuple.Item13;
            yield return tuple.Item14;
            yield return tuple.Item15;
            yield return tuple.Item16;
            yield return tuple.Item17;
            yield return tuple.Item18;
            yield return tuple.Item19;
            yield return tuple.Item20;
            yield return tuple.Item21;
            yield return tuple.Item22;
            yield return tuple.Item23;
            yield return tuple.Item24;
            yield return tuple.Item25;
            yield return tuple.Item26;
            yield return tuple.Item27;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
            yield return tuple.Item8;
            yield return tuple.Item9;
            yield return tuple.Item10;
            yield return tuple.Item11;
            yield return tuple.Item12;
            yield return tuple.Item13;
            yield return tuple.Item14;
            yield return tuple.Item15;
            yield return tuple.Item16;
            yield return tuple.Item17;
            yield return tuple.Item18;
            yield return tuple.Item19;
            yield return tuple.Item20;
            yield return tuple.Item21;
            yield return tuple.Item22;
            yield return tuple.Item23;
            yield return tuple.Item24;
            yield return tuple.Item25;
            yield return tuple.Item26;
            yield return tuple.Item27;
            yield return tuple.Item28;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
            yield return tuple.Item8;
            yield return tuple.Item9;
            yield return tuple.Item10;
            yield return tuple.Item11;
            yield return tuple.Item12;
            yield return tuple.Item13;
            yield return tuple.Item14;
            yield return tuple.Item15;
            yield return tuple.Item16;
            yield return tuple.Item17;
            yield return tuple.Item18;
            yield return tuple.Item19;
            yield return tuple.Item20;
            yield return tuple.Item21;
            yield return tuple.Item22;
            yield return tuple.Item23;
            yield return tuple.Item24;
            yield return tuple.Item25;
            yield return tuple.Item26;
            yield return tuple.Item27;
            yield return tuple.Item28;
            yield return tuple.Item29;
        }

        public static IEnumerable<T> Enumerate<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
            yield return tuple.Item8;
            yield return tuple.Item9;
            yield return tuple.Item10;
            yield return tuple.Item11;
            yield return tuple.Item12;
            yield return tuple.Item13;
            yield return tuple.Item14;
            yield return tuple.Item15;
            yield return tuple.Item16;
            yield return tuple.Item17;
            yield return tuple.Item18;
            yield return tuple.Item19;
            yield return tuple.Item20;
            yield return tuple.Item21;
            yield return tuple.Item22;
            yield return tuple.Item23;
            yield return tuple.Item24;
            yield return tuple.Item25;
            yield return tuple.Item26;
            yield return tuple.Item27;
            yield return tuple.Item28;
            yield return tuple.Item29;
            yield return tuple.Item30;
        }

        #endregion

        #region Count

        #region Homogeneous Tuples

        public static int Count<T>(this (T, T) tuple)
        {
            return 2;
        }

        public static int Count<T>(this (T, T, T) tuple)
        {
            return 3;
        }

        public static int Count<T>(this (T, T, T, T) tuple)
        {
            return 4;
        }

        public static int Count<T>(this (T, T, T, T, T) tuple)
        {
            return 5;
        }

        public static int Count<T>(this (T, T, T, T, T, T) tuple)
        {
            return 6;
        }

        public static int Count<T>(this (T, T, T, T, T, T, T) tuple)
        {
            return 7;
        }

        public static int Count<T>(this (T, T, T, T, T, T, T, T) tuple)
        {
            return 8;
        }

        public static int Count<T>(this (T, T, T, T, T, T, T, T, T) tuple)
        {
            return 9;
        }

        public static int Count<T>(this (T, T, T, T, T, T, T, T, T, T) tuple)
        {
            return 10;
        }

        public static int Count<T>(this (T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            return 11;
        }

        public static int Count<T>(this (T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            return 12;
        }

        public static int Count<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            return 13;
        }

        public static int Count<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            return 14;
        }

        public static int Count<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            return 15;
        }

        public static int Count<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            return 16;
        }

        public static int Count<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            return 17;
        }

        public static int Count<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            return 18;
        }

        public static int Count<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            return 19;
        }

        public static int Count<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            return 20;
        }

        public static int Count<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            return 21;
        }

        public static int Count<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            return 22;
        }

        public static int Count<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            return 23;
        }

        public static int Count<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            return 24;
        }

        public static int Count<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            return 25;
        }

        public static int Count<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            return 26;
        }

        public static int Count<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            return 27;
        }

        public static int Count<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            return 28;
        }

        public static int Count<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            return 29;
        }

        public static int Count<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple)
        {
            return 30;
        }


        #endregion

        #region Heterogeneous Tuples

        public static int Count<T1, T2>(this (T1, T2) tuple)
        {
            return 2;
        }

        public static int Count<T1, T2, T3>(this (T1, T2, T3) tuple)
        {
            return 3;
        }

        public static int Count<T1, T2, T3, T4>(this (T1, T2, T3, T4) tuple)
        {
            return 4;
        }

        public static int Count<T1, T2, T3, T4, T5>(this (T1, T2, T3, T4, T5) tuple)
        {
            return 5;
        }

        public static int Count<T1, T2, T3, T4, T5, T6>(this (T1, T2, T3, T4, T5, T6) tuple)
        {
            return 6;
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7>(this (T1, T2, T3, T4, T5, T6, T7) tuple)
        {
            return 7;
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8>(this (T1, T2, T3, T4, T5, T6, T7, T8) tuple)
        {
            return 8;
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9) tuple)
        {
            return 9;
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) tuple)
        {
            return 10;
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) tuple)
        {
            return 11;
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) tuple)
        {
            return 12;
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13) tuple)
        {
            return 13;
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14) tuple)
        {
            return 14;
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15) tuple)
        {
            return 15;
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16) tuple)
        {
            return 16;
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17) tuple)
        {
            return 17;
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18) tuple)
        {
            return 18;
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19) tuple)
        {
            return 19;
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20) tuple)
        {
            return 20;
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21) tuple)
        {
            return 21;
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22) tuple)
        {
            return 22;
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23) tuple)
        {
            return 23;
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24) tuple)
        {
            return 24;
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25) tuple)
        {
            return 25;
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26) tuple)
        {
            return 26;
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27) tuple)
        {
            return 27;
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28) tuple)
        {
            return 28;
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29) tuple)
        {
            return 29;
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30) tuple)
        {
            return 30;
        }

        #endregion

        #endregion

        #region Get

        public static T Get<T>(this (T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;
                case 5:
                    return tuple.Item6;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;
                case 5:
                    return tuple.Item6;
                case 6:
                    return tuple.Item7;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;
                case 5:
                    return tuple.Item6;
                case 6:
                    return tuple.Item7;
                case 7:
                    return tuple.Item8;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;
                case 5:
                    return tuple.Item6;
                case 6:
                    return tuple.Item7;
                case 7:
                    return tuple.Item8;
                case 8:
                    return tuple.Item9;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T, T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;
                case 5:
                    return tuple.Item6;
                case 6:
                    return tuple.Item7;
                case 7:
                    return tuple.Item8;
                case 8:
                    return tuple.Item9;
                case 9:
                    return tuple.Item10;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T, T, T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;
                case 5:
                    return tuple.Item6;
                case 6:
                    return tuple.Item7;
                case 7:
                    return tuple.Item8;
                case 8:
                    return tuple.Item9;
                case 9:
                    return tuple.Item10;
                case 10:
                    return tuple.Item11;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T, T, T, T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;
                case 5:
                    return tuple.Item6;
                case 6:
                    return tuple.Item7;
                case 7:
                    return tuple.Item8;
                case 8:
                    return tuple.Item9;
                case 9:
                    return tuple.Item10;
                case 10:
                    return tuple.Item11;
                case 11:
                    return tuple.Item12;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;
                case 5:
                    return tuple.Item6;
                case 6:
                    return tuple.Item7;
                case 7:
                    return tuple.Item8;
                case 8:
                    return tuple.Item9;
                case 9:
                    return tuple.Item10;
                case 10:
                    return tuple.Item11;
                case 11:
                    return tuple.Item12;
                case 12:
                    return tuple.Item13;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;
                case 5:
                    return tuple.Item6;
                case 6:
                    return tuple.Item7;
                case 7:
                    return tuple.Item8;
                case 8:
                    return tuple.Item9;
                case 9:
                    return tuple.Item10;
                case 10:
                    return tuple.Item11;
                case 11:
                    return tuple.Item12;
                case 12:
                    return tuple.Item13;
                case 13:
                    return tuple.Item14;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;
                case 5:
                    return tuple.Item6;
                case 6:
                    return tuple.Item7;
                case 7:
                    return tuple.Item8;
                case 8:
                    return tuple.Item9;
                case 9:
                    return tuple.Item10;
                case 10:
                    return tuple.Item11;
                case 11:
                    return tuple.Item12;
                case 12:
                    return tuple.Item13;
                case 13:
                    return tuple.Item14;
                case 14:
                    return tuple.Item15;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;
                case 5:
                    return tuple.Item6;
                case 6:
                    return tuple.Item7;
                case 7:
                    return tuple.Item8;
                case 8:
                    return tuple.Item9;
                case 9:
                    return tuple.Item10;
                case 10:
                    return tuple.Item11;
                case 11:
                    return tuple.Item12;
                case 12:
                    return tuple.Item13;
                case 13:
                    return tuple.Item14;
                case 14:
                    return tuple.Item15;
                case 15:
                    return tuple.Item16;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;
                case 5:
                    return tuple.Item6;
                case 6:
                    return tuple.Item7;
                case 7:
                    return tuple.Item8;
                case 8:
                    return tuple.Item9;
                case 9:
                    return tuple.Item10;
                case 10:
                    return tuple.Item11;
                case 11:
                    return tuple.Item12;
                case 12:
                    return tuple.Item13;
                case 13:
                    return tuple.Item14;
                case 14:
                    return tuple.Item15;
                case 15:
                    return tuple.Item16;
                case 16:
                    return tuple.Item17;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;
                case 5:
                    return tuple.Item6;
                case 6:
                    return tuple.Item7;
                case 7:
                    return tuple.Item8;
                case 8:
                    return tuple.Item9;
                case 9:
                    return tuple.Item10;
                case 10:
                    return tuple.Item11;
                case 11:
                    return tuple.Item12;
                case 12:
                    return tuple.Item13;
                case 13:
                    return tuple.Item14;
                case 14:
                    return tuple.Item15;
                case 15:
                    return tuple.Item16;
                case 16:
                    return tuple.Item17;
                case 17:
                    return tuple.Item18;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;
                case 5:
                    return tuple.Item6;
                case 6:
                    return tuple.Item7;
                case 7:
                    return tuple.Item8;
                case 8:
                    return tuple.Item9;
                case 9:
                    return tuple.Item10;
                case 10:
                    return tuple.Item11;
                case 11:
                    return tuple.Item12;
                case 12:
                    return tuple.Item13;
                case 13:
                    return tuple.Item14;
                case 14:
                    return tuple.Item15;
                case 15:
                    return tuple.Item16;
                case 16:
                    return tuple.Item17;
                case 17:
                    return tuple.Item18;
                case 18:
                    return tuple.Item19;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;
                case 5:
                    return tuple.Item6;
                case 6:
                    return tuple.Item7;
                case 7:
                    return tuple.Item8;
                case 8:
                    return tuple.Item9;
                case 9:
                    return tuple.Item10;
                case 10:
                    return tuple.Item11;
                case 11:
                    return tuple.Item12;
                case 12:
                    return tuple.Item13;
                case 13:
                    return tuple.Item14;
                case 14:
                    return tuple.Item15;
                case 15:
                    return tuple.Item16;
                case 16:
                    return tuple.Item17;
                case 17:
                    return tuple.Item18;
                case 18:
                    return tuple.Item19;
                case 19:
                    return tuple.Item20;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;
                case 5:
                    return tuple.Item6;
                case 6:
                    return tuple.Item7;
                case 7:
                    return tuple.Item8;
                case 8:
                    return tuple.Item9;
                case 9:
                    return tuple.Item10;
                case 10:
                    return tuple.Item11;
                case 11:
                    return tuple.Item12;
                case 12:
                    return tuple.Item13;
                case 13:
                    return tuple.Item14;
                case 14:
                    return tuple.Item15;
                case 15:
                    return tuple.Item16;
                case 16:
                    return tuple.Item17;
                case 17:
                    return tuple.Item18;
                case 18:
                    return tuple.Item19;
                case 19:
                    return tuple.Item20;
                case 20:
                    return tuple.Item21;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;
                case 5:
                    return tuple.Item6;
                case 6:
                    return tuple.Item7;
                case 7:
                    return tuple.Item8;
                case 8:
                    return tuple.Item9;
                case 9:
                    return tuple.Item10;
                case 10:
                    return tuple.Item11;
                case 11:
                    return tuple.Item12;
                case 12:
                    return tuple.Item13;
                case 13:
                    return tuple.Item14;
                case 14:
                    return tuple.Item15;
                case 15:
                    return tuple.Item16;
                case 16:
                    return tuple.Item17;
                case 17:
                    return tuple.Item18;
                case 18:
                    return tuple.Item19;
                case 19:
                    return tuple.Item20;
                case 20:
                    return tuple.Item21;
                case 21:
                    return tuple.Item22;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;
                case 5:
                    return tuple.Item6;
                case 6:
                    return tuple.Item7;
                case 7:
                    return tuple.Item8;
                case 8:
                    return tuple.Item9;
                case 9:
                    return tuple.Item10;
                case 10:
                    return tuple.Item11;
                case 11:
                    return tuple.Item12;
                case 12:
                    return tuple.Item13;
                case 13:
                    return tuple.Item14;
                case 14:
                    return tuple.Item15;
                case 15:
                    return tuple.Item16;
                case 16:
                    return tuple.Item17;
                case 17:
                    return tuple.Item18;
                case 18:
                    return tuple.Item19;
                case 19:
                    return tuple.Item20;
                case 20:
                    return tuple.Item21;
                case 21:
                    return tuple.Item22;
                case 22:
                    return tuple.Item23;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;
                case 5:
                    return tuple.Item6;
                case 6:
                    return tuple.Item7;
                case 7:
                    return tuple.Item8;
                case 8:
                    return tuple.Item9;
                case 9:
                    return tuple.Item10;
                case 10:
                    return tuple.Item11;
                case 11:
                    return tuple.Item12;
                case 12:
                    return tuple.Item13;
                case 13:
                    return tuple.Item14;
                case 14:
                    return tuple.Item15;
                case 15:
                    return tuple.Item16;
                case 16:
                    return tuple.Item17;
                case 17:
                    return tuple.Item18;
                case 18:
                    return tuple.Item19;
                case 19:
                    return tuple.Item20;
                case 20:
                    return tuple.Item21;
                case 21:
                    return tuple.Item22;
                case 22:
                    return tuple.Item23;
                case 23:
                    return tuple.Item24;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;
                case 5:
                    return tuple.Item6;
                case 6:
                    return tuple.Item7;
                case 7:
                    return tuple.Item8;
                case 8:
                    return tuple.Item9;
                case 9:
                    return tuple.Item10;
                case 10:
                    return tuple.Item11;
                case 11:
                    return tuple.Item12;
                case 12:
                    return tuple.Item13;
                case 13:
                    return tuple.Item14;
                case 14:
                    return tuple.Item15;
                case 15:
                    return tuple.Item16;
                case 16:
                    return tuple.Item17;
                case 17:
                    return tuple.Item18;
                case 18:
                    return tuple.Item19;
                case 19:
                    return tuple.Item20;
                case 20:
                    return tuple.Item21;
                case 21:
                    return tuple.Item22;
                case 22:
                    return tuple.Item23;
                case 23:
                    return tuple.Item24;
                case 24:
                    return tuple.Item25;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;
                case 5:
                    return tuple.Item6;
                case 6:
                    return tuple.Item7;
                case 7:
                    return tuple.Item8;
                case 8:
                    return tuple.Item9;
                case 9:
                    return tuple.Item10;
                case 10:
                    return tuple.Item11;
                case 11:
                    return tuple.Item12;
                case 12:
                    return tuple.Item13;
                case 13:
                    return tuple.Item14;
                case 14:
                    return tuple.Item15;
                case 15:
                    return tuple.Item16;
                case 16:
                    return tuple.Item17;
                case 17:
                    return tuple.Item18;
                case 18:
                    return tuple.Item19;
                case 19:
                    return tuple.Item20;
                case 20:
                    return tuple.Item21;
                case 21:
                    return tuple.Item22;
                case 22:
                    return tuple.Item23;
                case 23:
                    return tuple.Item24;
                case 24:
                    return tuple.Item25;
                case 25:
                    return tuple.Item26;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;
                case 5:
                    return tuple.Item6;
                case 6:
                    return tuple.Item7;
                case 7:
                    return tuple.Item8;
                case 8:
                    return tuple.Item9;
                case 9:
                    return tuple.Item10;
                case 10:
                    return tuple.Item11;
                case 11:
                    return tuple.Item12;
                case 12:
                    return tuple.Item13;
                case 13:
                    return tuple.Item14;
                case 14:
                    return tuple.Item15;
                case 15:
                    return tuple.Item16;
                case 16:
                    return tuple.Item17;
                case 17:
                    return tuple.Item18;
                case 18:
                    return tuple.Item19;
                case 19:
                    return tuple.Item20;
                case 20:
                    return tuple.Item21;
                case 21:
                    return tuple.Item22;
                case 22:
                    return tuple.Item23;
                case 23:
                    return tuple.Item24;
                case 24:
                    return tuple.Item25;
                case 25:
                    return tuple.Item26;
                case 26:
                    return tuple.Item27;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;
                case 5:
                    return tuple.Item6;
                case 6:
                    return tuple.Item7;
                case 7:
                    return tuple.Item8;
                case 8:
                    return tuple.Item9;
                case 9:
                    return tuple.Item10;
                case 10:
                    return tuple.Item11;
                case 11:
                    return tuple.Item12;
                case 12:
                    return tuple.Item13;
                case 13:
                    return tuple.Item14;
                case 14:
                    return tuple.Item15;
                case 15:
                    return tuple.Item16;
                case 16:
                    return tuple.Item17;
                case 17:
                    return tuple.Item18;
                case 18:
                    return tuple.Item19;
                case 19:
                    return tuple.Item20;
                case 20:
                    return tuple.Item21;
                case 21:
                    return tuple.Item22;
                case 22:
                    return tuple.Item23;
                case 23:
                    return tuple.Item24;
                case 24:
                    return tuple.Item25;
                case 25:
                    return tuple.Item26;
                case 26:
                    return tuple.Item27;
                case 27:
                    return tuple.Item28;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;
                case 5:
                    return tuple.Item6;
                case 6:
                    return tuple.Item7;
                case 7:
                    return tuple.Item8;
                case 8:
                    return tuple.Item9;
                case 9:
                    return tuple.Item10;
                case 10:
                    return tuple.Item11;
                case 11:
                    return tuple.Item12;
                case 12:
                    return tuple.Item13;
                case 13:
                    return tuple.Item14;
                case 14:
                    return tuple.Item15;
                case 15:
                    return tuple.Item16;
                case 16:
                    return tuple.Item17;
                case 17:
                    return tuple.Item18;
                case 18:
                    return tuple.Item19;
                case 19:
                    return tuple.Item20;
                case 20:
                    return tuple.Item21;
                case 21:
                    return tuple.Item22;
                case 22:
                    return tuple.Item23;
                case 23:
                    return tuple.Item24;
                case 24:
                    return tuple.Item25;
                case 25:
                    return tuple.Item26;
                case 26:
                    return tuple.Item27;
                case 27:
                    return tuple.Item28;
                case 28:
                    return tuple.Item29;

                default:
                    return default(T);
            }
        }

        public static T Get<T>(this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple, int index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;
                case 1:
                    return tuple.Item2;
                case 2:
                    return tuple.Item3;
                case 3:
                    return tuple.Item4;
                case 4:
                    return tuple.Item5;
                case 5:
                    return tuple.Item6;
                case 6:
                    return tuple.Item7;
                case 7:
                    return tuple.Item8;
                case 8:
                    return tuple.Item9;
                case 9:
                    return tuple.Item10;
                case 10:
                    return tuple.Item11;
                case 11:
                    return tuple.Item12;
                case 12:
                    return tuple.Item13;
                case 13:
                    return tuple.Item14;
                case 14:
                    return tuple.Item15;
                case 15:
                    return tuple.Item16;
                case 16:
                    return tuple.Item17;
                case 17:
                    return tuple.Item18;
                case 18:
                    return tuple.Item19;
                case 19:
                    return tuple.Item20;
                case 20:
                    return tuple.Item21;
                case 21:
                    return tuple.Item22;
                case 22:
                    return tuple.Item23;
                case 23:
                    return tuple.Item24;
                case 24:
                    return tuple.Item25;
                case 25:
                    return tuple.Item26;
                case 26:
                    return tuple.Item27;
                case 27:
                    return tuple.Item28;
                case 28:
                    return tuple.Item29;
                case 29:
                    return tuple.Item30;

                default:
                    return default(T);
            }
        }

        #endregion
    }
}