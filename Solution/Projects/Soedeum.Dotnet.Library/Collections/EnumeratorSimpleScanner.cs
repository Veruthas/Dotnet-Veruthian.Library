using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public class EnumeratorSimpleScanner<T> : BaseScanner<T, EnumeratorSimpleScanner<T>>
    {
        IEnumerator<T> items;

        T current;

        bool atEnd;


        public EnumeratorSimpleScanner(IEnumerator<T> items) => this.items = items;


        public override bool IsEnd
        {
            get
            {
                Peek();
                return atEnd;
            }
        }

        public override void Dispose() => items.Dispose();

        protected override T Get(int lookahead = 0) => current;

        protected override void Initialize()
        {
            MoveToNext();
        }

        protected override void MoveToNext()
        {
            atEnd = !items.MoveNext();

            if (atEnd)
                current = default(T);
            else
                current = items.Current;
        }
    }
}