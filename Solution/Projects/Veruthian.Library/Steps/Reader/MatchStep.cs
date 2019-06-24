using Veruthian.Library.Collections;
using Veruthian.Library.Readers;

namespace Veruthian.Library.Steps.Reader
{
    public abstract class MatchStep<T> : ReaderStep<T>
    {
        protected MatchStep() { }


        public abstract (bool Result, object State) Match(T value, object state = null);

        protected abstract string MatchingTo { get; }

        public override string Description => "Match";


        public override string ToString() => $"{Description}<{MatchingTo ?? "NULL"}>";

        protected override bool Act(IReader<T> reader)
        {
            object matchState = null;

            do
            {
                if (reader.IsEnd)
                    return false;

                var result = Match(reader.Current, matchState);

                if (result.Result)
                    reader.Advance();
                else
                    return false;

                matchState = result.State;

            } while (matchState != null);

            return true;
        }


    }
}