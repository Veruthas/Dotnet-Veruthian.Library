namespace Veruthian.Dotnet.Library.Numeric
{
    public interface IReal<T>
        where T : IReal<T>
    {
        T Base { get; }

        T Exponent { get; }
    }
}