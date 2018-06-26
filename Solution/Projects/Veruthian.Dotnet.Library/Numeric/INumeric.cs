namespace Veruthian.Dotnet.Library.Numeric
{
    public interface INumeric<N> : IOrderable<N>
        where N : INumeric<N>
    {
        N Add(N value);

        N Subtract(N value);

        N Delta(N value);

        N Multiply(N value);

        N Divide(N value);        

        N Divide(N value, out N remainder);

        N Modulus(N value);
    }
}