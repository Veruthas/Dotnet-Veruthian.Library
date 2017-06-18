using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Text;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    public abstract class LookaheadLexer<TToken, TType, TReader> : Lexer<TToken, TType, TReader>
        where TToken : IToken<TType>
        where TReader : ILookaheadReader<char>
    {        
        protected virtual bool PeekIsEnd(int lookahead) => reader.PeekIsEnd(lookahead);

        protected virtual char Peek(int lookahead) => reader.Peek(lookahead);

        protected virtual bool PeekIsIn(int lookahead, CharacterSet set) => set.Includes(reader.Peek(lookahead));

        protected virtual bool PeekIs(int lookahead, char value) => reader.Peek(lookahead) == value;
    }
}