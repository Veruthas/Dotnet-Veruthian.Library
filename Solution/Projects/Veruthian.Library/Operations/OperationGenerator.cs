using System;
using System.Collections.Generic;
using Veruthian.Library.Collections;
using Veruthian.Library.Processing;
using Veruthian.Library.Types;

namespace Veruthian.Library.Operations
{
    public class OperationGenerator<TState> : IOperationGenerator<TState>
    {
        // Boolean
        public virtual BooleanOperation<TState> True
            => BooleanOperation<TState>.True;

        public virtual BooleanOperation<TState> False
            => BooleanOperation<TState>.False;


        // Action
        public virtual ActionOperation<TState> Action(OperationAction<TState> action)
            => new ActionOperation<TState>(action);


        // Sequence
        public virtual SequentialOperation<TState> Sequence(params IOperation<TState>[] operations)
            => new SequentialOperation<TState>(operations);


        // Optional
        public virtual OptionalOperation<TState> Optional(IOperation<TState> operation)
            => new OptionalOperation<TState>(operation);


        // If
        public virtual ConditionOperation<TState> If(IOperation<TState> condition)
            => ConditionOperation<TState>.If(condition);

        public virtual ConditionOperation<TState> IfThen(IOperation<TState> condition, IOperation<TState> thenOperation)
            => ConditionOperation<TState>.IfThen(condition, thenOperation);

        public virtual ConditionOperation<TState> IfElse(IOperation<TState> condition, IOperation<TState> elseOperation)
            => ConditionOperation<TState>.IfElse(condition, elseOperation);

        public virtual ConditionOperation<TState> IfThenElse(IOperation<TState> condition, IOperation<TState> thenOperation, IOperation<TState> elseOperation)
            => ConditionOperation<TState>.IfThenElse(condition, thenOperation, elseOperation);


        // Unless
        public virtual ConditionOperation<TState> Unless(IOperation<TState> condition)
            => ConditionOperation<TState>.Unless(condition);

        public virtual ConditionOperation<TState> UnlessThen(IOperation<TState> condition, IOperation<TState> thenOperation)
            => ConditionOperation<TState>.UnlessThen(condition, thenOperation);

        public virtual ConditionOperation<TState> UnlessElse(IOperation<TState> condition, IOperation<TState> elseOperation)
            => ConditionOperation<TState>.UnlessElse(condition, elseOperation);

        public virtual ConditionOperation<TState> UnlessThenElse(IOperation<TState> condition, IOperation<TState> thenOperation, IOperation<TState> elseOperation)
            => ConditionOperation<TState>.UnlessThenElse(condition, thenOperation, elseOperation);


        // Repeat
        public virtual RepeatedOperation<TState> Repeat(IOperation<TState> operation)
            => RepeatedOperation<TState>.Repeat(operation);

        public virtual RepeatedOperation<TState> AtMost(int times, IOperation<TState> operation)
            => RepeatedOperation<TState>.AtMost(times, operation);

        public virtual RepeatedOperation<TState> Exactly(int times, IOperation<TState> operation)
            => RepeatedOperation<TState>.Exactly(times, operation);

        public virtual SequentialOperation<TState> AtLeast(int times, IOperation<TState> operation)
            => Sequence(Exactly(times, operation), Repeat(operation));

        public virtual SequentialOperation<TState> InRange(int min, int max, IOperation<TState> operation)
            => Sequence(Exactly(min, operation), AtMost(max - min, operation));


        // While
        public virtual RepeatedOperation<TState> While(IOperation<TState> condition, IOperation<TState> operation)
            => Repeat(IfThen(condition, operation));

        // Until
        public virtual RepeatedOperation<TState> Until(IOperation<TState> condition, IOperation<TState> operation)
            => Repeat(UnlessThen(condition, operation));


        // Choice
        public virtual IOperation<TState> Choice(params IOperation<TState>[] operations)
        {
            if (operations.Length == 0)
            {
                return True;
            }
            else
            {
                IOperation<TState> current = ChoiceFinal(operations[operations.Length - 1]);

                for (int i = operations.Length - 2; i >= 0; i--)
                {
                    current = ChoiceComponent(operations[i], current);
                }

                return current;
            }
        }

        protected virtual IOperation<TState> ChoiceFinal(IOperation<TState> final)
            => final;

        protected virtual IOperation<TState> ChoiceComponent(IOperation<TState> previous, IOperation<TState> next)
            => IfElse(previous, next);


        // Classify
        public virtual ClassifiedOperation<TState> Classify()
            => new ClassifiedOperation<TState>();

        public virtual ClassifiedOperation<TState> Classify(IOperation<TState> operation)
            => new ClassifiedOperation<TState>(operation);

        public virtual ClassifiedOperation<TState> Classify(params string[] classes)
            => new ClassifiedOperation<TState>(new DataSet<string>(classes));

        public virtual ClassifiedOperation<TState> Classify(IOperation<TState> operation, params string[] classes)
            => new ClassifiedOperation<TState>(operation, new DataSet<string>(classes));
    }
}