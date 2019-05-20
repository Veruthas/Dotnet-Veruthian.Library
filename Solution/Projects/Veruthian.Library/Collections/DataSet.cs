using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Collections.Extensions;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public class DataSet<T> : IResizableContainer<T>, IEnumerable<T>
    {
        HashSet<T> items;


        public DataSet() => this.items = new HashSet<T>();

        public DataSet(IEqualityComparer<T> comparer) => this.items = new HashSet<T>(comparer);

        public DataSet(params T[] items) => this.items = new HashSet<T>(items);

        public DataSet(IEnumerable<T> items) => this.items = new HashSet<T>(items);

        public DataSet(IEqualityComparer<T> comparer, params T[] items) => this.items = new HashSet<T>(items, comparer);

        public DataSet(IEqualityComparer<T> comparer, IEnumerable<T> items) => this.items = new HashSet<T>(items, comparer);

        public Number Count => items.Count;


        public bool Contains(T value) => items.Contains(value);

        public void Add(T value) => items.Add(value);

        public void AddRange(IEnumerable<T> values)
        {
            this.items.UnionWith(values);
        }

        public bool Remove(T value) => items.Remove(value);

        public void Clear() => items.Clear();


        public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        private const string start = "{", end = "}", separator = ", ";


        public override string ToString() => this.ToListString(start, end, separator);
    }
}