using System;
using System.Collections.Generic;
using System.Text;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public class FlattenedOperation<TState>
    {
        public FlattenedOperation(IOperation<TState> operation, int suboperationCount)
        {
            this.Operation = operation;

            this.SubOperationIndices = new int[suboperationCount];
        }

        public IOperation<TState> Operation { get; }

        public int[] SubOperationIndices { get; }

        public override string ToString() => FlattenedString();

        public string FlattenedString(string separator = " ", string before = "<", string after = ">")
        {
            var result = new StringBuilder();
            result.Append(Operation.ToString());
            result.Append('(');

            IndicesToString(result, separator, before, after); ;

            result.Append(')');

            return result.ToString();
        }

        public string IndicesToString(string separator = " ", string before = "<", string after = ">")
        {
            StringBuilder builder = new StringBuilder();

            IndicesToString(builder, separator, before, after);

            return builder.ToString();
        }

        public void IndicesToString(StringBuilder builder, string separator = " ", string before = "<", string after = ">")
        {
            bool started = false;

            foreach (var index in SubOperationIndices)
            {
                if (!started)
                    started = true;
                else
                    builder.Append(separator);

                builder.Append(before);
                builder.Append(index.ToString());
                builder.Append(after);
            }
        }
    }

    public static class OperationUtility
    {
        public static FlattenedOperation<TState>[] Flatten<TState>(this IOperation<TState> operation)
        {
            var operations = new List<FlattenedOperation<TState>>();

            var discovered = new Dictionary<IOperation<TState>, int>();

            Flatten(operation, operations, discovered);

            return operations.ToArray();
        }

        private static int Flatten<TState>(IOperation<TState> operation, List<FlattenedOperation<TState>> operations, Dictionary<IOperation<TState>, int> discovered)
        {
            if (discovered.ContainsKey(operation))
            {
                return discovered[operation];
            }
            else
            {
                int index = operations.Count;

                var flattened = new FlattenedOperation<TState>(operation, operation.Count);

                operations.Add(flattened);

                discovered.Add(operation, index);


                for (int i = 0; i < operation.Count; i++)
                {
                    var suboperation = operation.GetSubOperation(i);

                    flattened.SubOperationIndices[i] = Flatten(suboperation, operations, discovered);
                }

                return index;
            }
        }

        public static RepeatedOperation<TState> Repeat<TState>(this IOperation<TState> operation, int minimum = 0, int maximum = 0)
        {
            return new RepeatedOperation<TState>(operation, minimum, maximum);
        }

        public static OptionalOperation<TState> MakeOptional<TState>(this IOperation<TState> operation)
        {
            return new OptionalOperation<TState>(operation);
        }

        public static AnyOfSequenceOperation<TState> Or<TState>(this IOperation<TState> first, IOperation<TState> follow)
        {
            return new AnyOfSequenceOperation<TState>(first, follow);
        }

        public static AllOfSequenceOperation<TState> And<TState>(this IOperation<TState> first, IOperation<TState> follow)
        {
            return new AllOfSequenceOperation<TState>(first, follow);
        }
    }
}