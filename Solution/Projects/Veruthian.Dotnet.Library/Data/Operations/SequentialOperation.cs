using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public enum SequenceType
    {
        AllOf,

        AnyOf
    }

    public abstract class SequentialOperation<TState> : Operation<TState>, IParentOperation<TState>
    {
        private bool returnWhenResult;

        public SequenceType SequenceType => returnWhenResult ? SequenceType.AnyOf : SequenceType.AllOf;


        protected SequentialOperation(SequenceType type)
        {
            returnWhenResult = type == SequenceType.AnyOf;
        }


        public override string Name => $"{this.SequenceType}Sequence";

        public abstract IOperation<TState> this[int index] { get; }

        public abstract int Count { get; }


        public abstract IEnumerator<IOperation<TState>> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        protected override bool DoAction(TState state, IOperationTracer<TState> tracer = null)
        {
            bool result;

            foreach (var operation in this)
            {
                if (operation.Perform(state, tracer) == returnWhenResult)
                {
                    result = returnWhenResult;

                    break;
                }
            }

            return !returnWhenResult;
        }
    }
}