using System;

namespace Soedeum.Dotnet.Library.Collections
{
    public class FetcherLookaheadScanner<T> : BaseLookaheadScanner<T>
    {
        IFetcher<T> fetcher;

        public FetcherLookaheadScanner(int lookahead, IFetcher<T> fetcher)
            : base(lookahead)
        {
            this.fetcher = fetcher;
        }


        public override void Dispose() => fetcher.Dispose();

        public override bool IsEnd => fetcher.IsEnd(Peek());

        protected override T FetchInitial()
        {
            return fetcher.FetchInitial();
        }

        protected override T FetchNext(T previous)
        {
            return fetcher.FetchNext(previous);
        }
    }
}