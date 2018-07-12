using System;
using System.Collections.Generic;
using Veruthian.Dotnet.Library.Collections;

namespace Veruthian.Dotnet.Library.Numeric
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

        public bool this[int index]
        {
            get
            {
                if (TryGet(index, out var value))
                    return value;
                else
                    throw new IndexOutOfRangeException();
            }
            set
            {
                if (!TrySet(index, value))
                    throw new IndexOutOfRangeException();
            }
        }

        bool ILookup<int, bool>.this[int index] => this[index];

        public bool TryGet(int index, out bool value)
        {
            if (HasIndex(index))
            {
                ulong longValue;

                if (this.values == null)
                {
                    longValue = this.value;
                }
                else
                {
                    int segmentIndex = index / lengthOfUlong;

                    index = index % lengthOfUlong;

                    longValue = this.values[segmentIndex];
                }


                value = ((this.value >> index) & 0x1) == 0x1;

                return true;
            }
            else
            {
                value = false;

                return false;
            }
        }

        public bool TrySet(int index, bool value)
        {
            if (HasIndex(index))
            {
                if (this.values == null)
                {
                    SetBit(ref this.value, index, value);
                }
                else
                {
                    int valueIndex = index / lengthOfUlong;

                    SetBit(ref this.values[valueIndex], index % lengthOfUlong, value);
                }

                return true;
            }

            return false;
        }

        private static void SetBit(ref ulong value, int index, bool bit)
        {
            ulong longBit = 0x01;

            longBit <<= index;

            value &= ~longBit;

            value |= (bit ? longBit : 0x0);
        }

        public int Count => count;


        IEnumerable<int> ILookup<int, bool>.Keys => NumericUtility.GetRange(0, Count - 1);


        IEnumerable<bool> IContainer<bool>.Values
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

        IEnumerable<KeyValuePair<int, bool>> ILookup<int, bool>.Pairs
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

                        yield return new KeyValuePair<int, bool>(i, value);
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

                        yield return new KeyValuePair<int, bool>(i, value);
                    }
                }
            }
        }

        LookupDensity ILookup<int, bool>.Density => LookupDensity.Dense;
        
        bool ILookup<int, bool>.HasKey(int index) => HasIndex(index);

        bool HasIndex(int index) => (uint)index < Count;

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