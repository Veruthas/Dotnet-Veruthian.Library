using System;
using System.Collections;
using System.Collections.Generic;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Compilers.Lexers;

namespace Soedeum.Dotnet.Library.Compilers.Parsers
{
    public abstract class Parser<TNode, TToken, TType, TReader> : IEnumerator<TNode>
        where TNode : INode<TToken, TType>
        where TToken : IToken<TType>
        where TReader : IReader<TToken>
    {
        public abstract TNode Current { get; }

        object IEnumerator.Current => throw new NotImplementedException();

        public abstract void Dispose();
        public abstract bool MoveNext();
        public abstract void Reset();
    }
}