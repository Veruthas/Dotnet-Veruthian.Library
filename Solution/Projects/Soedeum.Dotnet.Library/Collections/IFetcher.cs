using System;

namespace Soedeum.Dotnet.Library.Collections
{
    public interface IFetcher<T> : IDisposable
    {
         T FetchInitial();

        T FetchNext(T previous);

        bool IsEnd(T item);
    }
}