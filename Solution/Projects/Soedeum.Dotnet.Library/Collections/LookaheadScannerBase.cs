using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public abstract class LookaheadScannerBase<T, S> : ScannerBase<T, S>, ILookaheadScanner<T>
        where S : ScannerBase<T, S>
    {
        public LookaheadScannerBase(IEnumerator<T> enumerator, Func<T, T> generateEndItem) 
            : base(enumerator, generateEndItem) { }

        public T Peek(int lookahead) => base.CheckedPeek(lookahead);

        public bool PeekIsEnd(int lookahead) => base.CheckedIsAtEnd(lookahead);
    }
}