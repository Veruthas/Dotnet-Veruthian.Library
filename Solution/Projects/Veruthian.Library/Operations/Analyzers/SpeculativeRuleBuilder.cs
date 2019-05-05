using Veruthian.Library.Processing;
using Veruthian.Library.Types;

namespace Veruthian.Library.Operations.Analyzers
{
    public class SpeculativeRuleBuilder<TState> : RuleBuilder<TState>
        where TState : Has<ISpeculative>
    {
        public SpeculativeOperation<TState> Speculate(IOperation<TState> operation) => new SpeculativeOperation<TState>(operation);
    }
}