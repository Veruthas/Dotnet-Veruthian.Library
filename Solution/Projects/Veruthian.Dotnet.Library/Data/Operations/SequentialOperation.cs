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