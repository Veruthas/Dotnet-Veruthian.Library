using System.Collections.Generic;
using System.Linq;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public class FixedSequentialOperation<TState> : SequentialOperation<TState>
    {
        IOperation<TState>[] operations;

        protected FixedSequentialOperation(SequenceType type, params IOperation<TState>[] operations) :
            base(type)
        {
            this.operations = (IOperation<TState>[])operations.Clone();
        }

        protected FixedSequentialOperation(SequenceType type, IEnumerable<IOperation<TState>> operations) :
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


        public override IEnumerator<IOperation<TState>> GetEnumerator() => (IEnumerator<IOperation<TState>>)operations.GetEnumerator();
    }
}