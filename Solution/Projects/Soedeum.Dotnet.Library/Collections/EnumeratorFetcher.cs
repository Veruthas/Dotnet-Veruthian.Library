using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public class EnumeratorFetcher<T> : BaseFetcher<T>
    {
        IEnumerator<T> enumerator;

        bool isEnd = false;

        bool initialized = false;


        public EnumeratorFetcher(IEnumerator<T> enumerator)
        {
            this.enumerator = enumerator;
        }

        public EnumeratorFetcher(IEnumerable<T> enumerable)
            : this(enumerable.GetEnumerator()) { }


        public override void Dispose() => enumerator.Dispose();

        public override bool IsEnd(T item) => !initialized || isEnd;


        public override T FetchInitial() => Fetch();


        public override T FetchNext(T previous) => Fetch();

        private T Fetch()
        {
            initialized = true;

            isEnd = !enumerator.MoveNext();

            return isEnd ? default(T) : enumerator.Current;
        }
    }
}