using System;
using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Numeric
{
    public sealed class BitArray : IMutableIndex<bool>
    {
        const int lengthOfUlong = 64;

        ulong value;

        ulong[] values;

        int count;


        public BitArray(int size)
        {
            if ((uint)size < lengthOfUlong)
            {
                this.value = 0;

                this.values = null;
            }
            else
            {
                int valueSize = size / lengthOfUlong;

                if (size % lengthOfUlong != 0)
                    valueSize++;

                values = new ulong[valueSize];
            }

            this.value = 0;
            this.count = size;
        }

        public bool this[int address]
        {
            get
            {
                if (TryGet(address, out var value))
                    return value;
                else
                    throw new IndexOutOfRangeException();
            }
            set
            {
                if (!TrySet(address, value))
                    throw new IndexOutOfRangeException();
            }
        }

        bool ILookup<int, bool>.this[int address] => this[address];

        public bool TryGet(int address, out bool value)
        {
            if (HasAddress(address))
            {
                ulong longValue;

                if (this.values == null)
                {
                    longValue = this.value;
                }
                else
                {
                    int segmentIndex = address / lengthOfUlong;

                    address = address % lengthOfUlong;

                    longValue = this.values[segmentIndex];
                }


                value = ((this.value >> address) & 0x1) == 0x1;

                return true;
            }
            else
            {
                value = false;

                return false;
            }
        }

        public bool TrySet(int address, bool value)
        {
            if (HasAddress(address))
            {
                if (this.values == null)
                {
                    SetBit(ref this.value, address, value);
                }
                else
                {
                    int valueIndex = address / lengthOfUlong;

                    SetBit(ref this.values[valueIndex], address % lengthOfUlong, value);
                }

                return true;
            }

            return false;
        }

        private static void SetBit(ref ulong value, int address, bool bit)
        {
            ulong longBit = 0x01;

            longBit <<= address;

            value &= ~longBit;

            value |= (bit ? longBit : 0x0);
        }

        public int Count => count;

        int IIndex<int, bool>.Start => 0;

        IEnumerable<int> ILookup<int, bool>.Addresses => Enumerables.GetRange(0, Count - 1);

        public IEnumerator<bool> GetEnumerator() => Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Values.GetEnumerator();

        private IEnumerable<bool> Values
        {
            get
            {
                if (values == null)
                {
                    var current = value;

                    for (int i = 0; i < count; i++)
                    {
                        bool value = (current & 0x1) == 0x1;

                        current >>= 1;

                        yield return value;
                    }
                }
                else
                {
                    int index = -1;

                    ulong current = 0;

                    for (int i = 0; i < count; i++)
                    {
                        if (i % lengthOfUlong == 0)
                            current = values[++index];

                        bool value = (current & 0x1) == 0x1;

                        current >>= 1;

                        yield return value;
                    }
                }
            }
        }

        IEnumerable<(int Address, bool Value)> ILookup<int, bool>.Pairs
        {
            get
            {
                if (values == null)
                {
                    var current = value;

                    for (int i = 0; i < count; i++)
                    {
                        bool value = (current & 0x1) == 0x1;

                        current >>= 1;

                        yield return (i, value);
                    }
                }
                else
                {
                    int index = -1;

                    ulong current = 0;

                    for (int i = 0; i < count; i++)
                    {
                        if (i % lengthOfUlong == 0)
                            current = values[++index];

                        bool value = (current & 0x1) == 0x1;

                        current >>= 1;

                        yield return (i, value);
                    }
                }
            }
        }

        bool ILookup<int, bool>.HasAddress(int address) => HasAddress(address);

        bool HasAddress(int address) => (uint)address < Count;

        bool IContainer<bool>.Contains(bool value)
        {
            ulong compareTo = value ? ulong.MinValue : ulong.MaxValue;

            if (this.values == null)
            {
                return this.value != compareTo;
            }
            else
            {
                for (int i = 0; i < this.values.Length; i++)
                    if (this.value != compareTo)
                        return true;

                return false;
            }
        }
    }
}