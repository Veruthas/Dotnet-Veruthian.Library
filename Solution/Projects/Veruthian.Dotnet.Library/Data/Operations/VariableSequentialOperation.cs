using System.Collections.Generic;
using System.Linq;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public class VariableSequentialOperation<TState> : SequentialOperation<TState>
    {
        List<IOperation<TState>> operations;

        public VariableSequentialOperation(SequenceType type)
            : this(type, new List<IOperation<TState>>()) { }

        public VariableSequentialOperation(SequenceType type, IEnumerable<IOperation<TState>> operations)
            : this(type, operations.ToList()) { }

        public VariableSequentialOperation(SequenceType type, params IOperation<TState>[] operations)
            : this(type, operations.ToList()) { }

        protected VariableSequentialOperation(SequenceType type, List<IOperation<TState>> operations)
            : base(type)
        {
            this.operations = operations;
        }


        public override int Count => operations.Count;

        public override IOperation<TState> GetSubOperation(int index)
        {
            VerifyIndex(index);

            return operations[index];
        }

        public override IEnumerator<IOperation<TState>> GetEnumerator() => operations.GetEnumerator();


        public VariableSequentialOperation<TState> Add(IOperation<TState> operation)
        {
            operations.Add(operation);

            return this;
        }

        public VariableSequentialOperation<TState> AddSelf() => Add(this);

        public VariableSequentialOperation<TState> AddRange(IEnumerable<IOperation<TState>> operations)
        {
            this.operations.AddRange(operations);

            return this;
        }

        public VariableSequentialOperation<TState> AddRange(params IOperation<TState>[] operations)
        {
            return AddRange((IEnumerable<IOperation<TState>>)operations);
        }


        public VariableSequentialOperation<TState> Insert(int index, IOperation<TState> operation)
        {
            operations.Insert(index, operation);

            return this;
        }

        public VariableSequentialOperation<TState> InsertSelf(int index) => Insert(index, this);

        public VariableSequentialOperation<TState> InsertRange(int index, IEnumerable<IOperation<TState>> operations)
        {
            this.operations.InsertRange(index, operations);

            return this;
        }

        public VariableSequentialOperation<TState> InsertRange(int index, params IOperation<TState>[] operations)
        {
            return InsertRange(index, (IEnumerable<IOperation<TState>>)operations);            
        }


        public void RemoveAt(int index) => operations.RemoveAt(index);

        public void Remove(IOperation<TState> operation) => operations.Remove(operation);


        public void Clear() => operations.Clear();
    }
}