using System.Collections.Generic;
using System.Linq;

namespace Veruthian.Library.Collections
{
    public class DataIndex<T> : BaseIndex<T>
    {
        T[] items;


        public DataIndex() => this.items = new T[0];

        public DataIndex(IEnumerable<T> items) => this.items = items.ToArray();


        public override int Count => items.Length;

        public override bool Contains(T value)
        {
            if (value == null)
            {
                foreach (var item in items)
                {
                    if (item == null)
                        return true;
                }
            }
            else
            {
                foreach (var item in items)
                {
                    if (item.Equals(value))
                        return true;
                }
            }

            return false;
        }
        
        protected override T RawGet(int verifiedIndex) => items[verifiedIndex];
    }
}