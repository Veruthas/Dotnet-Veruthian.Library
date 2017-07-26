namespace Soedeum.Dotnet.Library.Data.Patterns
{
    public struct PatternTransition<T>
    {
        public PatternTransition(T on, int toIndex)
        {
            this.On = on;

            this.ToIndex = toIndex;
        }

        public T On { get; private set; }

        public int ToIndex { get; private set; }


        public PatternTransition<T> Offset(int offset)
        {
            return new PatternTransition<T>(On, toIndex: +offset);
        }
    }
}