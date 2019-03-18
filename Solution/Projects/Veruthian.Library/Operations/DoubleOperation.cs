namespace Veruthian.Library.Operations
{
    public abstract class DoubleOperation<TState> : Operation<TState>
    {
        readonly IOperation<TState> first, second;

        protected DoubleOperation(IOperation<TState> first, IOperation<TState> second)
        {
            this.first = first;

            this.second = second;
        }

        protected IOperation<TState> First => first;

        protected IOperation<TState> Second => second;

        protected override int Count => 2;


        protected override IOperation<TState> GetSubOperation(int verifiedIndex)
        {
            switch (verifiedIndex)
            {
                case 1:
                    return second;
                default:
                    return first;
            }
        }
    }
}