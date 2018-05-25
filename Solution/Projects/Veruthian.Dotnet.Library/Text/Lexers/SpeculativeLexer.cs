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

        protected List<TextLocation> markedLocations;

        protected virtual void OnRetreated(int fromPosition, int markCount)
        {
            if (markCount > markedLocations.Count)
                throw new ArgumentOutOfRangeException("markCount");

            if (IsCapturing)
            {
                int length = fromPosition - Reader.Position;

                RollbackCaptured(length);
            }

            int markIndex = markedLocations.Count - markCount;

            this.Location = markedLocations[markIndex];

            markedLocations.RemoveRange(markIndex, markCount);
        }

        protected void Mark()
        {
            if (markedLocations == null)
                markedLocations = new List<TextLocation>();

            markedLocations.Add(this.Location);

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

            OnRetreated(fromPosition, 1);

            return fromPosition;
        }

        protected int Rollback(int marks)
        {
            int fromPosition = Reader.Position;

            OnRetreated(fromPosition, marks);

            return fromPosition;
        }

        protected int RollbackAll()
        {
            int markCount = Reader.MarkCount;

            int fromPosition = Reader.Position;

            Reader.RollbackAll();

            OnRetreated(fromPosition, markCount);

            return fromPosition;
        }
    }
}