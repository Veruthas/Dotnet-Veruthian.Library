using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    #region SequentialOperation

    public abstract class SequentialOperation<TState, TOperation> : IOperation<TState>, IEnumerable<TOperation>
        where TOperation : IOperation<TState>
    {
        protected readonly List<TOperation> operations;

        protected SequentialOperation(List<TOperation> operations = null)
        {
            this.operations = operations ?? new List<TOperation>();
        }


        protected SequentialOperation(IEnumerable<TOperation> operations)
        {
            this.operations = new List<TOperation>(operations);
        }


        public abstract bool UntilSubResult { get; }

        public abstract bool ResultOnSuccess { get; }



        public IEnumerator<TOperation> GetEnumerator() => operations.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        public bool Perform(TState state)
        {
            foreach (var operation in operations)
            {
                if (operation.Perform(state) == UntilSubResult)
                    return ResultOnSuccess;
            }

            return !ResultOnSuccess;
        }

        public bool Perform(TState state, IOperationTracer<TState> tracer)
        {
            tracer.StartingOperation(this, state);

            bool result = !ResultOnSuccess;

            foreach (var operation in operations)
            {
                if (operation.Perform(state, tracer) == UntilSubResult)
                {
                    result = ResultOnSuccess;

                    break;
                }
            }

            tracer.FinishingOperation(this, state, result);

            return result;
        }


        public override string ToString()
        {
            return $"Sequence({operations.Count})";
        }
    }

    #endregion

    #region DynamicSequentialOperation

    public abstract class DynamicSequentialOperation<TState, TOperation> : SequentialOperation<TState, TOperation>
        where TOperation : IOperation<TState>
    {
        protected DynamicSequentialOperation(IEnumerable<TOperation> operations)
            : base(operations) { }


        public List<TOperation> SubOperations => operations;
        

        public DynamicSequentialOperation<TState, TOperation> Add(TOperation operation)
        {
            SubOperations<
        }
    }

    #endregion

    #region AnyOfSequenceOperation

    public class AnyOfSequenceOperation<TState, TOperation> : DynamicSequentialOperation<TState, TOperation>
        where TOperation : IOperation<TState>
    {
        public AnyOfSequenceOperation(params TOperation[] operations)
            : base(operations) { }


        public AnyOfSequenceOperation(IEnumerable<TOperation> operations)
            : base(operations) { }

        public override bool UntilSubResult => true;

        public override bool ResultOnSuccess => true;

        public override string ToString() => "AnyOf" + base.ToString();
    }

    public class AnyOfSequenceOperation<TState> : AnyOfSequenceOperation<TState, IOperation<TState>>
    {
        public AnyOfSequenceOperation(params IOperation<TState>[] operations)
            : base(operations) { }


        public AnyOfSequenceOperation(IEnumerable<IOperation<TState>> operations)
            : base(operations) { }
    }

    #endregion

    #region AllOfSequenceOperation

    public class AllOfSequenceOperation<TState, TOperation> : DynamicSequentialOperation<TState, TOperation>
        where TOperation : IOperation<TState>
    {
        public AllOfSequenceOperation(params TOperation[] operations)
            : base(operations) { }


        public AllOfSequenceOperation(IEnumerable<TOperation> operations)
            : base(operations) { }

        public override bool UntilSubResult => true;

        public override bool ResultOnSuccess => true;

        public override string ToString() => "AllOf" + base.ToString();
    }

    public class AllOfSequenceOperation<TState> : AnyOfSequenceOperation<TState, IOperation<TState>>
    {
        public AllOfSequenceOperation(params IOperation<TState>[] operations)
            : base(operations) { }


        public AllOfSequenceOperation(IEnumerable<IOperation<TState>> operations)
            : base(operations) { }
    }

    #endregion

    #region NoneOfSequenceOperation

    public class NoneOfSequenceOperation<TState, TOperation> : DynamicSequentialOperation<TState, TOperation>
        where TOperation : IOperation<TState>
    {
        public NoneOfSequenceOperation(params TOperation[] operations)
            : base(operations) { }


        public NoneOfSequenceOperation(IEnumerable<TOperation> operations)
            : base(operations) { }

        public override bool UntilSubResult => true;

        public override bool ResultOnSuccess => true;

        public override string ToString() => "NoneOf" + base.ToString();
    }

    public class NoneOfSequenceOperation<TState> : AnyOfSequenceOperation<TState, IOperation<TState>>
    {
        public NoneOfSequenceOperation(params IOperation<TState>[] operations)
            : base(operations) { }


        public NoneOfSequenceOperation(IEnumerable<IOperation<TState>> operations)
            : base(operations) { }
    }

    #endregion

    #region IsNotOfSequenceOperation

    public class IsNotOfSequenceOperation<TState, TOperation> : DynamicSequentialOperation<TState, TOperation>
        where TOperation : IOperation<TState>
    {
        public IsNotOfSequenceOperation(params TOperation[] operations)
            : base(operations) { }


        public IsNotOfSequenceOperation(IEnumerable<TOperation> operations)
            : base(operations) { }

        public override bool UntilSubResult => true;

        public override bool ResultOnSuccess => true;

        public override string ToString() => "IsNotOf" + base.ToString();
    }

    public class IsNotOfSequenceOperation<TState> : AnyOfSequenceOperation<TState, IOperation<TState>>
    {
        public IsNotOfSequenceOperation(params IOperation<TState>[] operations)
            : base(operations) { }


        public IsNotOfSequenceOperation(IEnumerable<IOperation<TState>> operations)
            : base(operations) { }
    }

    #endregion
}