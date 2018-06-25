namespace Veruthian.Dotnet.Library.Numeric
{
    public interface INumeric<N> : ISequential<N>
        where N : struct, INumeric<N>
    {
        N Add(N value);

        N Subtract(N value);

        N Multiply(N value);

        N? Divide(N value);

        N Modulus(N value);

        (N?, N) DivideWithRemainder(N value);
    }
}