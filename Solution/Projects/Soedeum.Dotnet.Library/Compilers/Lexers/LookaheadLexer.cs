using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Text;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    public abstract class LookaheadLexer<TToken, TTokenType, TReader> : Lexer<TToken, TTokenType, TReader>
        where TToken : IToken<TTokenType>
        where TReader : ILookaheadReader<char>
    {
        public LookaheadLexer(TReader reader)
            : base(reader) { }

        protected virtual bool PeekIsEnd(int lookahead) => reader.PeekIsEnd(lookahead);

        protected virtual char Peek(int lookahead) => reader.Peek(lookahead);

        protected virtual bool IsPeekIn(int lookahead, CharSet set) => set.Includes(reader.Peek(lookahead));

        protected virtual bool IsPeek(int lookahead, char value) => reader.Peek(lookahead) == value;
    }
}