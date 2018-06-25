using System.Collections.Generic;
using System.Linq;

namespace Veruthian.Dotnet.Library.Data.Operations
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



        public override int Count => operations.Count;

        public override IOperation<TState> GetSubOperation(int index)
        {
            VerifyIndex(index);

            return operations[index];
        }

        
        public override IEnumerator<IOperation<TState>> GetEnumerator() => operations.GetEnumerator();    
    }
}