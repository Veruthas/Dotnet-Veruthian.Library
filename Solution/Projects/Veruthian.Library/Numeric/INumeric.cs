namespace Veruthian.Library.Numeric
{
    public interface INumeric<T> : IOrderable<T>
        where T : INumeric<T>
    {
        T Sum(T addend);

        T Difference(T subtrahend);

        T Delta(T subtrahend);

        T Product(T multiplicand);

        T Power(T exponent);

        T Quotient(T divisor);

        T Remainder(T divisor);

        (T Quotient, T Remainder) Division(T divisor);
    }
}