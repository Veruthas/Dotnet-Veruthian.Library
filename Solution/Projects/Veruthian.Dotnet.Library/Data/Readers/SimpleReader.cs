using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public class SimpleReader<T> : ReaderBase<T>
    {
        T item;

        public SimpleReader(IEnumerator<T> enumerator, GenerateEndItem<T> generateEndItem)
        {
            SetData(enumerator, generateEndItem);
        }


        protected override void Initialize() => MoveNext();

        protected override T RawPeek(int lookahead = 0) => item;

        protected override void EnsureLookahead(int lookahead = 0) { }

        protected override void MoveNext()
        {
            bool success = GetNext(out T next);

            if (!IsEnd)            
                Position++;

            if (!success && !EndFound)
                EndPosition = Position;
                
            item = next;
        }

        protected override int SkipAhead(int amount)
        {
            if (amount <= 0)
                return 0;

            for (int i = 0; i < amount; i++)
            {
                MoveNext();

                if (IsEnd)
                    return i;
            }

            return amount;
        }
    }
}