namespace Veruthian.Dotnet.Library.Operations
{
    public class BooleanOperation<TState> : SimpleOperation<TState>
    {
        bool value;

        private BooleanOperation(bool value) => this.value = value;


        public override string Description => value.ToString();

        protected override bool DoAction(TState state, IOperationTracer<TState> tracer = null) => value;
        

        public override string ToString() => value.ToString();


        public static readonly BooleanOperation<TState> True = new BooleanOperation<TState>(true);
        public static readonly BooleanOperation<TState> False = new BooleanOperation<TState>(false);
    }
}