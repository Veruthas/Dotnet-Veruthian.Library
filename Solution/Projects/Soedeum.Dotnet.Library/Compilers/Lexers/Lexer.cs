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
        private bool initialized = false;

        private Source[] sources;

        private int sourceIndex;

        private TToken current;

        private TextLocation location;

        private TReader reader;


        // Buffer data
        private bool capturing = false;

        private StringBuilder buffer = new StringBuilder();

        private TextLocation bufferLocation;


        // Constructor
        public Lexer(params Source[] sources)
        {
            if (sources == null || sources.Length == 0)
                sources = new Source[] { new Source(null, EmptyEnumerable<char>.Default.GetEnumerator()) };

            this.sources = sources;

        }


        // Properties
        protected TReader Reader => reader;

        protected virtual TextLocation Location { get => location; set => location = value; }


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
        protected bool IsCapturing => capturing;

        protected virtual void CaptureRead()
        {
            ReleaseCaptured();
            bufferLocation = Location;
            capturing = true;
        }

        protected virtual void ReleaseCaptured(bool stopCapture = true)
        {
            buffer.Clear();

            if (stopCapture)
            {
                capturing = false;
                bufferLocation = default(TextLocation);
            }
            else
            {
                bufferLocation = Location;
            }
        }

        protected virtual string ExtractRead()
        {
            if (!capturing)
                throw new InvalidOperationException("Buffer is not capturing!");

            return buffer.ToString();
        }

        protected virtual void SetBuffer(string data, TextLocation? location = null, bool clear = false)
        {
            if (clear)
                buffer.Clear();

            buffer.Append(data);

            if (location.HasValue)
                bufferLocation = location.GetValueOrDefault();
        }

        protected virtual void RemoveFromBufferEnd(int length)
        {
            if (length > buffer.Length)
                ReleaseCaptured();
            else
                buffer.Remove(buffer.Length - length, length);
        }

        protected virtual void OnRead(char item)
        {
            if (capturing)
                buffer.Append(item);

            Location = Location.MoveToNext(item, Reader.Peek());
        }


        // Helper Code    
        protected virtual bool IsEnd => Reader.IsEnd;

        protected virtual char Peek() => Reader.Peek();

        protected virtual bool PeekIsIn(CharacterSet set) => set.Includes(Reader.Peek());

        protected virtual bool PeekIs(char value) => Reader.Peek() == value;

        protected virtual char Read()
        {
            char read = Reader.Read();

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

            if (!Reader.IsEnd || GetNextReader())
            {
                current = GetNextToken();
                return true;
            }
            else
            {
                current = CreateEofToken(SourceName, Location);
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
                ReleaseCaptured();

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