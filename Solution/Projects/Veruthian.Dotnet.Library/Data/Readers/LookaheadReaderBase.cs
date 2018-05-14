using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public abstract class LookaheadReaderBase<T> : ReaderBase<T>, ILookaheadReader<T>        
    {
        public LookaheadReaderBase(IEnumerator<T> enumerator, GenerateEndItem<T> generateEndItem) 
            : base(enumerator, generateEndItem) { }

        public T Peek(int lookahead) => base.CheckedPeek(lookahead);

        public bool PeekIsEnd(int lookahead) => base.CheckedIsAtEnd(lookahead);
    }
}