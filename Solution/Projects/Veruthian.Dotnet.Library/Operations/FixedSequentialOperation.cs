using System.Collections.Generic;
using System.Linq;

namespace Veruthian.Dotnet.Library.Operations
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


        protected override IOperation<TState> GetSubOperation(int index) => operations[index];

        protected override int Count => operations.Length;

    }
}