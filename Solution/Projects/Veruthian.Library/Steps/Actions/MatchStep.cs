using Veruthian.Library.Collections;
using Veruthian.Library.Readers;

namespace Veruthian.Library.Steps.Actions
{
    public abstract class MatchStep<T> : BaseStep, IActionStep<IReader<T>>, IActionStep<ObjectTable>
    {
        protected MatchStep() { }

        protected MatchStep(string name) => this.Name = name;


        public override string Type => $"Match";

        public override string Name { get; }


        public abstract (bool Result, object State) Match(T item, object state = null);

        public bool? Act(bool? state, bool completed, IReader<T> reader)
        {
            if (!completed && state == true)
            {
                object matchState;

                do
                {
                    var result = Match(reader.Current, state);

                    if (result.Result)
                        reader.Advance();
                    else
                        return false;

                    matchState = result.State;

                } while (matchState != null);

                return true;
            }

            return state;
        }

        protected abstract string MatchingTo { get; }

        public override string ToString() => StepString($"{Type}<{MatchingTo ?? "NULL"}>", Name);


        public const string ReaderAddress = "MatchStep.Reader";

        public bool? Act(bool? state, bool completed, ObjectTable table)
            => Act(state, completed, table.Get<IReader<T>>(ReaderAddress));
    }
}