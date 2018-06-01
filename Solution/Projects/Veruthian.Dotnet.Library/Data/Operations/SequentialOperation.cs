using System;
using System.Collections.Generic;
using System.Text;

namespace Veruthian.Dotnet.Library.Data.Operations
{

    public class SequentialOperation<TState> : IOperation<TState>
    {
        List<IOperation<TState>> operations;

        bool untilResult;

        bool resultOnFound;


        public SequentialOperation(bool untilResult, bool resultOnFound, params IOperation<TState>[] operations)
            : this(untilResult, resultOnFound, new List<IOperation<TState>>(operations)) { }

        public SequentialOperation(bool untilResult, bool resultOnFound, IEnumerable<IOperation<TState>> operations)
            : this(untilResult, resultOnFound, new List<IOperation<TState>>(operations)) { }

        private SequentialOperation(bool untilResult, bool resultOnFound, List<IOperation<TState>> operations = null)
        {
            this.untilResult = untilResult;

            this.resultOnFound = resultOnFound;

            this.operations = operations != null ? operations : new List<IOperation<TState>>();
        }


        public SequentialOperation<TState> Add(IOperation<TState> operation)
        {
            if (operation == null)
                throw new ArgumentNullException("Operation cannot be null!");

            operations.Add(operation);

            return this;
        }

        public SequentialOperation<TState> AddSelf() => Add(this);


        public bool Perform(TState state)
        {
            foreach (var operation in operations)
            {
                if (operation.Perform(state) == untilResult)
                    return resultOnFound;
            }

            return !resultOnFound;
        }

        public bool Perform(TState state, IOperationTracer<TState> tracer)
        {
            tracer.StartingOperation(this, state);

            bool result = !untilResult;

            foreach (var operation in operations)
            {
                if (operation.Perform(state, tracer) == resultOnFound)
                {
                    result = resultOnFound;
                    break;
                }
            }

            tracer.FinishingOperation(this, state, result);

            return result;
        }

        // Problem, recursive sequences will print forever!
        public override string ToString()
        {
            string separator = untilResult ? " or " : " ";

            var builder = new StringBuilder();

            if (untilResult ^ resultOnFound)
                builder.Append("not ");

            builder.Append('(');

            bool started = false;

            foreach (var operation in operations)
            {
                if (!started)
                    started = true;
                else
                    builder.Append(separator);

                builder.Append(operation.ToString());
            }

            builder.Append(')');

            return builder.ToString();
        }

        public static SequentialOperation<TState> AnyOf(params IOperation<TState>[] operations) => new SequentialOperation<TState>(true, true, operations);

        public static SequentialOperation<TState> AnyOf(IEnumerable<IOperation<TState>> operations) => new SequentialOperation<TState>(true, true, operations);

        public static SequentialOperation<TState> AnyOf() => new SequentialOperation<TState>(true, true);


        public static SequentialOperation<TState> AllOf(params IOperation<TState>[] operations) => new SequentialOperation<TState>(false, false, operations);

        public static SequentialOperation<TState> AllOf(IEnumerable<IOperation<TState>> operations) => new SequentialOperation<TState>(false, false, operations);

        public static SequentialOperation<TState> AllOf() => new SequentialOperation<TState>(false, false);


        public static SequentialOperation<TState> NoneOf(params IOperation<TState>[] operations) => new SequentialOperation<TState>(true, false, operations);

        public static SequentialOperation<TState> NoneOf(IEnumerable<IOperation<TState>> operations) => new SequentialOperation<TState>(true, false, operations);

        public static SequentialOperation<TState> NoneOf() => new SequentialOperation<TState>(true, false);
    }
}