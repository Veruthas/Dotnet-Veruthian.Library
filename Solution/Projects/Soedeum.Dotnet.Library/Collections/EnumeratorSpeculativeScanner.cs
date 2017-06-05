using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public class EnumeratorSpeculativeScanner<T>
        : BaseScanner<T, EnumeratorSpeculativeScanner<T>>, ISpeculativeScanner<T>
    {
        IEnumerator<T> items;

        List<T> buffer = new List<T>();

        int index;


        public override bool IsEnd => throw new NotImplementedException();



        public override void Dispose() => items.Dispose();




        public T Peek(int lookahead)
        {
            throw new NotImplementedException();
        }

        public bool PeekIsEnd(int lookahead)
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



        public int MarkCount => throw new NotImplementedException();

        public void Mark()
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }
        public int GetLengthToMark(int index)
        {
            throw new NotImplementedException();
        }

        public int GetMarkPosition(int index)
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }
    }
}