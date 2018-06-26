using Veruthian.Dotnet.Library.Collections;

namespace Veruthian.Dotnet.Library.Numeric
{
    public interface IBinary<B>
        where B : IBinary<B>, IIndex<bool>
    {
        B And(B value);

        B Nand(B value);

        B Or(B value);

        B Nor(B value);

        B Xor(B value);

        B Equivalence(B value);

        B Not();
    }
}