using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Patterns
{
    public interface IPatternNode<TNode, TType> : IEnumerable<TNode>
        where TNode : IPatternNode<TNode, TType>
    {
        TType Type { get; }

        bool HasChildren { get; }

        int ChildCount { get; }

        TNode GetChild(int index);
    }
}