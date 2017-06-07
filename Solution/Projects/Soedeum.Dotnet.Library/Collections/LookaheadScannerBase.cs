using System;

namespace Soedeum.Dotnet.Library.Collections
{
    public abstract class LookaheadScannerBase<T, S> : ScannerBase<T, S>, ILookaheadScanner<T>
        where S : ScannerBase<T, S>
    {
        public T Peek(int lookahead) => base.CheckedPeek(lookahead);

        public bool PeekIsEnd(int lookahead) => base.CheckedIsAtEnd(lookahead);
    }
}