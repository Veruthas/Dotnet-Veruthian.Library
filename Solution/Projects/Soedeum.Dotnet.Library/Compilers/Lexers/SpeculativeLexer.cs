using Soedeum.Dotnet.Library.Collections;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    public abstract class SpeculativeLexer<TToken, TType, TReader> : SpeculativeLexer<TToken, TType, TReader, object>
        where TToken : IToken<TType>
        where TReader : ISpeculativeReader<char>
    {
    }

    public abstract class SpeculativeLexer<TToken, TType, TReader, TState> : Lexer<TToken, TType, TReader>
        where TToken : IToken<TType>
        where TReader : ISpeculativeReader<char, TState>
    {

        protected virtual void OnRetreated(int length)
        {
            if (capturing)
            {
                if (length > buffer.Length)
                    ReleaseRead();
                else
                    buffer.Remove(buffer.Length - length, length);
            }
        }

        protected void Mark(TState withState = default(TState)) => reader.Mark();

        protected void Commit() => reader.Commit();


        protected int Retreat()
        {
            int length = reader.Retreat();

            OnRetreated(length);

            return length;
        }

        protected int Retreat(int marks)
        {
            int length = reader.Retreat(marks);

            OnRetreated(length);

            return length;
        }

        protected int RetreatAll() 
        {
            int length = reader.RetreatAll();

            OnRetreated(length);

            return length;
        }

    }
}