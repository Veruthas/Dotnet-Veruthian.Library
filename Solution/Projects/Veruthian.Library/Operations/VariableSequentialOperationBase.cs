using System.Collections.Generic;
using System.Linq;

namespace Veruthian.Library.Operations
{
    public abstract class VariableSequentialOperationBase<TState> : SequentialOperation<TState>
    {
        List<IOperation<TState>> operations;

        protected VariableSequentialOperationBase(SequenceType type, List<IOperation<TState>> operations)
            : base(type)
        {
            this.operations = operations;
        }

        protected List<IOperation<TState>> Operations => operations;



        protected override int Count => operations.Count;

        protected override IOperation<TState> GetSubOperation(int index) => operations[index];    
    }
}