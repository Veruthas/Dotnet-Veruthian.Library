namespace Soedeum.Dotnet.Library.Utility
{
    public class HashCodeCombiner
    {
        public static readonly HashCodeCombiner Combiner = new HashCodeCombiner();

        private readonly int primeBase;
        private readonly int primeOffset;

        public HashCodeCombiner(int primeBase = 269, int primeOffset = 47)
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
    }
}