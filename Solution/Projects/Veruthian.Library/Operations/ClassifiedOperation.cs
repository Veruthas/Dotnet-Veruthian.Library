using Veruthian.Library.Collections;

namespace Veruthian.Library.Operations
{
    public class ClassifiedOperation<TState> : BaseNestedOperation<TState>
    {
        IExpandableContainer<string> classes;


        public ClassifiedOperation() { }

        public ClassifiedOperation(IOperation<TState> operation) => this.operation = operation;

        public ClassifiedOperation(IExpandableContainer<string> classes) => this.classes = classes;

        public ClassifiedOperation(IOperation<TState> operation, IExpandableContainer<string> classes)
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