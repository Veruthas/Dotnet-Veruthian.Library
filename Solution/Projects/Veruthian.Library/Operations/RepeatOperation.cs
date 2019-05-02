namespace Veruthian.Library.Operations
{
    public class RepeatOperation<TState> : BaseNestedOperation<TState>
    {
        int? times;


        public RepeatOperation(IOperation<TState> operation, int? times = null) : base(operation)
        {
            this.times = times;
        }

        public override string Description => $"repeat {(times == null ? "?" : times.ToString())} {Operation}";

        protected override bool DoAction(TState state, ITracer<TState> tracer = null)
        {
            if (times == null)
            {
                while (Operation.Perform(state, tracer)) ;

                return true;
            }
            else
            {
                for (int i = 0; i < times; i++)
                    if (!Operation.Perform(state, tracer))
                        return false;

                return true;
            }
        }
    }
}