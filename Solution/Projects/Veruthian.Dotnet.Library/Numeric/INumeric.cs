namespace Veruthian.Dotnet.Library.Numeric
{
    public interface INumeric<N> : IOrderable<N>
        where N : INumeric<N>
    {
        N Add(N other);
    
        N Subtract(N other);
    
        N Delta(N other);

        N Increment();
        
        N Decrement();

        N Multiply(N other);

        N Divide(N other);        

        N Divide(N other, out N remainder);

        N Modulus(N other);
    }
}