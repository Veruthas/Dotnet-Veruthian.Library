namespace Veruthian.Dotnet.Library.Data.Collections
{
    public abstract class ExpandableIndexBase<T> : MutableIndexBase<T>, IExpandableIndex<T>
    {
        protected ExpandableIndexBase(int startIndex = 0)
            : base(startIndex) { }
            
        public abstract void Add(T value);

        public abstract void Clear();

        public abstract void Insert(int key, T value);

        public abstract bool Remove(T value);
        
        public abstract void RemoveBy(int key);
    }
}