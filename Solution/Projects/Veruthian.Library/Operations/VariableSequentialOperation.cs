using System.Collections.Generic;

namespace Veruthian.Library.Operations
{
    public class VariableSequentialOperation<TState> : VariableSequentialOperationBase<TState>
    {        
        public VariableSequentialOperation(SequenceType type)
            : base(type, new List<IOperation<TState>>()) { }

        public VariableSequentialOperation(SequenceType type, IEnumerable<IOperation<TState>> operations)
            : base(type, new List<IOperation<TState>>(operations)) { }

        public VariableSequentialOperation(SequenceType type, params IOperation<TState>[] operations)
            : base(type, new List<IOperation<TState>>(operations)) { }


        public VariableSequentialOperation<TState> Add(IOperation<TState> operation)
        {
            Operations.Add(operation);

            return this;
        }

        public VariableSequentialOperation<TState> AddSelf() => Add(this);

        public VariableSequentialOperation<TState> AddRange(IEnumerable<IOperation<TState>> operations)
        {
            Operations.AddRange(operations);

            return this;
        }

        public VariableSequentialOperation<TState> AddRange(params IOperation<TState>[] operations)
        {
            return AddRange((IEnumerable<IOperation<TState>>)operations);
        }


        public VariableSequentialOperation<TState> Insert(int index, IOperation<TState> operation)
        {
            Operations.Insert(index, operation);

            return this;
        }

        public VariableSequentialOperation<TState> InsertSelf(int index) => Insert(index, this);

        public VariableSequentialOperation<TState> InsertRange(int index, IEnumerable<IOperation<TState>> operations)
        {
            Operations.InsertRange(index, operations);

            return this;
        }

        public VariableSequentialOperation<TState> InsertRange(int index, params IOperation<TState>[] operations)
        {
            return InsertRange(index, (IEnumerable<IOperation<TState>>)operations);
        }


        public void RemoveAt(int index) => Operations.RemoveAt(index);

        public void Remove(IOperation<TState> operation) => Operations.Remove(operation);


        public void Clear() => Operations.Clear();
    }
}