using Soedeum.Dotnet.Library.Collections;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    public abstract class SpeculativeLexer<TToken, TType, TReader> : SpeculativeLexer<TToken, TType, TReader, object>
        where TToken : IToken<TType>
        where TReader : ISpeculativeReader<char>
    {
        public SpeculativeLexer(TReader reader)
            : base(reader) { }
    }

    public abstract class SpeculativeLexer<TToken, TType, TReader, TState> : Lexer<TToken, TType, TReader>
        where TToken : IToken<TType>
        where TReader : ISpeculativeReader<char, TState>
    {
        public SpeculativeLexer(TReader reader)
            : base(reader) { }


        protected override void AttachBufferEvents()
        {
            base.AttachBufferEvents();

            reader.Retreated += OnRetreated;
        }

        protected virtual void OnRetreated(ISpeculativeReader<char, TState> reader,
                                int fromPosition,
                                int originalPosition,
                                TState originalState)
        {
            if (capturing)
            {
                int length = fromPosition - originalPosition;

                if (length > buffer.Length)
                    ReleaseRead();
                else
                    buffer.Remove(buffer.Length - length, length);
            }
        }

        void Mark(TState withState = default(TState)) => reader.Mark();

        void Commit() => reader.Commit();

        void Retreat() => reader.Retreat();

        void Retreat(int marks) => reader.Retreat();

        void RetreatAll() => reader.RetreatAll();

    }
}