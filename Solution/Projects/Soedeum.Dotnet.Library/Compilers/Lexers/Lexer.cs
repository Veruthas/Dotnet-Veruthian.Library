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
        where TReader : IReader<char>
    {
        public Lexer(TReader reader)
            : base(reader) { }

    }


    public abstract class Lexer<TToken, TTokenType, TReader, TState> : IEnumerator<TToken>
        where TToken : IToken<TTokenType>
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


        // Buffer code
        protected void Capture()
        {
            Release();
            bufferLocation = location;
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

            location = location.MoveToNext(item, reader.Peek());
        }

        // Token code
        protected abstract TToken CreateEofToken(TextLocation location);

        protected abstract TToken CreateToken(TTokenType tokenType, TextLocation location, string value);

        protected virtual TToken CreateTokenFromBuffer(TTokenType tokenType, bool releaseBuffer = true)
        {
            string value = GetDefaultString(tokenType) ?? Extract();

            var token = CreateToken(tokenType, bufferLocation, value);

            if (releaseBuffer)
                Release();

            return token;
        }

        protected abstract string GetDefaultString(TTokenType tokenType);

        protected abstract TToken GetNextToken();

        public virtual bool MoveNext()
        {
            initialized = true;

            if (!reader.IsEnd)
            {
                current = GetNextToken();
                return true;
            }
            else
            {
                current = CreateEofToken(location);
                return false;
            }

        }
    }
}