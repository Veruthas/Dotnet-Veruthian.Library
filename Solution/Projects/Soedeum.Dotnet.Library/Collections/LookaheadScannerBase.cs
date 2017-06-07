using System;

namespace Soedeum.Dotnet.Library.Collections
{
    public abstract class LookaheadScannerBase<T, S> : ScannerBase<T, S>, ILookaheadScanner<T>
        where S : ScannerBase<T, S>
    {
        T lastValid;

        protected T LastValid { get => lastValid; set => lastValid = value; }

        public T Peek(int lookahead) => base.CheckedPeek(lookahead);

        public bool PeekIsEnd(int lookahead) => base.CheckedIsAtEnd(lookahead);
    }
}