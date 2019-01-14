namespace Veruthian.Library.Collections
{
    public interface IPool<K, A> : IContainer<(K, A)>
    {
        (K, A) Resolve(K key, A attribute = default(A));
    }
}