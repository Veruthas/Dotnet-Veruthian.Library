using System;

namespace Soedeum.Dotnet.Library.Collections
{
    public abstract class BaseFetcher<T> : IFetcher<T>
    {
        public abstract void Dispose();

        public abstract bool IsEnd(T item);

        public abstract T FetchInitial();

        public abstract T FetchNext(T previous);


        public NotifyingFetcher<T> GetNotifyingFetcher()
        {
            return new NotifyingFetcher<T>(this);
        }

        public NotifyingFetcher<T> GetNotifyingFetcher(params Action<IFetcher<T>, T>[] onFetchedHandlers)
        {
            var fetcher = GetNotifyingFetcher();

            foreach (var handler in onFetchedHandlers)
                fetcher.ItemFetched += handler;

            return fetcher;
        }

        public IScanner<T> GetSimpleScanner()
        {
            return new FetcherSimpleScanner<T>(this);
        }

        public ILookaheadScanner<T> GetLookaheadScanner(int k)
        {
            throw new NotImplementedException();
        }

        public ISpeculativeScanner<T> GetSpeculativeScanner()
        {
            throw new NotImplementedException();
        }
    }
}