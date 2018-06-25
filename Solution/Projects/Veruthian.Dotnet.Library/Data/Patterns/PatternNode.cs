namespace Veruthian.Dotnet.Library.Data.Patterns
{
    public class PatternNode<TNode, TType, TData> : IPatternNode<TNode, TType, TData>
        where TNode : PatternNode<TNode, TType, TData>
    {
        TType type;

        TData data;

        IPatternNodeList<TNode, TType, TData> children;


        public PatternNode(TType type, TData data, IPatternNodeList<TNode, TType, TData> children)
        {
            this.type = type;
            
            this.data = data;

            this.children = children;
        }
        

        public TType Type => type;

        public TData Data => data;

        public IPatternNodeList<TNode, TType, TData> Children => children;
    }
}