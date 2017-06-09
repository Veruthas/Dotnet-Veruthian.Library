using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Text;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    public class Lexer<TToken, TTokenType, TReader> : Lexer<TToken, TTokenType, TReader, Object>
        where TToken : IToken<TTokenType>
        where TTokenType : IEquatable<TTokenType>
        where TReader : IReader<char>
    {
        public Lexer(TReader reader)
            : base(reader) { }

    }


    public class Lexer<TToken, TTokenType, TReader, TState> : IEnumerator<TToken>
        where TToken : IToken<TTokenType>
        where TTokenType : IEquatable<TTokenType>
        where TReader : IReader<char>
    {
        bool initialized = false;


        // Capture Buffer
        bool capturing = false;

        StringBuilder buffer = new StringBuilder();

        TextLocation bufferLocation;



        protected TextLocation location;

        protected TToken current;

        protected TReader reader;


        public Lexer(TReader reader)
        {
            this.reader = reader;

            AttachBufferEvents();
        }

        public void Reset() => throw new NotImplementedException();

        public void Dispose() => reader.Dispose();


        public TToken Current
        {

            get
            {
                if (!initialized)
                    throw new InvalidOperationException("Lexer has not been initialized yet.");

                return current;
            }
        }

        object IEnumerator.Current => Current;


        public bool MoveNext()
        {
            throw new NotImplementedException();
        }


        // Buffer code
        protected void Capture()
        {

        }

        protected void Release()
        {

        }

        protected virtual void AttachBufferEvents()
        {
            reader.ItemRead += OnRead;

            if (reader is ISpeculativeReader<char, TState>)
            {
                var speculativeReader = reader as ISpeculativeReader<char, TState>;

                speculativeReader.Retreated += OnRetreated;
            }
        }

        protected virtual void OnRetreated(ISpeculativeReader<char, TState> reader,
                                            int fromPosition,
                                            int originalPosition,
                                            TState originalState)
        {
            int length = fromPosition - originalPosition;

        }

        protected virtual void OnRead(IReader<char> reader, char item)
        {
            if (capturing)
                buffer.Append(item);
        }
    }
}