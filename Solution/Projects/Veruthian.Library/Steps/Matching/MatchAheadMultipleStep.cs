using Veruthian.Library.Utility;

namespace Veruthian.Library.Steps.Matching
{
    public abstract class MatchAheadMultipleStep<T> : MatchMultipleStep<T>
    {
        int lookahead;

        public MatchAheadMultipleStep(int lookahead)
        {
            ExceptionHelper.VerifyNotNull(lookahead);

            this.lookahead = lookahead;
        }

        public int LookAhead => lookahead;
    }
}