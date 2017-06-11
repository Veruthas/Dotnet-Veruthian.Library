using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Text;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    public abstract class Lexer<TToken, TTokenType, TReader> : IEnumerator<TToken>
        where TToken : IToken<TTokenType>
        where TReader : IReader<char>
    {

        // Reader data        
        bool initialized = false;

        protected TextLocation location;

        protected TToken current;

        protected readonly TReader reader;



        // Buffer data
        protected bool capturing = false;

        protected StringBuilder buffer = new StringBuilder();

        protected TextLocation bufferLocation;


        // Constructor
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
        }


        protected virtual void OnRead(IReader<char> reader, char item)
        {
            if (capturing)
                buffer.Append(item);

            location = location.MoveToNext(item, reader.Peek());
        }

        // Helper Code    
        protected virtual bool IsEnd => reader.IsEnd;

        protected virtual char Peek() => reader.Peek();

        protected virtual bool IsPeekIn(CharSet set) => set.Includes(reader.Peek());

        protected virtual bool IsPeek(char value) => reader.Peek() == value;

        protected virtual char Read() => reader.Read();

        protected virtual void Read(int amount) => reader.Read(amount);


        // Token code
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

        protected virtual TToken CreateTokenFromBuffer(TTokenType tokenType, bool releaseBuffer = true)
        {
            string value = GetDefaultString(tokenType) ?? Extract();

            var token = CreateToken(tokenType, bufferLocation, value);

            if (releaseBuffer)
                Release();

            return token;
        }

        // Abstracts
        protected abstract TToken CreateEofToken(TextLocation location);

        protected abstract TToken CreateToken(TTokenType tokenType, TextLocation location, string value);

        protected abstract string GetDefaultString(TTokenType tokenType);

        protected abstract TToken GetNextToken();
    }
}