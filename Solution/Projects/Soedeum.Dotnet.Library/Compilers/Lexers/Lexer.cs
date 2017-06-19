using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Text;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    public abstract class Lexer<TToken, TType> : Lexer<TToken, TType, IReader<char>>
        where TToken : IToken<TType>
    {
        public Lexer(Source[] sources) : base(sources) { }
    }

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

        private Dictionary<string, string> internedStrings = new Dictionary<string, string>();


        // Constructor
        public Lexer(Source[] sources)
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

        public void Dispose()
        {
            reader.Dispose();

            foreach (Source source in sources)
                source.Dispose();
        }


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

        protected virtual string GetCaptured()
        {
            if (!capturing)
                throw new InvalidOperationException("Buffer is not capturing!");

            var captured = buffer.ToString();

            if (InternStrings)
            {
                if (internedStrings.ContainsKey(captured))
                    return internedStrings[captured];
                else
                    return internedStrings[captured] = captured;
            }
            else
            {
                return captured;
            }
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

        protected virtual bool PeekIsIn(CharSet set) => set.Contains(Reader.Peek());

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

            if (!Reader.IsEnd)
            {
                current = GetNextToken();
                return true;
            }
            else
            {
                // Store potential eof token info
                var source = SourceName;
                var location = Location;
                var value = Peek();

                bool success = GetNextReader();

                if (EofBetweenSources || !success)
                    current = CreateToken(EofType, source, location, value.ToString());
                else
                    current = GetNextToken();

                return success;
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
                this.location = default(TextLocation);

                this.reader = CreateReader(sources[sourceIndex++]);

                this.ReleaseCaptured();

                return true;
            }
        }

        protected virtual TToken CreateTokenFromBuffer(TType type, bool releaseBuffer = true)
        {
            string value = GetCaptured();

            var token = CreateToken(type, SourceName, bufferLocation, value);

            if (releaseBuffer)
                ReleaseCaptured();

            return token;
        }

        // Abstracts
        protected abstract TReader CreateReader(Source source);

        protected abstract TToken CreateToken(TType type, string source, TextLocation location, string value);


        protected abstract TType EofType { get; }

        protected abstract bool EofBetweenSources { get; }

        protected abstract bool InternStrings { get; }

        protected abstract TToken GetNextToken();
    }
}