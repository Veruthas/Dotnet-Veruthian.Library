using Veruthian.Dotnet.Library.Collections;

namespace Veruthian.Dotnet.Library.Numeric
{
    public interface IBinary<T>
        where T : IBinary<T>, IIndex<bool>
    {
        T And(T other);

        T Or(T other);

        T Xor(T other);

        T Not();
    }
}