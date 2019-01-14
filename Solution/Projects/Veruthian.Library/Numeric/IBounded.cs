namespace Veruthian.Library.Numeric
{
    public interface IBounded<T>
    {
        T MinValue { get; }

        T MaxValue { get; }
    }
}