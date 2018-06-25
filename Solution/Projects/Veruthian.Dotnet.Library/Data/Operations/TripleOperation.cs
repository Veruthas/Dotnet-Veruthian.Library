using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public abstract class TripleOperation<TState> : Operation<TState>
    {
        readonly IOperation<TState> first, second, third;

        protected TripleOperation(IOperation<TState> first, IOperation<TState> second, IOperation<TState> third)
        {
            this.first = first;

            this.second = second;

            this.third = third;
        }

        public IOperation<TState> First => first;

        public IOperation<TState> Second => second;

        public IOperation<TState> Third => third;

        public override int Count => 3;

        public override IEnumerator<IOperation<TState>> GetEnumerator()
        {
            yield return first;

            yield return second;

            yield return third;
        }

        public override IOperation<TState> GetSubOperation(int index)
        {
            VerifyIndex(index);

            switch (index)
            {
                case 2:
                    return third;
                case 1:
                    return second;
                default:
                    return first;
            }
        }
    }
}