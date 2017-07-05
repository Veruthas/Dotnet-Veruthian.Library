using System;
using System.Text;

namespace Soedeum.Dotnet.Library.Numerics
{
    public struct Bits64
    {
        static ulong[] masks = { 0x0, 0xFF, 0xFFFF, 0xFF_FFFF, 0xFFFF_FFFF, 0xFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF, 0xFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF };

        ulong value;

        int length;


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

            this.value = value;
        }

        #endregion


        public const int MaxByteCount = 8;

        public static readonly Bits64 Empty = new Bits64();

        public static readonly Bits64 Min = new Bits64(0ul, 8);

        public static readonly Bits64 Max = new Bits64(ulong.MaxValue, 8);


        // Bits
        public int BitCount => length * 8;

        public bool GetBit(int index) => ((value >> index) & 0x1) == 1;

        public void SetBit(int index, bool value)
        {
            ulong mask = 0x1ul << index;

            ulong newValue;

            if (value)
                newValue = this.value | mask;
            else
                newValue = this.value & ~mask;

            this.value = newValue & masks[this.length];
        }

        // Byte
        public int ByteCount => length;

        private const int ByteOffset = 8;
        private const int ByteOffset0 = ByteOffset * 0;
        private const int ByteOffset1 = ByteOffset * 1;
        private const int ByteOffset2 = ByteOffset * 2;
        private const int ByteOffset3 = ByteOffset * 3;
        private const int ByteOffset4 = ByteOffset * 4;
        private const int ByteOffset5 = ByteOffset * 5;
        private const int ByteOffset6 = ByteOffset * 6;
        private const int ByteOffset7 = ByteOffset * 7;

        private const ulong ByteMask = 0xFF;

        public byte GetByte(int index)
        {
            int offset = ByteOffset * index;

            return (byte)((value >> offset) & ByteMask);
        }

        public void SetByte(int index, byte value)
        {
            int offset = ByteOffset * index;

            ulong mask = ByteMask << offset;

            ulong newValue = this.value & ~mask;

            newValue |= ((ulong)value << offset);

            this.value = newValue & masks[this.length];
        }

        // Try to optimize
        public void Reverse()
        {
            ulong newValue = 0;

            for (int i = 0; i < length; i++)
            {
                ulong value = (this.value >> (ByteOffset * i)) & ByteMask;

                value <<= (ByteOffset * length);

                newValue |= value;
            }

            this.value = newValue;
        }

        // Short
        public int ShortCount => length / 2 + ((length % 2 == 0) ? 0 : 1);

        private const int ShortOffset = 16;
        private const int ShortOffset0 = ShortOffset * 0;
        private const int ShortOffset1 = ShortOffset * 1;
        private const int ShortOffset2 = ShortOffset * 2;
        private const int ShortOffset3 = ShortOffset * 3;

        private const ulong ShortMask = 0xFFFF;

        public ushort GetShort(int index)
        {
            int offset = ByteOffset * index;

            return (ushort)((value >> offset) & ShortMask);
        }

        // Int
        private const int IntOffset = 16;
        private const int IntOffset0 = IntOffset * 0;
        private const int IntOffset1 = IntOffset * 1;

        private const ulong IntMask = 0xFFFF;


        public int IntCount => length / 4 + ((length % 4 == 0) ? 0 : 1);

        public uint GetInt(int index)
        {
            int offset = ByteOffset * index;

            return (uint)((value >> offset) & IntMask);
        }

        // Long
        public ulong GetLong() => value;


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