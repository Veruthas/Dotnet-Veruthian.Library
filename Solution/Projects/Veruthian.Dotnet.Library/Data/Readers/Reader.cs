using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public class Reader<T> : ReaderBase<T>
    {
        T item;

        public Reader(IEnumerator<T> enumerator, GenerateEndItem<T> generateEndItem)
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

        protected override void TryPreload(int amount) { }

        protected override void SkipAhead(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                MoveNext();

                if (IsEnd)
                    break;
            }
        }
    }
}