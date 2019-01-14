using System.Collections.Generic;

namespace Veruthian.Library.Patterns
{
    public interface IPatternNodeList<TNode, TType, TData> : IEnumerable<TNode>
        where TNode : IPatternNode<TNode, TType, TData>
    {
        int Count { get; }

        TNode this[int index] { get; }
    }
}