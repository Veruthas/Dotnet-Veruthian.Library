using System.Collections;
using System.Collections.Generic;
using Veruthian.Dotnet.Library.Collections.Extensions;

namespace Veruthian.Dotnet.Library.Collections
{
    public class DataSet<T> : IExpandableContainer<T>, IEnumerable<T>
    {
        HashSet<T> items;


        public DataSet() { }

        public DataSet(IEqualityComparer<T> comparer) => this.items = new HashSet<T>(comparer);

        public DataSet(params T[] items) => this.items = new HashSet<T>(items);

        public DataSet(IEnumerable<T> items) => this.items = new HashSet<T>(items);

        public DataSet(IEqualityComparer<T> comparer, params T[] items) => this.items = new HashSet<T>(items, comparer);

        public DataSet(IEqualityComparer<T> comparer, IEnumerable<T> items) => this.items = new HashSet<T>(items, comparer);        


        public int Count => items.Count;

        public bool Contains(T value) => items.Contains(value);

        public void Add(T value) => items.Add(value);

        public bool Remove(T value) => items.Remove(value);

        public void Clear() => items.Clear();


        IEnumerable<T> IContainer<T>.Values => items;

        public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();



        public override string ToString() => this.ToListString("{", "}", ", ") ;        
    }
}