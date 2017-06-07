using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public class SimpleScanner<T> : SimpleScannerBase<T, SimpleScanner<T>>
    {
        IEnumerator<T> enumerator;

        Func<T, T> generateEndItem;

        public SimpleScanner(IEnumerator<T> enumerator, Func<T, T> generateEndItem = null)
        {
            this.enumerator = enumerator;

            this.generateEndItem = generateEndItem;
        }

        public override void Dispose() => enumerator.Dispose();

        protected override bool GetNext(out T next) => GetNextFromEnumerator(enumerator, generateEndItem, out next);
    }
}