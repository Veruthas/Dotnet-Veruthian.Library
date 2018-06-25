using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public abstract class DoubleOperation<TState> : Operation<TState>
    {
        readonly IOperation<TState> first, second;

        protected DoubleOperation(IOperation<TState> first, IOperation<TState> second)
        {
            this.first = first;

            this.second = second;
        }

        public IOperation<TState> First => first;

        public IOperation<TState> Second => second;

        public override int Count => 2;

        public override IEnumerator<IOperation<TState>> GetEnumerator()
        {
            yield return first;

            yield return second;
        }

        public override IOperation<TState> GetSubOperation(int index)
        {
            VerifyIndex(index);

            switch (index)
            {
                case 1:
                    return second;
                default:
                    return first;
            }
        }
    }
}