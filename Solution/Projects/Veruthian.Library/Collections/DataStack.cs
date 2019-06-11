using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public class DataStack<T> : IStack<T>
    {
        Stack<T> stack = new Stack<T>();


        public Number Count => stack.Count;


        public void Push(T value) => stack.Push(value);

        public void Replace(T value) { stack.Pop(); stack.Push(value); }

        public T Peek() => stack.Peek();

        public T Pop() => stack.Pop();
        
        public void Clear() => stack.Clear();


        public bool Contains(T value) => stack.Contains(value);



        public IEnumerator<T> GetEnumerator() => stack.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => stack.GetEnumerator();
    }
}