namespace Veruthian.Library.Numeric
{
    public interface IIntegral<T> : INumeric<T>
        where T : IIntegral<T>
    {
        T Sign { get; }

        T Magnitude { get; }


        T Negate();
    }
}