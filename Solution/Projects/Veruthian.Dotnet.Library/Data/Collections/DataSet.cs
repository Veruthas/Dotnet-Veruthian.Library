using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public class DataSet<T> : IExpandableContainer<T>
    {
        HashSet<T> items;


        public DataSet() => items = new HashSet<T>();

        public DataSet(IEnumerable<T> items) => items = new HashSet<T>(items);


        public int Count => items.Count;


        public IEnumerable<T> Values => items;

        public bool Contains(T value) => items.Contains(value);


        public void Add(T value) => items.Add(value);

        public bool Remove(T value) => items.Remove(value);

        public void Clear() => items.Clear();

    }
}