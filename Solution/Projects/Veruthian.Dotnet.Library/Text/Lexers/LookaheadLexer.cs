using Veruthian.Dotnet.Library.Data.Readers;
using Veruthian.Dotnet.Library.Text.Code;

namespace Veruthian.Dotnet.Library.Text.Lexers
{
    public abstract class LookaheadLexer<TToken, TType> : LookaheadLexer<TToken, TType, ILookaheadReader<CodePoint>>
      where TToken : IToken<TType>
    {
        public LookaheadLexer(Source[] sources, Pool<CodeString, TType> pool) : base(sources, pool) { }
    }

    public abstract class LookaheadLexer<TToken, TType, TReader> : Lexer<TToken, TType, TReader>
        where TToken : IToken<TType>
        where TReader : ILookaheadReader<CodePoint>
    {
        public LookaheadLexer(Source[] sources, Pool<CodeString, TType> pool) : base(sources, pool) { }

        protected virtual bool PeekIsEnd(int lookahead) => Reader.PeekIsEnd(lookahead);

        protected virtual CodePoint Peek(int lookahead) => Reader.Peek(lookahead);

        protected virtual bool PeekIsIn(int lookahead, CodeSet set) => set.Contains(Reader.Peek(lookahead));

        protected virtual bool PeekIs(int lookahead, CodePoint value) => Reader.Peek(lookahead) == value;
    }
}