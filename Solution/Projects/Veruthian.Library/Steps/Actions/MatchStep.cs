using Veruthian.Library.Collections;
using Veruthian.Library.Readers;

namespace Veruthian.Library.Steps.Actions
{
    public abstract class MatchStep<T> : BaseStep, IActionStep<IReader<T>>, IActionStep<ObjectTable>
    {
        protected MatchStep() { }

        public const string TypeName = "Match";
        
        public override string Type => TypeName;


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

        public override string ToString() => $"{Type}<{MatchingTo ?? "NULL"}>";


        public const string ReaderAddress = "MatchStep.Reader";

        public bool? Act(bool? state, bool completed, ObjectTable table)
            => Act(state, completed, table.Get<IReader<T>>(ReaderAddress));
    }
}