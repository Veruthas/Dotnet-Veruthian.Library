using Soedeum.Dotnet.Library.Compilers.Lexers;

namespace Soedeum.Dotnet.Library.Compilers.Parsers
{
    public interface INode<TToken, TType>
        where TToken : IToken<TType>
    {
        TToken Token { get; }
    }
}