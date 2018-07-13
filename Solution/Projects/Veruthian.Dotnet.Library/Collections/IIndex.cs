namespace Veruthian.Dotnet.Library.Collections
{
    public interface IIndex<K, V> : ILookup<K, V>
    {
        K Start { get; }
    }
}