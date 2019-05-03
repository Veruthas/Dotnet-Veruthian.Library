using Veruthian.Library.Collections;

namespace Veruthian.Library.Operations
{
    public class ClassedOperation<TState> : BaseNestedOperation<TState>
    {
        IExpandableContainer<string> classes;


        public ClassedOperation() { }

        public ClassedOperation(IOperation<TState> operation) => this.operation = operation;

        public ClassedOperation(IExpandableContainer<string> classes) => this.classes = classes;

        public ClassedOperation(IOperation<TState> operation, IExpandableContainer<string> classes)
        {
            this.operation = operation;

            this.classes = classes;
        }


        public new IOperation<TState> Operation
        {
            get => operation;
            set => operation = value;
        }

        public IExpandableContainer<string> Classes
        {
            get => classes;
            set => classes = value;
        }

        public override string Description => $"({operation?.ToString() ?? ""}):{(classes == null ? "{}" : classes.ToString())}";

        protected override bool DoAction(TState state, ITracer<TState> tracer = null) => Operation.Perform(state, tracer);
    }
}