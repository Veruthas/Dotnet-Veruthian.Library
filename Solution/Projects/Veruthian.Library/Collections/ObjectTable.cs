using Veruthian.Library.Types;

namespace Veruthian.Library.Collections
{
    public class ObjectTable : DataLookup<string, object>
    {
        public T Get<T>(string address)
        {
            this.Get(address, out T result);

            return result;
        }

        public void Get<T>(string address, out T result)
        {
            object value = this[address];

            result = (T)value;
        }

        public bool TryGet<T>(string address, out T result)
        {
            object value = this[address];

            if (value is T)
            {
                result = (T)value;
                return true;
            }
            else
            {
                result = default(T);
                return false;
            }
        }

        public T GetOrInsert<T>(string address, T value)
        {
            return (T)this.GetOrInsert(address, value);
        }


        // Refs
        public Ref<T> InsertRef<T>(string address, T value = default(T))
        {
            var refValue = new Ref<T>(value);

            this.Insert(address, refValue);

            return refValue;
        }

        public Ref<T> GetOrInsertRef<T>(string address, T value = default(T))
        {
            var refValue = new Ref<T>(value);

            refValue = (Ref<T>)this.GetOrInsert(address, refValue);

            return refValue;
        }

        public Ref<T> GetRef<T>(string address)
        {
            this.Get(address, out Ref<T> result);

            return result;
        }

        public bool TryGetRef<T>(string address, out Ref<T> value)
        {
            return this.TryGet(address, out value);
        }

        public void SetRef<T>(string address, T value)
        {
            var refItem = GetRef<T>(address);

            refItem.Value = value;
        }


        // Flags
        public bool GetFlag(string flag)
        {
            Get(flag, out Ref<bool> result);

            return result.Value;
        }

        public void ToggleFlag(string flag)
        {
            Get(flag, out Ref<bool> result);

            result.Value = !result.Value;
        }

        public void SetFlag(string flag, bool value)
        {
            Get(flag, out Ref<bool> result);

            result.Value = value;
        }
    }
}