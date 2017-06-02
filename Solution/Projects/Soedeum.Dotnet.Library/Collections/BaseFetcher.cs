using System;

namespace Soedeum.Dotnet.Library.Collections
{
    public abstract class BaseFetcher<T> : IFetcher<T>
    {
        public abstract void Dispose();

        public abstract bool IsEnd(T item);

        public abstract T FetchInitial();

        public abstract T FetchNext(T previous);
    }
}