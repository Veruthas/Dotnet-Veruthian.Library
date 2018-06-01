using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    #region SequentialOperation

    public abstract class SequentialOperation<TState> : Operation<TState>, ISequentialOperation<TState>
    {
        protected readonly List<IOperation<TState>> operations;

        protected SequentialOperation(List<IOperation<TState>> operations = null)
        {
            this.operations = operations ?? new List<IOperation<TState>>();
        }


        protected SequentialOperation(IEnumerable<IOperation<TState>> operations)
        {
            this.operations = new List<IOperation<TState>>(operations);
        }

        public IOperation<TState> this[int index] => operations[index];

        public int Count => operations.Count;


        public IEnumerator<IOperation<TState>> GetEnumerator() => operations.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        public abstract bool UntilSubResult { get; }

        public abstract bool ResultOnSuccess { get; }

        protected override bool DoAction(TState state, IOperationTracer<TState> tracer = null)
        {
            bool result = !ResultOnSuccess;

            foreach (var operation in operations)
            {
                if (operation.Perform(state, tracer) == UntilSubResult)
                {
                    result = ResultOnSuccess;

                    break;
                }
            }

            return result;
        }


        public override string ToString()
        {
            return $"Sequence({operations.Count})";
        }


    }

    #endregion

    #region DynamicSequentialOperation

    public abstract class DynamicSequentialOperation<TState> : SequentialOperation<TState>        
    {
        protected DynamicSequentialOperation(IEnumerable<IOperation<TState>> operations)
            : base(operations) { }


        public DynamicSequentialOperation<TState> Add(IOperation<TState> operation)
        {
            operations.Add(operation);

            return this;
        }

        public DynamicSequentialOperation<TState> AddSelf()
        {
            operations.Add(this);

            return this;
        }
    }

    #endregion

    #region AnyOfSequenceOperation

    public class AnyOfSequenceOperation<TState> : DynamicSequentialOperation<TState>
    {
        public AnyOfSequenceOperation(params IOperation<TState>[] operations)
            : base(operations) { }


        public AnyOfSequenceOperation(IEnumerable<IOperation<TState>> operations)
            : base(operations) { }

        public override bool UntilSubResult => true;

        public override bool ResultOnSuccess => true;

        public override string ToString() => "AnyOf" + base.ToString();
    }


    #endregion

    #region AllOfSequenceOperation

    public class AllOfSequenceOperation<TState> : DynamicSequentialOperation<TState>
    {
        public AllOfSequenceOperation(params IOperation<TState>[] operations)
            : base(operations) { }


        public AllOfSequenceOperation(IEnumerable<IOperation<TState>> operations)
            : base(operations) { }

        public override bool UntilSubResult => true;

        public override bool ResultOnSuccess => true;

        public override string ToString() => "AllOf" + base.ToString();
    }

    #endregion

    #region NoneOfSequenceOperation

    public class NoneOfSequenceOperation<TState, TOperation> : DynamicSequentialOperation<TState>
    {
        public NoneOfSequenceOperation(params IOperation<TState>[] operations)
            : base(operations) { }


        public NoneOfSequenceOperation(IEnumerable<IOperation<TState>> operations)
            : base(operations) { }

        public override bool UntilSubResult => true;

        public override bool ResultOnSuccess => true;

        public override string ToString() => "NoneOf" + base.ToString();
    }

    #endregion

    #region IsNotOfSequenceOperation

    public class IsNotOfSequenceOperation<TState> : DynamicSequentialOperation<TState>
    {
        public IsNotOfSequenceOperation(params IOperation<TState>[] operations)
            : base(operations) { }


        public IsNotOfSequenceOperation(IEnumerable<IOperation<TState>> operations)
            : base(operations) { }

        public override bool UntilSubResult => true;

        public override bool ResultOnSuccess => true;

        public override string ToString() => "IsNotOf" + base.ToString();
    }

    #endregion
}