namespace Soedeum.Dotnet.Library.Data.Patterns
{
    public struct PatternTransition<T>
    {
        T on;

        int toIndex;


        public PatternTransition(T on, int toIndex)
        {
            this.on = on;

            this.toIndex = toIndex;
        }


        public T On => on;

        public int ToIndex => toIndex;


        public PatternTransition<T> Offset(int offset)
        {
            return new PatternTransition<T>(On, toIndex: +offset);
        }
    }
}