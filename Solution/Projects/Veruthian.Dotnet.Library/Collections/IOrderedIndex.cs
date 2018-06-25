namespace Veruthian.Dotnet.Library.Collections
{
    public interface IOrderedIndex<T> : IIndex<T>, IExpandableContainer<T>
    {
        void RemoveBy(int index);
    }
}