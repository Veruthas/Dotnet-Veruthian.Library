using System;
using System.Collections.Generic;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Text;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    public abstract class SpeculativeLexer<TToken, TType, TReader> : Lexer<TToken, TType, TReader>
        where TToken : IToken<TType>
        where TReader : ISpeculativeReader<char>
    {
        Stack<TextLocation> markedLocations;

        protected virtual void OnRetreated(int fromPosition, int markCount)
        {
            if (markCount > markedLocations.Count)
                throw new ArgumentOutOfRangeException("markCount");
                
            if (capturing)
            {
                int length = fromPosition - reader.Position;

                if (length > buffer.Length)
                    ReleaseRead();
                else
                    buffer.Remove(buffer.Length - length, length);
            }

            for (int i = 0; i < markCount - 1; i++)
                markedLocations.Pop();

            this.location = markedLocations.Pop();
        }

        protected void Mark()
        {
            markedLocations.Push(this.location);
            reader.Mark();
        }

        protected void Commit()
        {
            reader.Commit();
        }


        protected int Retreat()
        {
            int fromPosition = reader.Retreat();

            OnRetreated(fromPosition, 1);

            return fromPosition;
        }

        protected int Retreat(int marks)
        {
            int fromPosition = reader.Retreat(marks);

            OnRetreated(fromPosition, marks);

            return fromPosition;
        }

        protected int RetreatAll()
        {
            int markCount = reader.MarkCount;

            int fromPosition = reader.RetreatAll();

            OnRetreated(fromPosition, markCount);

            return fromPosition;
        }
    }
}