using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Text;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    public abstract class LookaheadLexer<TToken, TType> : Lexer<TToken, TType, ILookaheadReader<char>>
      where TToken : IToken<TType>
    {
        public LookaheadLexer(Source[] sources) : base(sources) { }
    }

    public abstract class LookaheadLexer<TToken, TType, TReader> : Lexer<TToken, TType, TReader>
        where TToken : IToken<TType>
        where TReader : ILookaheadReader<char>
    {
        public LookaheadLexer(Source[] sources) : base(sources) { }

        protected virtual bool PeekIsEnd(int lookahead) => Reader.PeekIsEnd(lookahead);

        protected virtual char Peek(int lookahead) => Reader.Peek(lookahead);

        protected virtual bool PeekIsIn(int lookahead, CharacterSet set) => set.Includes(Reader.Peek(lookahead));

        protected virtual bool PeekIs(int lookahead, char value) => Reader.Peek(lookahead) == value;
    }
}