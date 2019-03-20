using Veruthian.Library.Collections;
using Veruthian.Library.Collections.Extensions;

namespace Veruthian.Library.Operations
{
    public class SequentialOperation<TState> : BaseGroupedOperation<TState>
    {
        public SequentialOperation(DataIndex<IOperation<TState>> operations) : base(operations)
        {
        }


        public override string Description => this.ToListString("(", ")", " ");

        protected override bool DoAction(TState state, ITracer<TState> tracer = null)
        {
            foreach(var operation in Operations)
            {
                if (!operation.Perform(state, tracer))
                    return false;
            }

            return true;
        }
    }
}