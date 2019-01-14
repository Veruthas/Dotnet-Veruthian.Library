using Veruthian.Library.Collections;

namespace Veruthian.Library.Numeric
{
    public interface ILogical<T>
        where T : ILogical<T>
    {
        T And(T other);

        T Or(T other);

        T Xor(T other);

        T Not();
    }
}