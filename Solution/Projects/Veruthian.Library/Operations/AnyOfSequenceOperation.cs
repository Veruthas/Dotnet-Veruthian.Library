using System.Collections.Generic;

namespace Veruthian.Library.Operations
{
    public class AnyOfSequenceOperation<TState> : VariableSequentialOperationBase<TState>
    {
        public AnyOfSequenceOperation()
            : base(SequenceType.AllOf, new List<IOperation<TState>>()) { }

        public AnyOfSequenceOperation(IEnumerable<IOperation<TState>> operations)
            : base(SequenceType.AllOf, new List<IOperation<TState>>(operations)) { }

        public AnyOfSequenceOperation(params IOperation<TState>[] operations)
            : base(SequenceType.AllOf, new List<IOperation<TState>>(operations)) { }


        public AnyOfSequenceOperation<TState> Or(IOperation<TState> operation)
        {
            base.Operations.Add(operation);

            return this;
        }

        public AnyOfSequenceOperation<TState> OrSelf()
        {
            base.Operations.Add(this);

            return this;
        }
    }
}