using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Veruthian.Library.Operations
{
    public enum SequenceType
    {
        AllOf,

        AnyOf
    }

    public abstract class SequentialOperation<TState> : Operation<TState>
    {
        private bool returnWhenResult;

        public SequenceType SequenceType => returnWhenResult ? SequenceType.AnyOf : SequenceType.AllOf;


        protected SequentialOperation(SequenceType type)
        {
            returnWhenResult = type == SequenceType.AnyOf;
        }


        public override string Description => $"{this.SequenceType}Sequence";


        protected override bool DoAction(TState state, IOperationTracer<TState> tracer = null)
        {
            foreach (var operation in SubOperations)
            {
                var result = operation.Perform(state, tracer);

                OnOperationPeformed(operation, state, tracer, result);

                if (result == returnWhenResult)
                {
                    OnCompletion(returnWhenResult);

                    return returnWhenResult;
                }
            }

            OnCompletion(!returnWhenResult);

            return !returnWhenResult;
        }

        protected virtual void OnOperationPeformed(IOperation<TState> operation, TState state, IOperationTracer<TState> tracer, bool result) { }

        protected virtual void OnCompletion(bool success) { }
    }
}