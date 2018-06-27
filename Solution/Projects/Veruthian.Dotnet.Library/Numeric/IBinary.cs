using Veruthian.Dotnet.Library.Collections;

namespace Veruthian.Dotnet.Library.Numeric
{
    public interface IBinary<B>
        where B : IBinary<B>, IIndex<bool>
    {
        B And(B other);

        B Nand(B other);

        B Or(B other);

        B Nor(B other);

        B Xor(B other);

        B Equivalence(B other);

        B Not();
    }
}