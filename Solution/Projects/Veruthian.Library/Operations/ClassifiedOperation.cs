using Veruthian.Library.Collections;
using Veruthian.Library.Collections.Extensions;

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


        public bool Contains (string className) => classes != null && classes.Contains(className);
        

        public override string Description => classes == null ? "class<>" : "class" + classes.ToListString("<", ">", ", ");

        protected override bool DoAction(TState state, ITracer<TState> tracer = null) => Operation == null ? true : Operation.Perform(state, tracer);
    }
}