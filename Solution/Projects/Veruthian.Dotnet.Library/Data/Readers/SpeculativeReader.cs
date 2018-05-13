using System;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public class SpeculativeReader<T> : SpeculativeReaderBase<T, object>, ISpeculativeReader<T>
    {
        public SpeculativeReader(IEnumerator<T> enumerator, GenerateEndItem<T> generateEndItem = null)
            : base(enumerator, generateEndItem)
        {
        }

        public void Mark() => CreateMark(Position, Index, null);
    }

    public class SpeculativeReader<T, TState> : SpeculativeReaderBase<T, TState>, ISpeculativeReader<T, TState>
    {
        public SpeculativeReader(IEnumerator<T> enumerator, GenerateEndItem<T> generateEndItem = null)
            : base(enumerator, generateEndItem)
        {
        }

        public TState GetMarkState(int mark) => Marks[mark].State;

        public void Mark() => CreateMark(Position, Index, default(TState));

        public void Mark(TState withState) => CreateMark(Position, Index, withState);
    }
}