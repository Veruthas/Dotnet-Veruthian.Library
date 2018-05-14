using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public class SimpleReader<T> : ReaderBase<T>
    {
        T item;

        public SimpleReader(IEnumerator<T> enumerator, GenerateEndItem<T> generateEndItem) 
            : base(enumerator, generateEndItem) { }
        
        protected override T RawPeek(int lookahead = 0) => item;

        protected override void Initialize() => MoveToNext();

        protected override void MoveToNext()
        {
            bool success = GetNext(out T next);

            if (!success)
                EndPosition = Position + 1;

            item = next;
        }

        protected override void VerifyLookahead(int lookahead = 0) { }
    }
}