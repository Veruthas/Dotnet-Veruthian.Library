namespace Veruthian.Library.Collections
{
    public interface IExpandableContainer<T> : IContainer<T>
    {
        void Add(T value);

        bool Remove(T value);

        void Clear();
    }
}