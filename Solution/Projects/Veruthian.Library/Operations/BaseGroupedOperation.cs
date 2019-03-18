using System.Collections.Generic;
using System.Linq;

namespace Veruthian.Library.Operations
{
    public abstract class BaseGroupedOperation<TState> : BaseOperation<TState>
    {
        private IOperation<TState>[] operations;


        protected override int Count => operations.Length;

        protected override IOperation<TState> GetSubOperation(int verifiedIndex) => operations[verifiedIndex];
    }
}