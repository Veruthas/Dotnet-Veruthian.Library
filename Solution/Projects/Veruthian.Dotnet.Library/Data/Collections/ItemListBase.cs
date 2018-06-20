namespace Veruthian.Dotnet.Library.Data.Collections
{
    public abstract class ItemListBase<T> : IMutableArrayBase<T>, IExpandableContainer<T>, IExpandableLookup<int, T>
    {
        public abstract void Add(T value);

        public abstract void Insert(int index, T value);

        public abstract bool Remove(T value);

        public abstract void RemoveBy(int index);

        public abstract void Clear();
    }
}