namespace Veruthian.Dotnet.Library.Numeric
{
    public interface IBounded<T>
    {
        T MinValue { get; }

        T MaxValue { get; }
    }
}