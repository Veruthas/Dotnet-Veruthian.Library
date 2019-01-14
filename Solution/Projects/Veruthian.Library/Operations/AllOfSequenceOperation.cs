using System.Collections.Generic;

namespace Veruthian.Library.Operations
{
    public class AllOfSequenceOperation<TState> : VariableSequentialOperationBase<TState>
    {
        public AllOfSequenceOperation()
            : base(SequenceType.AllOf, new List<IOperation<TState>>()) { }

        public AllOfSequenceOperation(IEnumerable<IOperation<TState>> operations)
            : base(SequenceType.AllOf, new List<IOperation<TState>>(operations)) { }

        public AllOfSequenceOperation(params IOperation<TState>[] operations)
            : base(SequenceType.AllOf, new List<IOperation<TState>>(operations)) { }


        public AllOfSequenceOperation<TState> And(IOperation<TState> operation)
        {
            base.Operations.Add(operation);

            return this;
        }

        public AllOfSequenceOperation<TState> AndSelf()
        {
            base.Operations.Add(this);

            return this;
        }
    }
}