using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public class VariableLookaheadScanner<T> : VariableLookaheadScannerBase<T, VariableLookaheadScanner<T>>
    {
        IEnumerator<T> enumerator;

        Func<T, T> generateEndItem;

        public VariableLookaheadScanner(IEnumerator<T> enumerator, Func<T, T> generateEndItem = null)
        {
            this.enumerator = enumerator;

            this.generateEndItem = generateEndItem;
        }

        protected override bool CanRollback => true;

        public override void Dispose() => enumerator.Dispose();

        protected override bool GetNext(out T next) => GetNextFromEnumerator(enumerator, generateEndItem, out next);
    }
}