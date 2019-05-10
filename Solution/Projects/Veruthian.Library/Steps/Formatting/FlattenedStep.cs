using System.Collections.Generic;
using System.Text;

namespace Veruthian.Library.Steps.Formatting
{
    public class FlattenedStep
    {
        private FlattenedStep(int index, IStep step, int substepCount)
        {
            this.Index = index;

            this.Step = step;

            this.SubStepIndices = new int[substepCount];
        }

        public int Index { get; }

        public IStep Step { get; }

        public int[] SubStepIndices { get; }


        public override string ToString() => FlattenedString();


        public const string BeforeIndex = "<", AfterIndex = ">";

        public const string BeforeSubIndices = "(", AfterSubIndices = ")";

        public const string SubIndexSeparator = " ";


        public string FlattenedString(string separator = SubIndexSeparator, string beforeIndex = BeforeIndex, string afterIndex = AfterIndex,
                                      string beforeSubIndices = BeforeSubIndices, string afterSubIndices = AfterSubIndices)
        {
            var result = new StringBuilder();

            result.Append(beforeIndex).Append(Index).Append(afterIndex).Append(" ");
            result.Append(Step.ToString());

            if (SubStepIndices.Length > 0)
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

            foreach (var index in SubStepIndices)
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


        public static FlattenedStep[] Flatten(IStep step)
        {
            var steps = new List<FlattenedStep>();

            var discovered = new Dictionary<IStep, int>();

            Flatten(step, steps, discovered);

            return steps.ToArray();
        }

        private static int Flatten(IStep step, List<FlattenedStep> steps, Dictionary<IStep, int> discovered)
        {
            if (discovered.ContainsKey(step))
            {
                return discovered[step];
            }
            else
            {
                int index = steps.Count;

                var flattened = new FlattenedStep(index, step, step.SubSteps.Count);

                steps.Add(flattened);

                discovered.Add(step, index);


                for (int i = 0; i < step.SubSteps.Count; i++)
                {
                    var substep = step.SubSteps[i];

                    flattened.SubStepIndices[i] = Flatten(substep, steps, discovered);
                }

                return index;
            }
        }
    }
}