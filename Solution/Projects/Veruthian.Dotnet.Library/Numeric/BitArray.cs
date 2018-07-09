using System;
using System.Collections.Generic;
using Veruthian.Dotnet.Library.Collections;

namespace Veruthian.Dotnet.Library.Numeric
{
    public struct BitArray : IMutableIndex<bool>
    {
        const int lengthOfUlong = 64;

        ulong value;

        ulong[] values;

        int count;


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
            throw new System.NotImplementedException();
        }

        public bool TrySet(int index, bool value)
        {
            throw new System.NotImplementedException();
        }

        public int Count => count;


        int IIndex<bool>.StartIndex => 0;

        int IIndex<bool>.EndIndex => Count - 1;

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


        bool ILookup<int, bool>.HasKey(int index) => (uint)index < Count;

        bool IContainer<bool>.Contains(bool value) => IndexOf(value).HasValue;

        int? IIndex<bool>.IndexOf(bool value) => IndexOf(value);

        private int? IndexOf(bool value)
        {
            throw new System.NotImplementedException();
        }
    }
}