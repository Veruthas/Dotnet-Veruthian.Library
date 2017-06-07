using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public class FixedLookaheadScanner<T> : FixedLookaheadScannerBase<T, FixedLookaheadScanner<T>>
    {
        IEnumerator<T> enumerator;

        Func<T, T> generateEndItem;

        public FixedLookaheadScanner(IEnumerator<T> enumerator, int lookahead, Func<T, T> generateEndItem = null)
            : base(lookahead)
        {
            this.enumerator = enumerator;

            this.generateEndItem = generateEndItem;
        }

        public override void Dispose() => enumerator.Dispose();
        protected override bool GetNext(out T next) => GetNextFromEnumerator(enumerator, generateEndItem, out next);
    }
}