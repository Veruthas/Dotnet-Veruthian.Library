using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public class EnumeratorLookaheadScanner<T> : BaseScanner<T, EnumeratorLookaheadScanner<T>>, ILookaheadScanner<T>
    {
        IEnumerator<T> items;

        public EnumeratorLookaheadScanner(IEnumerator<T> items, int lookahead)
        {

        }

        public override bool IsEnd => throw new NotImplementedException();

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public T Peek(int lookahead)
        {
            throw new NotImplementedException();
        }

        protected override T Get(int lookahead = 0)
        {
            throw new NotImplementedException();
        }

        protected override void Initialize()
        {
            throw new NotImplementedException();
        }

        protected override void MoveToNext()
        {
            throw new NotImplementedException();
        }
    }
}