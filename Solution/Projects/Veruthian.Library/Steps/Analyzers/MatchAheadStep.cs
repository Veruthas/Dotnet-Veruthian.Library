using Veruthian.Library.Utility;

namespace Veruthian.Library.Steps.Analyzers
{
    public abstract class MatchAheadStep<T> : MatchStep<T>
    {
        int lookahead;


        public MatchAheadStep(int lookahead) 
        {
            ExceptionHelper.VerifyNotNull(lookahead);

            this.lookahead = lookahead;
        }

        public int LookAhead => lookahead;
    }
}