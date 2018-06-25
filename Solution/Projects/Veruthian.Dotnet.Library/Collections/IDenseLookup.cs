namespace Veruthian.Dotnet.Library.Collections
{
    public interface IDenseLookup<TKey, TValue> : ILookup<TKey, TValue>
        where TKey : ISequential<TKey>
    {
        TKey FirstKey { get; }
        
        TKey LastKey { get; }


    }
}