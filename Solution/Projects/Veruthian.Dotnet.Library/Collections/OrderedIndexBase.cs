namespace Veruthian.Dotnet.Library.Collections
{
    public abstract class OrderedIndexBase<T> : IndexBase<T>, IOrderedIndex<T>
    {
        public abstract void Add(T value);
        
        public abstract bool Remove(T value);

        public abstract void RemoveBy(int index);

        public abstract void Clear();
    }
}