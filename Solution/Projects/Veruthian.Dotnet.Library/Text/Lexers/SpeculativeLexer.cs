using System;
using System.Collections.Generic;
using Veruthian.Dotnet.Library.Data.Readers;
using Veruthian.Dotnet.Library.Text.Code;

namespace Veruthian.Dotnet.Library.Text.Lexers
{
    public abstract class SpeculativeLexer<TToken, TType> : SpeculativeLexer<TToken, TType, ISpeculativeReader<CodePoint>>
        where TToken : IToken<TType>
    {
        public SpeculativeLexer(Source[] sources, Pool<CodeString, TType> pool) : base(sources, pool) { }
    }

    public abstract class SpeculativeLexer<TToken, TType, TReader> : LookaheadLexer<TToken, TType, TReader>
        where TToken : IToken<TType>
        where TReader : ISpeculativeReader<CodePoint>
    {

        public SpeculativeLexer(Source[] sources, Pool<CodeString, TType> pool) : base(sources, pool) { }

        protected Stack<TextLocation> markedLocations;

        protected virtual void OnRetreated(int fromPosition)
        {
            if (IsCapturing)
            {
                int length = fromPosition - Reader.Position;

                RollbackCaptured(length);
            }

            this.Location = markedLocations.Pop();
        }

        protected void Mark()
        {
            if (markedLocations == null)
                markedLocations = new Stack<TextLocation>();

            markedLocations.Push(this.Location);

            Reader.Mark();
        }

        protected void Commit()
        {
            Reader.Commit();
        }


        protected int Rollback()
        {
            int fromPosition = Reader.Position;

            Reader.Rollback();

            OnRetreated(fromPosition);

            return fromPosition;
        }
    }
}