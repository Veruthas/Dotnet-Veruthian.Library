namespace Veruthian.Dotnet.Library.Numeric
{
    public interface IBounded<T>
    {
        T MinValue { get; }

        T Default { get; }

        T MaxValue { get; }
    }
}