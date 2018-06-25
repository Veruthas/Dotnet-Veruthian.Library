namespace Veruthian.Dotnet.Library.Numeric
{
    public interface IIntegral<N> : INumeric<N>
        where N : struct, IIntegral<N>
    {
        int Sign { get; }

        IIntegral<N> Magnitude { get; }

        N Negate();
    }
}