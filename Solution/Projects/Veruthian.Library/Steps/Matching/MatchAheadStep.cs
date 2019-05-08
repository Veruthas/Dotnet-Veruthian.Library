using Veruthian.Library.Utility;

namespace Veruthian.Library.Steps.Matching
{
    public abstract class MatchAheadStep<T> : MatchStep<T>
    {
        int lookahead;

        MatchStep<T> matcher;


        public MatchAheadStep(int lookahead, MatchStep<T> matcher) 
        {
            ExceptionHelper.VerifyNotNull(lookahead);

            ExceptionHelper.VerifyNotNull(matcher);

            this.lookahead = lookahead;

            this.matcher = matcher;
        }


        public int LookAhead => lookahead;

        public MatchStep<T> Matcher => matcher;
    }
}