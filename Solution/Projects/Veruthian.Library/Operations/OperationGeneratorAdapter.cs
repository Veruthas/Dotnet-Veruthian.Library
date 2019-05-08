namespace Veruthian.Library.Operations
{
    public class OperationGeneratorAdapter<TState> : IOperationGenerator<TState>
    {
        protected readonly IOperationGenerator<TState> generator;

        public OperationGeneratorAdapter(IOperationGenerator<TState> generator) => this.generator = generator;


        // Boolean
        public virtual BooleanOperation<TState> True
            => generator.True;

        public virtual BooleanOperation<TState> False
            => generator.False;


        // Action
        public virtual ActionOperation<TState> Action(OperationAction<TState> action)
            => generator.Action(action);


        // Sequence
        public virtual SequentialOperation<TState> Sequence(params IOperation<TState>[] operations)
            => generator.Sequence(operations);


        // Optional
        public virtual OptionalOperation<TState> Optional(IOperation<TState> operation)
            => generator.Optional(operation);


        // If
        public virtual ConditionOperation<TState> If(IOperation<TState> condition)
            => generator.If(condition);

        public virtual ConditionOperation<TState> IfThen(IOperation<TState> condition, IOperation<TState> thenOperation)
            => generator.IfThen(condition, thenOperation);

        public virtual ConditionOperation<TState> IfElse(IOperation<TState> condition, IOperation<TState> elseOperation)
            => generator.IfElse(condition, elseOperation);

        public virtual ConditionOperation<TState> IfThenElse(IOperation<TState> condition, IOperation<TState> thenOperation, IOperation<TState> elseOperation)
            => generator.IfThenElse(condition, thenOperation, elseOperation);


        // Unless
        public virtual ConditionOperation<TState> Unless(IOperation<TState> condition)
            => generator.Unless(condition);

        public virtual ConditionOperation<TState> UnlessThen(IOperation<TState> condition, IOperation<TState> thenOperation)
            => generator.UnlessThen(condition, thenOperation);

        public virtual ConditionOperation<TState> UnlessElse(IOperation<TState> condition, IOperation<TState> elseOperation)
            => generator.UnlessElse(condition, elseOperation);

        public virtual ConditionOperation<TState> UnlessThenElse(IOperation<TState> condition, IOperation<TState> thenOperation, IOperation<TState> elseOperation)
            => generator.UnlessThenElse(condition, thenOperation, elseOperation);


        // Repeat
        public virtual RepeatedOperation<TState> Repeat(IOperation<TState> operation)
            => generator.Repeat(operation);

        public virtual RepeatedOperation<TState> AtMost(int times, IOperation<TState> operation)
            => generator.AtMost(times, operation);

        public virtual RepeatedOperation<TState> Exactly(int times, IOperation<TState> operation)
            => generator.Exactly(times, operation);

        public virtual SequentialOperation<TState> AtLeast(int times, IOperation<TState> operation)
            => generator.AtLeast(times, operation);

        public virtual SequentialOperation<TState> InRange(int min, int max, IOperation<TState> operation)
            => generator.InRange(min, max, operation);


        // While
        public virtual RepeatedOperation<TState> While(IOperation<TState> condition, IOperation<TState> operation)
            => generator.While(condition, operation);

        // Until
        public virtual RepeatedOperation<TState> Until(IOperation<TState> condition, IOperation<TState> operation)
            => generator.Until(condition, operation);


        // Choice
        public virtual IOperation<TState> Choice(params IOperation<TState>[] operations)
            => generator.Choice(operations);


        // Classify
        public virtual ClassifiedOperation<TState> Classify()
            => generator.Classify();

        public virtual ClassifiedOperation<TState> Classify(IOperation<TState> operation)
            => generator.Classify(operation);

        public virtual ClassifiedOperation<TState> Classify(params string[] classes)
            => generator.Classify(classes);

        public virtual ClassifiedOperation<TState> Classify(IOperation<TState> operation, params string[] classes)
            => generator.Classify(operation, classes);
    }
}