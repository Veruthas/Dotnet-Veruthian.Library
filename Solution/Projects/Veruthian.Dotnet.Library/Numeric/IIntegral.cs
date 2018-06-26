namespace Veruthian.Dotnet.Library.Numeric
{
    public interface IIntegral<I> : INumeric<I>
        where I : IIntegral<I>
    {
        I Sign { get; }

        I Magnitude { get; }

        I Negate();
    }
}