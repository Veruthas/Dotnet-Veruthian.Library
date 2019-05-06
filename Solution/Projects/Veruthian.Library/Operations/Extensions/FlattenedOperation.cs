using System.Collections.Generic;
using System.Text;

namespace Veruthian.Library.Operations.Extensions
{
    // Produces a flattened list of Operations instead of Tree
    public class FlattenedOperation<TState>
    {
        private FlattenedOperation(int index, IOperation<TState> operation, int suboperationCount)
        {
            this.Index = index;

            this.Operation = operation;

            this.SubOperationIndices = new int[suboperationCount];
        }

        public int Index { get; }

        public IOperation<TState> Operation { get; }

        public int[] SubOperationIndices { get; }


        public override string ToString() => FlattenedString();


        public const string BeforeIndex = "<", AfterIndex = ">";
        
        public const string BeforeSubIndices = "(", AfterSubIndices = ")";

        public const string SubIndexSeparator = " ";


        public string FlattenedString(string separator = SubIndexSeparator, string beforeIndex = BeforeIndex, string afterIndex = AfterIndex,
                                      string beforeSubIndices = BeforeSubIndices, string afterSubIndices = AfterSubIndices)
        {
            var result = new StringBuilder();

            result.Append(beforeIndex).Append(Index).Append(afterIndex).Append(" ");
            result.Append(Operation.ToString());

            if (SubOperationIndices.Length > 0)
            {
                result.Append(beforeSubIndices);
                IndicesToString(result, separator, beforeIndex, afterIndex); ;
                result.Append(afterSubIndices);
            }

            return result.ToString();
        }

        public string IndicesToString(string separator = SubIndexSeparator, string before = BeforeIndex, string after = AfterIndex)
        {
            StringBuilder builder = new StringBuilder();

            IndicesToString(builder, separator, before, after);

            return builder.ToString();
        }

        public void IndicesToString(StringBuilder builder, string separator = SubIndexSeparator, string before = BeforeIndex, string after = AfterIndex)
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


        public static FlattenedOperation<TState>[] Flatten(IOperation<TState> operation)
        {
            var operations = new List<FlattenedOperation<TState>>();

            var discovered = new Dictionary<IOperation<TState>, int>();

            Flatten(operation, operations, discovered);

            return operations.ToArray();
        }

        private static int Flatten(IOperation<TState> operation, List<FlattenedOperation<TState>> operations, Dictionary<IOperation<TState>, int> discovered)
        {
            if (discovered.ContainsKey(operation))
            {
                return discovered[operation];
            }
            else
            {
                int index = operations.Count;

                var flattened = new FlattenedOperation<TState>(index, operation, operation.SubOperations.Count);

                operations.Add(flattened);

                discovered.Add(operation, index);


                for (int i = 0; i < operation.SubOperations.Count; i++)
                {
                    var suboperation = operation.SubOperations[i];

                    flattened.SubOperationIndices[i] = Flatten(suboperation, operations, discovered);
                }

                return index;
            }
        }
    }
}