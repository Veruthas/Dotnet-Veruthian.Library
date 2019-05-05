namespace Veruthian.Library.Readers
{
    public interface IRecollectiveReader<out T, K, D> : ISpeculativeReader<T>
    {
        void StoreProgress(K key, bool success, D data = default(D));

        (bool? success, int Length, D Data) RecallProgress(K key);
    }
}