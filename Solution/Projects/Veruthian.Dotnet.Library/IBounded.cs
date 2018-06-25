namespace Veruthian.Dotnet.Library
{
    public interface IBounded<T>
    {
        T MinValue { get; }

        T Default { get; }

        T MaxValue { get; }
    }
}