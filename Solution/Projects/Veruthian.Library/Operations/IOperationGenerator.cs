namespace Veruthian.Library.Operations
{
    public interface IOperationGenerator<TState>
    {
        // Boolean
        BooleanOperation<TState> True { get; }

        BooleanOperation<TState> False { get; }


        // Action
        ActionOperation<TState> Action(OperationAction<TState> action);


        // Sequence
        SequentialOperation<TState> Sequence(params IOperation<TState>[] operations);


        // Optional
        OptionalOperation<TState> Optional(IOperation<TState> operation);


        // If
        ConditionOperation<TState> If(IOperation<TState> condition);

        ConditionOperation<TState> IfThen(IOperation<TState> condition, IOperation<TState> thenOperation);

        ConditionOperation<TState> IfElse(IOperation<TState> condition, IOperation<TState> elseOperation);

        ConditionOperation<TState> IfThenElse(IOperation<TState> condition, IOperation<TState> thenOperation, IOperation<TState> elseOperation);


        // Unless
        ConditionOperation<TState> Unless(IOperation<TState> condition);

        ConditionOperation<TState> UnlessThen(IOperation<TState> condition, IOperation<TState> thenOperation);

        ConditionOperation<TState> UnlessElse(IOperation<TState> condition, IOperation<TState> elseOperation);

        ConditionOperation<TState> UnlessThenElse(IOperation<TState> condition, IOperation<TState> thenOperation, IOperation<TState> elseOperation);


        // Repeat
        RepeatedOperation<TState> Repeat(IOperation<TState> operation);

        RepeatedOperation<TState> AtMost(int times, IOperation<TState> operation);

        RepeatedOperation<TState> Exactly(int times, IOperation<TState> operation);

        SequentialOperation<TState> AtLeast(int times, IOperation<TState> operation);

        SequentialOperation<TState> InRange(int min, int max, IOperation<TState> operation);


        // While
        RepeatedOperation<TState> While(IOperation<TState> condition, IOperation<TState> operation);

        // Until
        RepeatedOperation<TState> Until(IOperation<TState> condition, IOperation<TState> operation);


        // Choice
        IOperation<TState> Choice(params IOperation<TState>[] operations);


        // Classify
        ClassifiedOperation<TState> Classify();

        ClassifiedOperation<TState> Classify(IOperation<TState> operation);

        ClassifiedOperation<TState> Classify(params string[] classes);

        ClassifiedOperation<TState> Classify(IOperation<TState> operation, params string[] classes);
    }
}