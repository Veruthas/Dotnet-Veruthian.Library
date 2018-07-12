namespace Veruthian.Dotnet.Library.Collections
{
    public abstract class BaseOrderedIndex<T> : BaseIndex<T>, IOrderedIndex<T>
    {
        public abstract void Add(T value);
        
        public abstract bool Remove(T value);

        public abstract void RemoveBy(int index);

        public abstract void Clear();
    }
}