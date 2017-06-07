namespace Soedeum.Dotnet.Library.Collections
{
    public interface ITable<TKey, TValue>
    {        
        void Define(TKey key, TValue value);

        TValue Resolve(TKey key);
    }
}