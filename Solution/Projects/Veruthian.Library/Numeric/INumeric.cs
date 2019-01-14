namespace Veruthian.Library.Numeric
{
    public interface INumeric<T> : IOrderable<T>
        where T : INumeric<T>
    {
        T Add(T other);
    
        T Subtract(T other);
    
        T Delta(T other);

        T Multiply(T other);

        T Divide(T other);        

        T Divide(T other, out T remainder);

        T Modulus(T other);
    }
}