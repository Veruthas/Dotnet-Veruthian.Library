namespace Veruthian.Dotnet.Library.Data.Collections
{
    public abstract class ItemSortedArrayBase<T> : ItemArrayBase<T>, IExpandableContainer<T>
    {
        public abstract void Add(T value);

        public abstract bool Remove(T value);

        public abstract bool RemoveBy(int index);

        public abstract void Clear();
    }
}