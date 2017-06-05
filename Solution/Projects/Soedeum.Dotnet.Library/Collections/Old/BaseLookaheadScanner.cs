using System;

namespace Soedeum.Dotnet.Library.Collections
{
    // public abstract class BaseLookaheadScanner<T, S> : BaseScanner<T, S>, ILookaheadScanner<T>
    //     where S : BaseLookaheadScanner<T, S>
    // {
    //     int endPosition = -1;
    //     T lastValid;
    //     Func<T, T> generateEndItem;


    //     public override bool IsEnd => PeekIsEnd(0);

    //     public T Peek(int lookahead)
    //     {
    //         VerifyInitialized();

    //         VerifyInRange(lookahead);

    //         return Get(lookahead);
    //     }

    //     public bool PeekIsEnd(int lookahead)
    //     {
    //         VerifyInitialized();

    //         VerifyInRange(lookahead);

    //         return Position + lookahead == endPosition;
    //     }


    //     protected abstract void VerifyInRange(int lookahead);
    // }
}