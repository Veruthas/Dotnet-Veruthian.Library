using Veruthian.Library.Collections;

namespace Veruthian.Library.Operations
{
    public class ClassedOperation<TState> : BaseOperation<TState>
    {
        IOperation<TState> operation;

        DataSet<string> classes;


        public ClassedOperation() { }

        public ClassedOperation(IOperation<TState> operation) => this.operation = operation;

        public ClassedOperation(DataSet<string> classes) => this.classes = classes;

        public ClassedOperation(IOperation<TState> operation, DataSet<string> classes)
        {
            this.operation = operation;

            this.classes = classes;
        }


        public IOperation<TState> Operation
        {
            get => operation;
            set => operation = value;
        }

        public DataSet<string> Classes
        {
            get => classes;
            set => classes = value;
        }

        public override string Description => $"({operation?.ToString() ?? ""}):{(classes == null ? "{}" : classes.ToString(false))}";

        protected override bool DoAction(TState state, ITracer<TState> tracer = null) => Operation.Perform(state, tracer);

        protected override int Count => 1;

        protected override IOperation<TState> GetSubOperation(int verifiedIndex) => operation;
    }
}