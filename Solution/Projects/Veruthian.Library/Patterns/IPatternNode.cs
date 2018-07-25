using System.Collections.Generic;

namespace Veruthian.Library.Patterns
{
    public interface IPatternNode<TNode, TType, TData>
        where TNode : IPatternNode<TNode, TType, TData>
    {
        TType Type { get; }

        TData Data { get; }

        IPatternNodeList<TNode, TType, TData> Children { get; }
    }
}