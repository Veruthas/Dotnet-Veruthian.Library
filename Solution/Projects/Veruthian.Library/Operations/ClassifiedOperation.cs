using Veruthian.Library.Collections;
using Veruthian.Library.Collections.Extensions;
using Veruthian.Library.Utility;

namespace Veruthian.Library.Operations
{
    public class ClassifiedOperation<TState> : BaseNestedOperation<TState>
    {
        IExpandableContainer<string> classes;


        public ClassifiedOperation() : this(null, null) { }

        public ClassifiedOperation(IOperation<TState> operation) : this(operation, null) { }

        public ClassifiedOperation(IExpandableContainer<string> classes) : this(null, classes) { }

        public ClassifiedOperation(IOperation<TState> operation, IExpandableContainer<string> classes)
        {
            Operation = operation;

            Classes = classes;
        }


        public new IOperation<TState> Operation
        {
            get => operation;
            set => operation = value ?? BooleanOperation<TState>.True;
        }

        public IExpandableContainer<string> Classes
        {
            get => classes;
            set => classes = value;
        }


        public bool Is(string className) => classes != null && classes.Contains(className);


        public override string Description => classes == null ? "class<>" : "class" + classes.ToListString("<", ">", ", ");

        protected override bool DoAction(TState state, ITracer<TState> tracer = null) => Operation == null ? true : Operation.Perform(state, tracer);
    }
}