using System;

namespace _Console.Experiments
{
    public class InGenericTest
    {

        static void Test()
        {
            var a = new A();

            var b = new B();

            C(a, new object(), "Hello");

            C(b, new object(), "Hello");

        }


        static void C<T>(R r, T t)
        {
            switch (r)
            {
                case S<T> s:
                    Console.WriteLine(s.Test(t));
                    break;
                default:
                    Console.WriteLine(r.Test());
                    break;
            }
        }

        static void C<T, U>(R r, T t, U u)
        {
            switch (r)
            {
                case S<T> s:
                    Console.WriteLine(s.Test(t));
                    break;
                case S<U> s:
                    Console.WriteLine(s.Test(u));
                    break;
                default:
                    Console.WriteLine(r.Test());
                    break;
            }
        }


        interface R
        {
            string Test();
        }

        interface S<in T> : R
        {
            string Test(T value);
        }

        class A : S<string>
        {
            public string Test() => "Plain String";

            public string Test(string value)
            {
                return "String: " + value;
            }
        }

        class B : S<object>
        {
            public string Test() => "Plain Object";

            public string Test(object value)
            {
                return "Object: " + value;
            }
        }
    }
}