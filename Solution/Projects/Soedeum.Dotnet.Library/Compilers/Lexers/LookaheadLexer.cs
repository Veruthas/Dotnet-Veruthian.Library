using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Text;
using Soedeum.Dotnet.Library.Text.Char;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    public abstract class LookaheadLexer<TToken, TType> : Lexer<TToken, TType, ILookaheadReader<char>>
      where TToken : IToken<TType>
    {
        public LookaheadLexer(Source[] sources, TypedStringPool<TType> pool) : base(sources, pool) { }
    }

    public abstract class LookaheadLexer<TToken, TType, TReader> : Lexer<TToken, TType, TReader>
        where TToken : IToken<TType>
        where TReader : ILookaheadReader<char>
    {
        public LookaheadLexer(Source[] sources, TypedStringPool<TType> pool) : base(sources, pool) { }

        protected virtual bool PeekIsEnd(int lookahead) => Reader.PeekIsEnd(lookahead);

        protected virtual char Peek(int lookahead) => Reader.Peek(lookahead);

        protected virtual bool PeekIsIn(int lookahead, CharSet set) => set.Contains(Reader.Peek(lookahead));

        protected virtual bool PeekIs(int lookahead, char value) => Reader.Peek(lookahead) == value;
    }
}