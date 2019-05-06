namespace Veruthian.Library.Operations
{
    public class BuilderAdapter<TState> : IBuilder<TState>
    {
        protected readonly IBuilder<TState> builder;

        public BuilderAdapter(IBuilder<TState> builder) => this.builder = builder;


        // Boolean
        public virtual BooleanOperation<TState> True
            => builder.True;

        public virtual BooleanOperation<TState> False
            => builder.False;


        // Action
        public virtual ActionOperation<TState> Action(OperationAction<TState> action)
            => builder.Action(action);


        // Sequence
        public virtual SequentialOperation<TState> Sequence(params IOperation<TState>[] operations)
            => builder.Sequence(operations);


        // Optional
        public virtual OptionalOperation<TState> Optional(IOperation<TState> operation)
            => builder.Optional(operation);


        // If
        public virtual ConditionOperation<TState> If(IOperation<TState> condition)
            => builder.If(condition);

        public virtual ConditionOperation<TState> IfThen(IOperation<TState> condition, IOperation<TState> thenOperation)
            => builder.IfThen(condition, thenOperation);

        public virtual ConditionOperation<TState> IfElse(IOperation<TState> condition, IOperation<TState> elseOperation)
            => builder.IfElse(condition, elseOperation);

        public virtual ConditionOperation<TState> IfThenElse(IOperation<TState> condition, IOperation<TState> thenOperation, IOperation<TState> elseOperation)
            => builder.IfThenElse(condition, thenOperation, elseOperation);


        // Unless
        public virtual ConditionOperation<TState> Unless(IOperation<TState> condition)
            => builder.Unless(condition);

        public virtual ConditionOperation<TState> UnlessThen(IOperation<TState> condition, IOperation<TState> thenOperation)
            => builder.UnlessThen(condition, thenOperation);

        public virtual ConditionOperation<TState> UnlessElse(IOperation<TState> condition, IOperation<TState> elseOperation)
            => builder.UnlessElse(condition, elseOperation);

        public virtual ConditionOperation<TState> UnlessThenElse(IOperation<TState> condition, IOperation<TState> thenOperation, IOperation<TState> elseOperation)
            => builder.UnlessThenElse(condition, thenOperation, elseOperation);


        // Repeat
        public virtual RepeatedOperation<TState> Repeat(IOperation<TState> operation)
            => builder.Repeat(operation);

        public virtual RepeatedOperation<TState> AtMost(int times, IOperation<TState> operation)
            => builder.AtMost(times, operation);

        public virtual RepeatedOperation<TState> Exactly(int times, IOperation<TState> operation)
            => builder.Exactly(times, operation);

        public virtual SequentialOperation<TState> AtLeast(int times, IOperation<TState> operation)
            => builder.AtLeast(times, operation);

        public virtual SequentialOperation<TState> InRange(int min, int max, IOperation<TState> operation)
            => builder.InRange(min, max, operation);


        // While
        public virtual RepeatedOperation<TState> While(IOperation<TState> condition, IOperation<TState> operation)
            => builder.While(condition, operation);

        // Until
        public virtual RepeatedOperation<TState> Until(IOperation<TState> condition, IOperation<TState> operation)
            => builder.Until(condition, operation);


        // Choice
        public virtual IOperation<TState> Choice(params IOperation<TState>[] operations)
            => builder.Choice(operations);


        // Classify
        public virtual ClassifiedOperation<TState> Classify()
            => builder.Classify();

        public virtual ClassifiedOperation<TState> Classify(IOperation<TState> operation)
            => builder.Classify(operation);

        public virtual ClassifiedOperation<TState> Classify(params string[] classes)
            => builder.Classify(classes);

        public virtual ClassifiedOperation<TState> Classify(IOperation<TState> operation, params string[] classes)
            => builder.Classify(operation, classes);
    }
}