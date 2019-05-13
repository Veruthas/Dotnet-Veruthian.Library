namespace Veruthian.Library.Collections
{
    public interface IPool<A, D> : IContainer<(A, D)>
    {
        (A, D) Resolve(A address, D data = default(D));
    }
}