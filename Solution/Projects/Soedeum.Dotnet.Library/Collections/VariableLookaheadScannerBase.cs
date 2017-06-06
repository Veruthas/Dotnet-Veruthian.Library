using System;

namespace Soedeum.Dotnet.Library.Collections
{
    public abstract class VariableLookaheadScannerBase<T, S> : LookaheadScannerBase<T, S>, ILookaheadScanner<T>
        where S : ScannerBase<T, S>
    { 
    }
}