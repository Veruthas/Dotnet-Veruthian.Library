using System.Collections.Generic;
using System.Linq;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Operations
{
    public abstract class BaseGroupedOperation<TState> : BaseOperation<TState>
    {
        private readonly DataIndex<IOperation<TState>> operations;


        public BaseGroupedOperation(DataIndex<IOperation<TState>> operations) => this.operations = operations;


        protected DataIndex<IOperation<TState>> Operations => operations;

        protected override int Count => operations.Count;

        protected override IOperation<TState> GetSubOperation(int verifiedIndex) => operations[verifiedIndex];
    }
}