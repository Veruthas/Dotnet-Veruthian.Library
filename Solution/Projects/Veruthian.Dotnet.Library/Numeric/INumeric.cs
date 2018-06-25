namespace Veruthian.Dotnet.Library.Numeric
{
    public interface INumeric<N> : IOrderable<N>
        where N : struct, INumeric<N>
    {
        N Add(N value);

        N Subtract(N value);

        N Multiply(N value);

        N Divide(N value);

        N Modulus(N value);

        N DivideWithRemainder(N value, out N remainder);
    }
}