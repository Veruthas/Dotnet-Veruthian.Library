using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Text;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    // THOUGHT: Should TReader be part of the signature?
    // It's there now to facilitate the inhertied Lexers
    public abstract class Lexer<TToken, TType, TReader> : IEnumerator<TToken>
        where TToken : IToken<TType>
        where TReader : IReader<char>
    {
        // Reader data        
        protected bool initialized = false;

        protected TextLocation location;

        protected TToken current;

        protected Source[] sources;

        protected int sourceIndex;

        protected TReader reader;



        public Lexer(params Source[] sources)
        {
            if (sources == null || sources.Length == 0)
                sources = new Source[] { new Source(null, EmptyEnumerable<char>.Default.GetEnumerator()) };

            this.sources = sources;

        }


        // Buffer data
        protected bool capturing = false;

        protected StringBuilder buffer = new StringBuilder();

        protected TextLocation bufferLocation;


        // IEnumerator<Token>
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

        // Source
        protected string SourceName { get => sources[sourceIndex].Name; }

        // Buffer code
        protected virtual void CaptureRead()
        {
            ReleaseRead();
            bufferLocation = location;
            capturing = true;
        }

        protected virtual void ReleaseRead()
        {
            buffer.Clear();
            bufferLocation = default(TextLocation);
            capturing = false;
        }

        protected virtual string ExtractRead()
        {
            if (!capturing)
                throw new InvalidOperationException("Buffer is not capturing!");

            return buffer.ToString();
        }


        protected virtual void OnRead(char item)
        {
            if (capturing)
                buffer.Append(item);

            location = location.MoveToNext(item, reader.Peek());
        }

        // Helper Code    
        protected virtual bool IsEnd => reader.IsEnd;

        protected virtual char Peek() => reader.Peek();

        protected virtual bool PeekIsIn(CharacterSet set) => set.Includes(reader.Peek());

        protected virtual bool PeekIs(char value) => reader.Peek() == value;

        protected virtual char Read()
        {
            char read = reader.Read();

            OnRead(read);

            return read;
        }

        protected void Read(int amount)
        {
            for (int i = 0; i < amount; i++)
                Read();
        }


        // Token code
        public bool MoveNext()
        {            
            initialized = true;

            if (!reader.IsEnd || GetNextReader())
            {
                current = GetNextToken();
                return true;
            }
            else
            {
                current = CreateEofToken(SourceName, location);
                return false;
            }

        }

        private bool GetNextReader()
        {            
            if (sourceIndex + 1 == sources.Length)
            {
                return false;
            }
            else
            {
                this.reader = CreateReader(sources[sourceIndex++]);
                return true;
            }
        }

        protected virtual TToken CreateTokenFromBuffer(TType type, bool releaseBuffer = true)
        {
            string value = GetDefaultString(type) ?? ExtractRead();

            var token = CreateToken(type, SourceName, bufferLocation, value);

            if (releaseBuffer)
                ReleaseRead();

            return token;
        }

        // Abstracts
        protected abstract TReader CreateReader(Source source);

        protected abstract TToken CreateToken(TType type, string source, TextLocation location, string value);

        protected abstract TToken CreateEofToken(string source, TextLocation location);

        protected abstract string GetDefaultString(TType type);

        protected abstract TToken GetNextToken();
    }
}