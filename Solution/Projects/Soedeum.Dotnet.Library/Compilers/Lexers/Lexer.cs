using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Text;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    public abstract class Lexer<TToken, TTokenType, TReader> : Lexer<TToken, TTokenType, TReader, Object>
        where TToken : IToken<TTokenType>
        where TTokenType : IEquatable<TTokenType>
        where TReader : IReader<char>
    {
        public Lexer(TReader reader)
            : base(reader) { }

    }


    public abstract class Lexer<TToken, TTokenType, TReader, TState> : IEnumerator<TToken>
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

        // Token code
        protected abstract TToken CreateEofToken(TextLocation location);

        protected abstract TToken GetNextToken();

        protected abstract TToken CreateToken(TTokenType tokenType, TextLocation location, string value);

        // Todo add default string check?
        protected virtual TToken CreateTokenFromBuffer(TTokenType tokenType)
        {
            return CreateToken(tokenType, bufferLocation, Extract());
        }

        // Buffer code
        protected void Capture()
        {
            Release();
            capturing = true;
        }

        protected void Release()
        {
            buffer.Clear();
            bufferLocation = default(TextLocation);
            capturing = false;
        }

        protected string Extract()
        {
            if (!capturing)
                throw new InvalidOperationException("Buffer is not capturing!");

            return buffer.ToString();
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
            if (capturing)
            {
                int length = fromPosition - originalPosition;

                if (length > buffer.Length)
                    Release();
                else
                    buffer.Remove(buffer.Length - length, length);
            }
        }

        protected virtual void OnRead(IReader<char> reader, char item)
        {
            if (capturing)
                buffer.Append(item);
        }
    }
}