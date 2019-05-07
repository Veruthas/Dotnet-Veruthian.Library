using Veruthian.Library.Types;

namespace Veruthian.Library.Operations
{
    public class AnalyzerClass : Enum<AnalyzerClass>
    {
        public static readonly AnalyzerClass Rule = new AnalyzerClass("rule");

        public static readonly AnalyzerClass Token = new AnalyzerClass("token");

        public static readonly AnalyzerClass Literal = new AnalyzerClass("literal");


        protected AnalyzerClass(string name) : base(name) { }


        public static implicit operator string(AnalyzerClass value) => value.Name;
    }
}