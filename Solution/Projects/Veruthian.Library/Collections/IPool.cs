namespace Veruthian.Library.Collections
{
    public interface IPool<A, V> : IContainer<(A, V)>
    {
        (A, V) Resolve(A address, V attribute = default(V));
    }
}