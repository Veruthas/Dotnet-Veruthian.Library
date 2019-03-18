namespace Veruthian.Library.Operations
{
    public class BooleanOperation<TState> : BaseSimpleOperation<TState>
    {
        readonly bool value;

        private BooleanOperation(bool value) => this.value = value;

        public override string Description => value.ToString();

        protected override bool DoAction(TState state, ITracer<TState> tracer = null) => value;


        public static readonly BooleanOperation<TState> True = new BooleanOperation<TState>(true);
        
        public static readonly BooleanOperation<TState> False = new BooleanOperation<TState>(false);
    }
}