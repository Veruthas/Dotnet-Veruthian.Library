using Soedeum.Dotnet.Library.Compilers.Lexers;
using Soedeum.Dotnet.Library.Text;

namespace _TestConsole.Numb
{
    public class NumbToken : Token<NumbType>
    {
        public NumbToken(string source, TextSpan span, NumbType type)
            : base(source, span, type) { }

        public double? Value { get; set; }

        public override string ToString()
        {
            if (Value.HasValue)
                return string.Format("{0}; Numeric Value: {1}", base.ToString(), Value.GetValueOrDefault());
            else
                return base.ToString();
        }

        public override string ToShortString()
        {
            if (Value.HasValue)
                return string.Format("{0}; Numeric Value: {1}", base.ToShortString(), Value.GetValueOrDefault());
            else
                return base.ToShortString();
        }
    }
}