namespace Veruthian.Dotnet.Library.Data
{
    public interface IBounded<T>
    {
        T MinValue { get; }

        T Default { get; }

        T MaxValue { get; }
    }
}