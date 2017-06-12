using Soedeum.Dotnet.Library.Compilers.Lexers;
using Soedeum.Dotnet.Library.Text;

namespace Soedeum.Dotnet.Library.Gray
{
    public class GrayToken : Token<GrayType>
    {
        public GrayToken(string source, TextSpan span, GrayType type)
            : base(source, span, type) { }

        public object Value { get; set; }
    }
}