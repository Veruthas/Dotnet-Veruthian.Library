using System.Collections.Generic;
using System.Linq;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public class FixedSequentialOperation<TState> : SequentialOperation<TState>
    {
        IOperation<TState>[] operations;

        public FixedSequentialOperation(SequenceType type, params IOperation<TState>[] operations) :
            base(type)
        {
            this.operations = (IOperation<TState>[])operations.Clone();
        }

        public FixedSequentialOperation(SequenceType type, IEnumerable<IOperation<TState>> operations) :
            base(type)
        {
            this.operations = operations.ToArray();
        }


        public override IOperation<TState> GetSubOperation(int index)
        {
            VerifyIndex(index);

            return operations[index];
        }

        public override int Count => operations.Length;


        public override IEnumerator<IOperation<TState>> GetEnumerator() 
        {
            foreach(var operation in operations)
                yield return operation;
        }
    }
}