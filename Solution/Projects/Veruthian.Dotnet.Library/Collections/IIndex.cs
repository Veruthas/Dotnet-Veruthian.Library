namespace Veruthian.Dotnet.Library.Collections
{
    public interface IIndex<K, T> : ILookup<K, T>
    {
        K Start { get; }
    }
}