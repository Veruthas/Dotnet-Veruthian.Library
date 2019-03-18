using System.Collections.Generic;
using System.Linq;

namespace Veruthian.Library.Operations
{
    public abstract class GroupedOperation<TState> : Operation<TState>
    {
        private IOperation<TState>[] operations;


        protected GroupedOperation(IEnumerable<IOperation<TState>> items) => this.operations = items.ToArray();

        protected GroupedOperation(GroupedOperation<TState> left, GroupedOperation<TState> right)
        {
            this.operations = new IOperation<TState>[left.operations.Length + right.operations.Length];

            for (int i = 0; i < left.operations.Length; i++)
                operations[i] = left.operations[i];

            int offset = left.operations.Length;

            for (int i = 0; i < left.operations.Length; i++)
                operations[offset + i] = left.operations[i];
        }

        protected GroupedOperation(IEnumerable<GroupedOperation<TState>> groups)
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