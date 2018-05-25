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

        protected override void Initialize() => SkipAhead(1);

        protected override bool MoveToNext()
        {
            bool success = GetNext(out T next);

            if (!success)
                EndPosition = Position + 1;

            item = next;

            return success;
        }

        protected override int SkipAhead(int amount)
        {
            if (amount <= 0) 
                return 0;

            for (int i = 0; i < amount; i++)
            {
                if (!MoveToNext())
                    return i;
            }

            return amount;
        }

        protected override void VerifyLookahead(int lookahead = 0) { }
    }
}