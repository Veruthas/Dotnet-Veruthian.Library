using Veruthian.Library.Processing;
using Veruthian.Library.Types;

namespace Veruthian.Library.Operations
{
    public class SpeculativeBuilder<TState> : Builder<TState>
            where TState :  Has<ISpeculative>        
    {
        // Speculative
        public SpeculativeOperation<TState> Speculate(IOperation<TState> operation) =>
            new SpeculativeOperation<TState>(operation);


        // If
        public override ConditionOperation<TState> If(IOperation<TState> condition)
            => ConditionOperation<TState>.If(Speculate(condition));

        public override ConditionOperation<TState> IfThen(IOperation<TState> condition, IOperation<TState> thenOperation)
            => ConditionOperation<TState>.IfThen(Speculate(condition), thenOperation);

        public override ConditionOperation<TState> IfElse(IOperation<TState> condition, IOperation<TState> elseOperation)
            => ConditionOperation<TState>.IfElse(Speculate(condition), elseOperation);

        public override ConditionOperation<TState> IfThenElse(IOperation<TState> condition, IOperation<TState> thenOperation, IOperation<TState> elseOperation)
            => ConditionOperation<TState>.IfThenElse(Speculate(condition), thenOperation, elseOperation);


        // Unless
        public override ConditionOperation<TState> Unless(IOperation<TState> condition)
            => ConditionOperation<TState>.Unless(Speculate(condition));

        public override ConditionOperation<TState> UnlessThen(IOperation<TState> condition, IOperation<TState> thenOperation)
            => ConditionOperation<TState>.UnlessThen(Speculate(condition), thenOperation);

        public override ConditionOperation<TState> UnlessElse(IOperation<TState> condition, IOperation<TState> elseOperation)
            => ConditionOperation<TState>.UnlessElse(Speculate(condition), elseOperation);

        public override ConditionOperation<TState> UnlessThenElse(IOperation<TState> condition, IOperation<TState> thenOperation, IOperation<TState> elseOperation)
            => ConditionOperation<TState>.UnlessThenElse(Speculate(condition), thenOperation, elseOperation);


        // Optional
        public override OptionalOperation<TState> Optional(IOperation<TState> operation)
            => base.Optional(IfThen(operation, operation));


        // Repeat
        public override RepeatedOperation<TState> Repeat(IOperation<TState> operation)
            => RepeatedOperation<TState>.Repeat(IfThen(operation, operation));

        public override RepeatedOperation<TState> AtMost(int times, IOperation<TState> operation)
            => RepeatedOperation<TState>.AtMost(times, IfThen(operation, operation));


        // Choice
        protected override IOperation<TState> ChoiceFinal(IOperation<TState> final)
            => IfThen(final, final);

        protected override IOperation<TState> ChoiceComponent(IOperation<TState> previous, IOperation<TState> next)
            => IfThenElse(previous, previous, next);
    }
}