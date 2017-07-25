namespace Soedeum.Dotnet.Library.Data.Patterns
{
    public struct Transition<T>
    {
        public Transition(T on, int toIndex)
        {
            this.On = on;

            this.ToIndex = toIndex;
        }

        public T On { get; private set; }

        public int ToIndex { get; private set; }


        public Transition<T> Offset(int offset)
        {
            return new Transition<T>(On, toIndex: +offset);
        }
    }
}