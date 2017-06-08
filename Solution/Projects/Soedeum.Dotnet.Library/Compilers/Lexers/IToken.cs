using Soedeum.Dotnet.Library.Text;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    public interface IToken<T>
        where T : IEquatable<T>
    {
        string Source { get; }

        TextSpan Span { get; }

        bool Is(T type);

        void VerifyIs(T type);
    }
}