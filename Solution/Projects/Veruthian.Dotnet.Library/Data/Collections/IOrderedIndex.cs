namespace Veruthian.Dotnet.Library.Data.Collections
{
    public interface IOrderedIndex<T> : IIndex<T>, IExpandableContainer<T>
    {
        void RemoveBy(int index);
    }
}