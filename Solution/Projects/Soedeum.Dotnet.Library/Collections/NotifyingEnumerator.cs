using System;
using System.Collections;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public class NotifyingEnumerator<T> : IEnumerator<T>
    {
        IEnumerator<T> enumerator;


        public NotifyingEnumerator(IEnumerator<T> enumerator) => this.enumerator = enumerator;


        public T Current => enumerator.Current;

        object IEnumerator.Current => enumerator.Current;

        public void Dispose() => enumerator.Dispose();

        public void Reset() => enumerator.Reset();

        public bool MoveNext()
        {
            bool success = enumerator.MoveNext();

            OnMovedNext(success, success ? Current : default(T));

            return false;
        }


        public IEnumerator<T> BaseEnumerator => enumerator;


        protected virtual void OnMovedNext(bool success, T item)
        {
            if (MovedNext != null)
                MovedNext(this, success, item);
        }

        public event Action<NotifyingEnumerator<T>, bool, T> MovedNext;
    }
}