using System;

namespace Soedeum.Dotnet.Library.Collections
{
    public class NotifyingFetcher<T> : IFetcher<T>
    {
        IFetcher<T> fetcher;

        public NotifyingFetcher(IFetcher<T> fetcher)
        {
            this.fetcher = fetcher;
        }


        public IFetcher<T> BaseFetcher => fetcher;


        public void Dispose() => fetcher.Dispose();

        public bool IsEnd(T item) => fetcher.IsEnd(item);

        public T FetchInitial()
        {
            var initial = fetcher.FetchInitial();

            OnItemFetch(initial);

            return initial;
        }

        public T FetchNext(T previous)
        {
            var next = fetcher.FetchNext(previous);

            OnItemFetch(next);

            return next;
        }

        public event Action<IFetcher<T>, T> ItemFetched;

        protected virtual void OnItemFetch(T item)
        {
            if (ItemFetched != null)
                ItemFetched(this, item);
        }

    }
}