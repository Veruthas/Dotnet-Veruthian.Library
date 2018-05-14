using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public abstract class BaseLookaheadReader<T> : BaseReader<T>, ILookaheadReader<T>        
    {
        public BaseLookaheadReader(IEnumerator<T> enumerator, GenerateEndItem<T> generateEndItem) 
            : base(enumerator, generateEndItem) { }

        public T Peek(int lookahead) => base.CheckedPeek(lookahead);

        public bool PeekIsEnd(int lookahead) => base.CheckedIsAtEnd(lookahead);
    }
}