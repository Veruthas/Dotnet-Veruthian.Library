namespace Veruthian.Dotnet.Library.Collections
{
    public interface IIndex<T> : ILookup<int, T>
    {
        int StartIndex { get; }

        int EndIndex { get; }
    }
}