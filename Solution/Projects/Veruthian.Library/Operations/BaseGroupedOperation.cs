using System.Collections.Generic;
using System.Linq;

namespace Veruthian.Library.Operations
{
    public abstract class BaseGroupedOperation<TState> : BaseOperation<TState>
    {
        private IOperation<TState>[] operations;


        protected BaseGroupedOperation(IEnumerable<IOperation<TState>> items) => this.operations = items.ToArray();

        protected BaseGroupedOperation(BaseGroupedOperation<TState> left, BaseGroupedOperation<TState> right)
        {
            this.operations = new IOperation<TState>[left.operations.Length + right.operations.Length];

            for (int i = 0; i < left.operations.Length; i++)
                operations[i] = left.operations[i];

            int offset = left.operations.Length;

            for (int i = 0; i < left.operations.Length; i++)
                operations[offset + i] = left.operations[i];
        }

        protected BaseGroupedOperation(IEnumerable<BaseGroupedOperation<TState>> groups)
        {
            var items = groups.ToArray();

            var total = 0;

            foreach (var item in items)
                total += item.operations.Length;

            this.operations = new IOperation<TState>[total];

            int offset = 0;

            foreach(var item in items)
            {
                for (int i = 0; i < item.operations.Length; i++)
                    this.operations[offset + i] = items[i];

                offset = item.operations.Length;
            }
        }


        protected override int Count => operations.Length;

        protected override IOperation<TState> GetSubOperation(int verifiedIndex) => operations[verifiedIndex];
    }
}