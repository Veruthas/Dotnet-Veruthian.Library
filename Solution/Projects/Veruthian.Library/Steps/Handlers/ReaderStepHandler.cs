using Veruthian.Library.Collections;
using Veruthian.Library.Collections.Extensions;
using Veruthian.Library.Readers;
using Veruthian.Library.Readers.Extensions;
using Veruthian.Library.Types;

namespace Veruthian.Library.Steps.Handlers
{
    public class ReaderStepHandler<TState, T> : MatchStepHandler<TState, T>
        where TState : ILookup<string, object>
    {
        public ReaderStepHandler(string readerAddress) => this.ReaderAddress = readerAddress;

        public string ReaderAddress { get; }
                    
        
        protected override void Advance(TState state)
        {
            state.Get(ReaderAddress, out IReader<T> reader);

            reader.Advance();
        }

        protected override T GetCurrent(TState state)
        {
            state.Get(ReaderAddress, out IReader<T> reader);

            return reader.Current;
        }

        protected override bool IsEnd(TState state)
        {
            state.Get(ReaderAddress, out IReader<T> reader);

            return reader.IsEnd;
        }
    }
}