using System.Collections.Generic;
using Soedeum.Dotnet.Library.Compilers.Lexers;

namespace Soedeum.Dotnet.Library.Compilers.Parsers
{
    public class Node<TNode, TToken, TType> : INode<TToken, TType>
        where TToken : IToken<TType>
        where TNode : INode<TToken, TType>
    {
        readonly TToken token;

        List<TNode> children;

        public Node(TToken token)
        {
            this.token = token;
        }

        public TToken Token { get => token; }


        public bool HasChildren { get => children != null && children.Count > 0; }
        
        public List<TNode> Children { get => children == null ? (children = new List<TNode>()) : children; }
    }
}