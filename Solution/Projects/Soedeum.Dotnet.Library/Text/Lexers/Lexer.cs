using System;
using System.Collections;
using System.Collections.Generic;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Text;
using Soedeum.Dotnet.Library.Text.Char;

namespace Soedeum.Dotnet.Library.Text.Lexers
{
    public abstract class Lexer<TToken, TType> : Lexer<TToken, TType, IReader<CodePoint>>
        where TToken : IToken<TType>
    {
        public Lexer(Source[] sources, TypedCodeStringPool<TType> pool) : base(sources, pool) { }
    }

    public abstract class Lexer<TToken, TType, TReader> : IEnumerator<TToken>
        where TToken : IToken<TType>
        where TReader : IReader<CodePoint>
    {
        // Reader data        
        private bool initialized = false;

        private Source[] sources;

        private int sourceIndex;

        private TToken current;

        private TextLocation location;

        private TReader reader;


        // String Pool
        private TypedCodeStringPool<TType> pool;


        // Capture data
        private bool capturing = false;

        private List<CodePoint> captured = new List<CodePoint>();

        private TextLocation captureLocation;


        // Constructor
        public Lexer(Source[] sources, TypedCodeStringPool<TType> pool)
        {
            if (sources == null || sources.Length == 0)
                sources = new Source[] { new Source(null, EmptyEnumerable<CodePoint>.Default.GetEnumerator()) };

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

            sources = null;

            reader = default(TReader);

            captured = null;

            pool = null;
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


        // String Pool
        protected TypedCodeStringPool<TType> CodeStringPool { get => pool; }


        // Capture code
        protected bool IsCapturing => capturing;

        protected virtual void StartCapture()
        {
            ReleaseCaptured();
            captureLocation = Location;
            capturing = true;
        }

        protected virtual void ReleaseCaptured(bool stopCapture = true)
        {
            captured.Clear();

            if (stopCapture)
            {
                capturing = false;
                captureLocation = default(TextLocation);
            }
            else
            {
                captureLocation = Location;
            }
        }

        protected virtual void RollbackCaptured(int length)
        {
            if (length > captured.Count)
                ReleaseCaptured();
            else
                captured.RemoveRange(captured.Count - length, length);
        }

        protected virtual TextLocation CaptureLocation { get => captureLocation; set => captureLocation = value; }

        protected virtual CodeString GetCaptured()
        {
            if (!capturing)
                throw new InvalidOperationException("Buffer is not capturing!");

            var captured = this.captured.ToCodeString();

            return captured;
        }

        protected virtual void AppendToCaptured(CodeString data, bool clear = false)
        {
            if (clear)
                captured.Clear();

            foreach (var point in data)
                captured.Add(point);
        }

        protected virtual void Capture(CodePoint item)
        {
            if (capturing)
                captured.Add(item);

            Location = Location.MoveToNext(item, Reader.Peek());
        }


        // Helper Code    
        protected virtual bool IsEnd => Reader.IsEnd;

        protected virtual CodePoint Peek() => Reader.Peek();

        protected virtual bool PeekIsIn(CodeSet set) => set.Contains(Reader.Peek());

        protected virtual bool PeekIs(CodePoint value) => Reader.Peek() == value;

        protected virtual CodePoint Read()
        {
            var read = Reader.Read();

            Capture(read);

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
                    current = CreateToken(EofType, source, location, value.ToCodeString());
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

        protected virtual TToken GetCapturedToken(TType type, bool releaseBuffer = true)
        {
            CodeString value = GetCaptured();

            var pool = CodeStringPool;

            if (pool != null)
            {
                var result = pool.Recall(value, type);

                value = result.Value;

                type = result.Type;
            }

            var token = CreateToken(type, SourceName, captureLocation, value);

            if (releaseBuffer)
                ReleaseCaptured();

            return token;
        }

        // Abstracts
        protected abstract TReader CreateReader(Source source);

        protected abstract TToken CreateToken(TType type, string source, TextLocation location, CodeString value);


        protected abstract TType EofType { get; }

        protected abstract bool EofBetweenSources { get; }


        protected abstract TToken GetNextToken();
    }
}