namespace Veruthian.Library.Collections
{
    public interface IStack<T> : IContainer<T>
    {
        void Push(T value);

        void Replace(T value);
        
        T Peek();

        T Pop();

        void Clear();
    }
}