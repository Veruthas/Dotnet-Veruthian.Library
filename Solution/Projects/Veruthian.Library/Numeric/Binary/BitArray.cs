using System;
using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Collections;

using Unit = System.UInt64;

namespace Veruthian.Library.Numeric.Binary
{
    public sealed class BitArray : IMutableVector<bool>
    {
        const int UnitBits = 64;

        Unit value;

        Unit[] values;

        Number count;


        public BitArray(Number size)
        {
            if ((uint)size < UnitBits)
            {
                this.value = 0;

                this.values = null;
            }
            else
            {
                int valueSize = size.ToCheckedSignedInt() / UnitBits;

                if (size % UnitBits != 0)
                    valueSize++;

                values = new Unit[valueSize];
            }

            this.value = 0;

            this.count = size;
        }

        public bool this[Number address]
        {
            get
            {
                if (TryGet(address, out var value))
                    return value;
                else
                    throw new ArgumentOutOfRangeException(nameof(address));
            }
            set
            {
                if (!TrySet(address, value))
                    throw new ArgumentOutOfRangeException(nameof(address));
            }
        }

        bool ILookup<Number, bool>.this[Number address] => this[address];

        public bool TryGet(Number address, out bool value)
        {
            if (HasAddress(address))
            {
                Unit longValue;

                var intAddress = (int)address;

                if (this.values == null)
                {
                    longValue = this.value;
                }
                else
                {
                    var segmentIndex = intAddress / UnitBits;

                    intAddress = intAddress % UnitBits;

                    longValue = this.values[segmentIndex];
                }


                value = ((this.value >> intAddress) & 0x1) == 0x1;

                return true;
            }
            else
            {
                value = false;

                return false;
            }
        }

        public bool TrySet(Number address, bool value)
        {
            if (HasAddress(address))
            {
                var intAddress = (int)address;

                if (this.values == null)
                {
                    SetBit(ref this.value, intAddress, value);
                }
                else
                {
                    int valueIndex = intAddress / UnitBits;

                    SetBit(ref this.values[valueIndex], intAddress % UnitBits, value);
                }

                return true;
            }

            return false;
        }

        private static void SetBit(ref Unit value, int address, bool bit)
        {
            Unit longBit = 0x01;

            longBit <<= address;

            value &= ~longBit;

            value |= (bit ? longBit : 0x0);
        }

        public Number Count => count;

        Number IVector<Number, bool>.Start => Number.Zero;

        IEnumerable<Number> ILookup<Number, bool>.Addresses => Enumerables.GetRange(0, Count - 1);

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
                        if (i % UnitBits == 0)
                            current = values[++index];

                        bool value = (current & 0x1) == 0x1;

                        current >>= 1;

                        yield return value;
                    }
                }
            }
        }

        IEnumerable<(Number Address, bool Value)> ILookup<Number, bool>.Pairs
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
                        if (i % UnitBits == 0)
                            current = values[++index];

                        bool value = (current & 0x1) == 0x1;

                        current >>= 1;

                        yield return (i, value);
                    }
                }
            }
        }

        bool ILookup<Number, bool>.HasAddress(Number address) => HasAddress(address);

        bool HasAddress(Number address) => (uint)address < Count;

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