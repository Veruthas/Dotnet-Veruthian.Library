using Veruthian.Library.Utility;

namespace Veruthian.Library.Operations
{
    public class ConditionOperation<TState> : BaseOperation<TState>
    {
        IOperation<TState> condition, onTrue, onFalse;

        bool when;


        public ConditionOperation(IOperation<TState> condition, bool when, IOperation<TState> onTrue = null, IOperation<TState> onFalse = null)
        {
            ExceptionHelper.VerifyNotNull(condition, nameof(condition));

            this.condition = condition;

            this.when = when;

            this.onTrue = onTrue;

            this.onFalse = onFalse;
        }


        public IOperation<TState> Condition => condition;

        public bool When => when;

        public IOperation<TState> OnTrue => onTrue;

        public IOperation<TState> OnFalse => onFalse;


        public override string Description => $"{(when ? "if" : "unless")}{(onTrue != null ? "-then" : "")}{(onFalse != null ? "-else" : "")}";


        protected override int Count => 1 + (onTrue != null ? 1 : 0) + (onFalse != null ? 1 : 0);

        protected override bool DoAction(TState state, ITracer<TState> tracer = null)
        {
            var result = condition.Perform(state, tracer);

            if (result == when)
                result = onTrue != null ? onTrue.Perform(state, tracer) : true;
            else
                result = onFalse != null ? onFalse.Perform(state, tracer) : false;

            return result;
        }

        protected override IOperation<TState> GetSubOperation(int verifiedIndex)
        {
            switch (verifiedIndex)
            {
                case 0:
                    return condition;
                case 1:
                    return onTrue != null ? onTrue : onFalse;
                case 2:
                    return onFalse;
                default:
                    return null;
            }
        }


        // If
        public static ConditionOperation<TState> IfThenElse(IOperation<TState> condition, IOperation<TState> thenOperation, IOperation<TState> elseOperation)
        {
            return new ConditionOperation<TState>(condition, true, thenOperation, elseOperation);
        }

        public static ConditionOperation<TState> IfThen(IOperation<TState> condition, IOperation<TState> thenOperation)
        {
            return new ConditionOperation<TState>(condition, true, thenOperation);
        }

        public static ConditionOperation<TState> IfElse(IOperation<TState> condition, IOperation<TState> elseOperation)
        {
            return new ConditionOperation<TState>(condition, true, null, elseOperation);
        }

        public static ConditionOperation<TState> If(IOperation<TState> condition)
        {
            return new ConditionOperation<TState>(condition, true);
        }


        // Unless
        public static ConditionOperation<TState> UnlessThenElse(IOperation<TState> condition, IOperation<TState> thenOperation, IOperation<TState> elseOperation)
        {
            return new ConditionOperation<TState>(condition, false, thenOperation, elseOperation);
        }

        public static ConditionOperation<TState> UnlessThen(IOperation<TState> condition, IOperation<TState> thenOperation)
        {
            return new ConditionOperation<TState>(condition, false, thenOperation);
        }

        public static ConditionOperation<TState> UnlessElse(IOperation<TState> condition, IOperation<TState> elseOperation)
        {
            return new ConditionOperation<TState>(condition, false, null, elseOperation);
        }

        public static ConditionOperation<TState> Unless(IOperation<TState> condition)
        {
            return new ConditionOperation<TState>(condition, false);
        }
    }
}