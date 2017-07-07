using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Soedeum.Dotnet.Library.Numerics
{
    public struct Bits64
    {
        static ulong[] masks = { 0x0, 0xFF, 0xFFFF, 0xFF_FFFF, 0xFFFF_FFFF, 0xFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF, 0xFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF };

        readonly ulong value;

        readonly int length;


        /* Constructors */
        #region Constructors

        // Bytes
        public static implicit operator Bits64(byte value) => new Bits64(value);

        public Bits64(byte value0)
        {
            this.length = 1;

            this.value = value0;
        }
        public Bits64(byte value0, byte value1)
        {
            this.length = 2;

            this.value = (ulong)value0
                | ((ulong)value1 << ByteOffset1);
        }
        public Bits64(byte value0, byte value1, byte value2)
        {
            this.length = 3;

            this.value = (ulong)value0
                | ((ulong)value1 << ByteOffset1)
                | ((ulong)value2 << ByteOffset2);
        }
        public Bits64(byte value0, byte value1, byte value2, byte value3)
        {
            this.length = 4;

            this.value = (ulong)value0
                | ((ulong)value1 << ByteOffset1)
                | ((ulong)value2 << ByteOffset2)
                | ((ulong)value3 << ByteOffset3);
        }
        public Bits64(byte value0, byte value1, byte value2, byte value3, byte value4)
        {
            this.length = 5;

            this.value = (ulong)value0
                | ((ulong)value1 << ByteOffset1)
                | ((ulong)value2 << ByteOffset2)
                | ((ulong)value3 << ByteOffset3)
                | ((ulong)value4 << ByteOffset4);
        }
        public Bits64(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5)
        {
            this.length = 6;

            this.value = (ulong)value0
                | ((ulong)value1 << ByteOffset1)
                | ((ulong)value2 << ByteOffset2)
                | ((ulong)value3 << ByteOffset3)
                | ((ulong)value4 << ByteOffset4)
                | ((ulong)value5 << ByteOffset5);
        }
        public Bits64(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5, byte value6)
        {
            this.length = 7;

            this.value = (ulong)value0
                | ((ulong)value1 << ByteOffset1)
                | ((ulong)value2 << ByteOffset2)
                | ((ulong)value3 << ByteOffset3)
                | ((ulong)value4 << ByteOffset4)
                | ((ulong)value5 << ByteOffset5)
                | ((ulong)value6 << ByteOffset6);
        }
        public Bits64(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5, byte value6, byte value7)
        {
            this.length = 8;

            this.value = (ulong)value0
                | ((ulong)value1 << ByteOffset1)
                | ((ulong)value2 << ByteOffset2)
                | ((ulong)value3 << ByteOffset3)
                | ((ulong)value4 << ByteOffset4)
                | ((ulong)value5 << ByteOffset5)
                | ((ulong)value6 << ByteOffset6)
                | ((ulong)value7 << ByteOffset7);
        }

        // Short
        public static implicit operator Bits64(ushort value) => new Bits64(value);

        public Bits64(ushort value0)
        {
            this.length = 2;

            this.value = (ulong)value0;
        }
        public Bits64(ushort value0, ushort value1)
        {
            this.length = 4;

            this.value = (ulong)value0
                    | ((ulong)value1 << ShortOffset1);
        }
        public Bits64(ushort value0, ushort value1, ushort value2)
        {
            this.length = 6;

            this.value = (ulong)value0
                    | ((ulong)value1 << ShortOffset1)
                    | ((ulong)value2 << ShortOffset2);
        }
        public Bits64(ushort value0, ushort value1, ushort value2, ushort value3)
        {
            this.length = 8;

            this.value = (ulong)value0
                    | ((ulong)value1 << ShortOffset1)
                    | ((ulong)value2 << ShortOffset2)
                    | ((ulong)value3 << ShortOffset3);
        }

        // Int
        public static implicit operator Bits64(uint value) => new Bits64();

        public Bits64(uint value0)
        {
            this.length = 4;

            this.value = (ulong)value0;
        }

        public Bits64(uint value0, uint value1)
        {
            this.length = 8;

            this.value = (ulong)value0
                    | ((ulong)value1 << IntOffset1);
        }

        // Long
        public static implicit operator Bits64(ulong value) => new Bits64(value);

        public Bits64(ulong value)
        {
            this.length = 8;

            this.value = value;
        }

        public Bits64(ulong value, int byteCount)
        {
            this.length = Math.Min(byteCount, MaxByteCount);

            this.value = value & masks[length];
        }

        #endregion


        public const int MaxByteCount = 8;

        public static readonly Bits64 Empty = new Bits64();

        public static readonly Bits64 Min = new Bits64(0ul, 8);

        public static readonly Bits64 Max = new Bits64(ulong.MaxValue, 8);


        // Bits
        public int BitCount => length * 8;

        public bool GetBit(int index) => ((value >> index) & 0x1) == 1;

        public Bits64 SetBit(int index, bool value)
        {
            ulong mask = 0x1ul << index;

            ulong newValue;

            if (value)
                newValue = this.value | mask;
            else
                newValue = this.value & ~mask;

            return new Bits64(newValue, this.length);
        }

        // Nibble
        public const int NibbleBitCount = 4;

        public const ulong NibbleMask = 0xF;

        public int NibbleCount => length * 2;

        public byte GetNibble(int index)
        {
            int offset = NibbleBitCount * index;

            return (byte)((value >> offset) & NibbleMask);
        }

        public Bits64 SetNibble(int index, byte value)
        {
            int offset = NibbleBitCount * index;

            ulong mask = NibbleMask << offset;

            ulong newValue = this.value & ~mask;

            newValue |= ((ulong)value << offset);

            return new Bits64(newValue, this.length);
        }

        public Bits64 ReverseNibbles()
        {
            ulong newValue = 0;

            int length = NibbleCount;

            for (int i = 0; i < length; i++)
            {
                ulong value = (this.value >> (NibbleBitCount * i)) & NibbleMask;

                value <<= (NibbleBitCount * (length - i - 1));

                newValue |= value;
            }

            return new Bits64(newValue, this.length);
        }

        // Byte
        public int ByteCount => length;

        private const int ByteBitCount = 8;
        private const int ByteOffset0 = ByteBitCount * 0;
        private const int ByteOffset1 = ByteBitCount * 1;
        private const int ByteOffset2 = ByteBitCount * 2;
        private const int ByteOffset3 = ByteBitCount * 3;
        private const int ByteOffset4 = ByteBitCount * 4;
        private const int ByteOffset5 = ByteBitCount * 5;
        private const int ByteOffset6 = ByteBitCount * 6;
        private const int ByteOffset7 = ByteBitCount * 7;

        private const ulong ByteMask = 0xFF;

        public byte GetByte(int index)
        {
            int offset = ByteBitCount * index;

            return (byte)((value >> offset) & ByteMask);
        }

        public Bits64 SetByte(int index, byte value)
        {
            int offset = ByteBitCount * index;

            ulong mask = ByteMask << offset;

            ulong newValue = this.value & ~mask;

            newValue |= ((ulong)value << offset);

            return new Bits64(newValue, this.length);
        }

        public Bits64 ReverseBytes()
        {
            ulong newValue = 0;

            int length = ByteCount;

            for (int i = 0; i < length; i++)
            {
                ulong value = (this.value >> (ByteBitCount * i)) & ByteMask;

                value <<= (ByteBitCount * (length - i - 1));

                newValue |= value;
            }

            return new Bits64(newValue, this.length);
        }

        // Short
        public int ShortCount => length / 2 + ((length % 2 == 0) ? 0 : 1);

        private const int ShortBitCount = 16;
        private const int ShortOffset0 = ShortBitCount * 0;
        private const int ShortOffset1 = ShortBitCount * 1;
        private const int ShortOffset2 = ShortBitCount * 2;
        private const int ShortOffset3 = ShortBitCount * 3;

        private const ulong ShortMask = 0xFFFF;

        public ushort GetShort(int index)
        {
            int offset = ByteBitCount * index;

            return (ushort)((value >> offset) & ShortMask);
        }

        public Bits64 SetShort(int index, ushort value)
        {
            int offset = ShortBitCount * index;

            ulong mask = ShortMask << offset;

            ulong newValue = this.value & ~mask;

            newValue |= ((ulong)value << offset);

            return new Bits64(newValue, this.length);
        }

        public Bits64 ReverseShorts()
        {
            ulong newValue = 0;

            int length = ShortCount;

            for (int i = 0; i < length; i++)
            {
                ulong value = (this.value >> (ShortBitCount * i)) & ShortMask;

                value <<= (ShortBitCount * (length - i - 1));

                newValue |= value;
            }

            return new Bits64(newValue, this.length);
        }

        public Bits64 ReverseShortBytes()
        {
            const ulong mask0 = 0x00FF_00FF_00FF_00FF;
            const ulong mask1 = 0xFF00_FF00_FF00_FF00;            

            ulong value0 = this.value & mask0;
            ulong value1 = this.value & mask1;

            ulong newValue = (value0 << ByteBitCount) | (value1 >> ByteBitCount);
            
            return new Bits64(newValue, this.length);
        }

        // Int
        private const int IntBitCount = 16;
        private const int IntOffset0 = IntBitCount * 0;
        private const int IntOffset1 = IntBitCount * 1;

        private const ulong IntMask = 0xFFFF;


        public int IntCount => length / 4 + ((length % 4 == 0) ? 0 : 1);

        public uint GetInt(int index)
        {
            int offset = ByteBitCount * index;

            return (uint)((value >> offset) & IntMask);
        }

        public Bits64 SetInt(int index, uint value)
        {
            int offset = IntBitCount * index;

            ulong mask = IntMask << offset;

            ulong newValue = this.value & ~mask;

            newValue |= ((ulong)value << offset);

            return new Bits64(newValue, this.length);
        }

        public Bits64 ReverseInts()
        {
            ulong newValue = 0;

            int length = IntCount;

            for (int i = 0; i < length; i++)
            {
                ulong value = (this.value >> (IntBitCount * i)) & IntMask;

                value <<= (IntBitCount * (length - i - 1));

                newValue |= value;
            }

            return new Bits64(newValue, this.length);
        }

        public uint[] ToIntArray()
        {
            uint[] values = new uint[ShortCount];

            for (int i = 0; i < IntCount; i++)
                values[i] = GetInt(i);

            return values;
        }


        // Long
        public ulong GetLong() => value;


        // Operations
        public Bits64 Invert()
        {
            ulong newValue = ~this.value;

            return new Bits64(newValue, this.length);
        }

        public static Bits64 operator ~(Bits64 value) => value.Invert();

        public static Bits64 operator &(Bits64 left, Bits64 right)
        {
            ulong result = left.value & right.value;

            int length = Math.Max(left.length, right.length);

            return new Bits64(result, length);
        }

        public static Bits64 operator |(Bits64 left, Bits64 right)
        {
            ulong result = left.value | right.value;

            int length = Math.Max(left.length, right.length);

            return new Bits64(result, length);
        }

        public static Bits64 operator ^(Bits64 left, Bits64 right)
        {
            ulong result = left.value ^ right.value;

            int length = Math.Max(left.length, right.length);

            return new Bits64(result, length);
        }



        // ToString()
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            bool initialized = false;

            for (int i = ByteCount - 1; i >= 0; i--)
            {
                if (initialized)
                    builder.Append('-');
                else
                    initialized = true;

                builder.Append(string.Format("{0:X2}", GetByte(i)));
            }

            return builder.ToString();
        }
    }
}