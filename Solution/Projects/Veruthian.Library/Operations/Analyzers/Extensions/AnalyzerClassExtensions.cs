namespace Veruthian.Library.Operations.Analyzers.Extensions
{
    public static class AnalyzerClassExtensions
    {
        public static ClassifiedOperation<TState> Literal<TState>(this IBuilder<TState> builder, IOperation<TState> operation)
            => builder.Classify(AnalyzerClass.Literal);

        public static bool IsRule<TState>(this ClassifiedOperation<TState> operation)
            => operation.Contains(AnalyzerClass.Rule.Name);

        public static bool IsToken<TState>(this ClassifiedOperation<TState> operation)
            => operation.Contains(AnalyzerClass.Token.Name);
        public static bool IsLiteral<TState>(this ClassifiedOperation<TState> operation)
            => operation.Contains(AnalyzerClass.Literal.Name);
    }
}