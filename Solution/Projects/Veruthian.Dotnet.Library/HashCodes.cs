using System.Collections.Generic;

namespace Veruthian.Dotnet.Library
{
    public class HashCodes
    {
        public static readonly HashCodes Default = new HashCodes();

        private readonly int primeBase;
        private readonly int primeOffset;

        public HashCodes(int primeBase = 269, int primeOffset = 47)
        {
            this.primeBase = primeBase;
            this.primeOffset = primeOffset;
        }

        public int Combine<T0, T1>(T0 a, T1 b)
        {
            int hash = primeBase;
            hash = (hash * primeOffset) + a.GetHashCode();
            hash = (hash * primeOffset) + b.GetHashCode();
            return hash;
        }

        public int Combine<T0, T1, T2>(T0 a, T1 b, T2 c)
        {
            int hash = primeBase;
            hash = (hash * primeOffset) + a.GetHashCode();
            hash = (hash * primeOffset) + b.GetHashCode();
            hash = (hash * primeOffset) + c.GetHashCode();
            return hash;
        }

        public int Combine<T0, T1, T2, T3>(T0 a, T1 b, T2 c, T3 d)
        {
            int hash = primeBase;
            hash = (hash * primeOffset) + a.GetHashCode();
            hash = (hash * primeOffset) + b.GetHashCode();
            hash = (hash * primeOffset) + c.GetHashCode();
            hash = (hash * primeOffset) + d.GetHashCode();
            return hash;
        }

        public int Combine<T0, T1, T2, T3, T4>(T0 a, T1 b, T2 c, T3 d, T4 e)
        {
            int hash = primeBase;
            hash = (hash * primeOffset) + a.GetHashCode();
            hash = (hash * primeOffset) + b.GetHashCode();
            hash = (hash * primeOffset) + c.GetHashCode();
            hash = (hash * primeOffset) + d.GetHashCode();
            hash = (hash * primeOffset) + e.GetHashCode();
            return hash;
        }      

        public int Combine<T>(IEnumerable<T> items)
        {
            int hash = primeBase;
            
            foreach (var item in items)
                hash = (hash * primeOffset) + item.GetHashCode();

            return hash;
        }
    }
}