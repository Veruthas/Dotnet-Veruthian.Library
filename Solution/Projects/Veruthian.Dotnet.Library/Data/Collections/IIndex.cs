namespace Veruthian.Dotnet.Library.Data.Collections
{
    public interface IIndex<T> : ILookup<int, T>
    {
        int StartIndex { get; }

        int? IndexOf(T value);
    }
}