using System.Collections.Generic;
using System.Linq;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Operations
{
    public abstract class BaseListOperation<TState> : BaseOperation<TState>
    {
        protected DataIndex<IOperation<TState>> operations;


        public BaseListOperation(DataIndex<IOperation<TState>> operations) => this.operations = operations;


        protected override int Count => operations.Count;

        protected override IOperation<TState> GetSubOperation(int verifiedIndex) => operations[verifiedIndex];
    }
}