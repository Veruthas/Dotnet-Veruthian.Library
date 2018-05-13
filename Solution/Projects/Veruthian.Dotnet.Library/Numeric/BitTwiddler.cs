using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Veruthian.Dotnet.Library.Numeric
{
    public struct BitTwiddler
    {

        /* Constants */
        #region Constants

        static readonly ulong[] masks = GetBitMasks();

        static ulong[] GetBitMasks()
        {
            ulong[] masks = new ulong[65];

            for (int i = 0; i < 64; i++)
            {
                unchecked
                {
                    masks[i] = ((ulong)0b1 << i) - 1;
                }
            }

            masks[64] = ulong.MaxValue;

            return masks;
        }


        // Count
        public const int BitsPerNibble = 4;

        public const int BitsPerByte = 8;

        public const int BitsPerShort = 16;

        public const int BitsPerInt = 32;

        public const int BitsPerLong = 64;


        public const int NibblesPerByte = BitsPerByte / BitsPerNibble;

        public const int NibblesPerShort = BitsPerShort / BitsPerNibble;

        public const int NibblesPerInt = BitsPerInt / BitsPerNibble;

        public const int NibblesPerLong = BitsPerLong / BitsPerNibble;


        public const int BytesPerShort = BitsPerShort / BitsPerByte;

        public const int BytesPerInt = BitsPerInt / BitsPerByte;

        public const int BytesPerLong = BitsPerLong / BitsPerByte;


        public const int ShortsPerInt = BitsPerInt / BitsPerShort;

        public const int ShortsPerLong = BitsPerLong / BitsPerShort;


        public const int IntsPerLong = BitsPerInt / BitsPerLong;


        // Max
        public const int MaxBitCount = 64;

        public const int MaxNibbleCount = MaxBitCount / BitsPerNibble;

        public const int MaxByteCount = MaxBitCount / BitsPerByte;

        public const int MaxShortCount = MaxBitCount / BitsPerShort;

        public const int MaxIntCount = MaxBitCount / BitsPerInt;

        public const int MaxLongCount = MaxBitCount / BitsPerLong;


        // Masks
        public const ulong BitMask = 0x1;

        public const ulong NibbleMask = 0xF;

        public const ulong ByteMask = 0xFF;

        public const ulong ShortMask = 0xFFFF;

        public const ulong IntMask = 0xFFFF_FFFF;

        public const ulong LongMask = 0xFFFF_FFFF_FFFF_FFFF;


        // Offsets
        public const int NibbleOffset0 = BitsPerNibble * 0;
        public const int NibbleOffset1 = BitsPerNibble * 1;
        public const int NibbleOffset2 = BitsPerNibble * 2;
        public const int NibbleOffset3 = BitsPerNibble * 3;
        public const int NibbleOffset4 = BitsPerNibble * 4;
        public const int NibbleOffset5 = BitsPerNibble * 5;
        public const int NibbleOffset6 = BitsPerNibble * 6;
        public const int NibbleOffset7 = BitsPerNibble * 7;
        public const int NibbleOffset8 = BitsPerNibble * 8;
        public const int NibbleOffset9 = BitsPerNibble * 9;
        public const int NibbleOffset10 = BitsPerNibble * 10;
        public const int NibbleOffset11 = BitsPerNibble * 11;
        public const int NibbleOffset12 = BitsPerNibble * 12;
        public const int NibbleOffset13 = BitsPerNibble * 13;
        public const int NibbleOffset14 = BitsPerNibble * 14;
        public const int NibbleOffset15 = BitsPerNibble * 15;

        public const int ByteOffset0 = BitsPerByte * 0;
        public const int ByteOffset1 = BitsPerByte * 1;
        public const int ByteOffset2 = BitsPerByte * 2;
        public const int ByteOffset3 = BitsPerByte * 3;
        public const int ByteOffset4 = BitsPerByte * 4;
        public const int ByteOffset5 = BitsPerByte * 5;
        public const int ByteOffset6 = BitsPerByte * 6;
        public const int ByteOffset7 = BitsPerByte * 7;

        public const int ShortOffset0 = BitsPerShort * 0;
        public const int ShortOffset1 = BitsPerShort * 1;
        public const int ShortOffset2 = BitsPerShort * 2;
        public const int ShortOffset3 = BitsPerShort * 3;

        private const int IntOffset0 = BitsPerInt * 0;
        private const int IntOffset1 = BitsPerInt * 1;


        // General
        public static readonly BitTwiddler Empty = new BitTwiddler();

        public static readonly BitTwiddler MinLong = new BitTwiddler(0ul, BitsPerLong);

        public static readonly BitTwiddler MaxLong = new BitTwiddler(ulong.MaxValue, BitsPerLong);

        #endregion

        readonly ulong value;

        readonly int bitCount;


        /* Constructors */
        #region Constructors

        private BitTwiddler(ulong value, int bitCount)
        {
            unchecked
            {
                ulong mask = masks[bitCount];

                this.value = value & mask;

                this.bitCount = bitCount;
            }
        }
        // Bits        


        // Bytes        
        public static implicit operator BitTwiddler(byte value) => FromByte(value);

        public static BitTwiddler FromByte(byte value)
        {
            int length = BitsPerByte * 1;

            return new BitTwiddler(value, length);
        }
        public static BitTwiddler FromBytes(byte value0, byte value1)
        {
            ulong value = (ulong)value0
                | ((ulong)value1 << ByteOffset1);

            int length = BitsPerByte * 2;

            return new BitTwiddler(value, length);
        }
        public static BitTwiddler FromBytes(byte value0, byte value1, byte value2)
        {
            ulong value = (ulong)value0
                | ((ulong)value1 << ByteOffset1)
                | ((ulong)value2 << ByteOffset2);

            int length = BitsPerByte * 3;

            return new BitTwiddler(value, length);
        }
        public static BitTwiddler FromBytes(byte value0, byte value1, byte value2, byte value3)
        {
            ulong value = (ulong)value0
                | ((ulong)value1 << ByteOffset1)
                | ((ulong)value2 << ByteOffset2)
                | ((ulong)value3 << ByteOffset3);

            int length = BitsPerByte * 4;

            return new BitTwiddler(value, length);
        }
        public static BitTwiddler FromBytes(byte value0, byte value1, byte value2, byte value3, byte value4)
        {
            ulong value = (ulong)value0
                | ((ulong)value1 << ByteOffset1)
                | ((ulong)value2 << ByteOffset2)
                | ((ulong)value3 << ByteOffset3)
                | ((ulong)value4 << ByteOffset4);

            int length = BitsPerByte * 5;

            return new BitTwiddler(value, length);
        }
        public static BitTwiddler FromBytes(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5)
        {
            ulong value = (ulong)value0
                | ((ulong)value1 << ByteOffset1)
                | ((ulong)value2 << ByteOffset2)
                | ((ulong)value3 << ByteOffset3)
                | ((ulong)value4 << ByteOffset4)
                | ((ulong)value5 << ByteOffset5);

            int length = BitsPerByte * 6;

            return new BitTwiddler(value, length);
        }
        public static BitTwiddler FromBytes(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5, byte value6)
        {
            ulong value = (ulong)value0
                | ((ulong)value1 << ByteOffset1)
                | ((ulong)value2 << ByteOffset2)
                | ((ulong)value3 << ByteOffset3)
                | ((ulong)value4 << ByteOffset4)
                | ((ulong)value5 << ByteOffset5)
                | ((ulong)value6 << ByteOffset6);

            int length = BitsPerByte * 7;

            return new BitTwiddler(value, length);
        }
        public static BitTwiddler FromBytes(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5, byte value6, byte value7)
        {
            ulong value = (ulong)value0
                | ((ulong)value1 << ByteOffset1)
                | ((ulong)value2 << ByteOffset2)
                | ((ulong)value3 << ByteOffset3)
                | ((ulong)value4 << ByteOffset4)
                | ((ulong)value5 << ByteOffset5)
                | ((ulong)value6 << ByteOffset6)
                | ((ulong)value7 << ByteOffset7);

            int length = BitsPerByte * 8;

            return new BitTwiddler(value, length);
        }

        public static BitTwiddler FromBytes(byte[] bytes) => FromBytes(bytes, 0, bytes.Length);

        public static BitTwiddler FromBytes(byte[] bytes, int index, int length)
        {
            if (index < 0 || index > bytes.Length)
                throw new ArgumentOutOfRangeException("index");
            if (length < 0 || index + length > bytes.Length)
                throw new ArgumentOutOfRangeException("length");

            int byteCount = Math.Min(length, MaxByteCount);

            var result = BitTwiddler.WithByteCount(byteCount);

            for (int i = 0; i < byteCount; i++)
                result = result.SetByte(bytes[i], i);

            return result;
        }

        public static BitTwiddler FromBytes(IEnumerable<byte> bytes, int length) => FromBytes(bytes.GetEnumerator(), length);


        public static BitTwiddler FromBytes(IEnumerator<byte> bytes, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("length");

            int byteCount = Math.Min(length, MaxByteCount);

            var result = BitTwiddler.WithByteCount(byteCount);

            for (int i = 0; i < byteCount; i++)
            {
                bytes.MoveNext();

                result = result.SetByte(bytes.Current);
            }

            return result;
        }


        // Sbyte
        public static implicit operator BitTwiddler(sbyte value) => FromByte(value);

        public static BitTwiddler FromByte(sbyte value)
        {
            return FromByte((byte)value);
        }
        public static BitTwiddler FromBytes(sbyte value0, sbyte value1)
        {
            return FromBytes((byte)value0, (byte)value1);
        }
        public static BitTwiddler FromBytes(sbyte value0, sbyte value1, sbyte value2)
        {
            return FromBytes((byte)value0, (byte)value1, (byte)value2);
        }
        public static BitTwiddler FromBytes(sbyte value0, sbyte value1, sbyte value2, sbyte value3)
        {
            return FromBytes((byte)value0, (byte)value1, (byte)value2, (byte)value3);
        }
        public static BitTwiddler FromBytes(sbyte value0, sbyte value1, sbyte value2, sbyte value3, sbyte value4)
        {
            return FromBytes((byte)value0, (byte)value1, (byte)value2, (byte)value3, (byte)value4);
        }
        public static BitTwiddler FromBytes(sbyte value0, sbyte value1, sbyte value2, sbyte value3, sbyte value4, sbyte value5)
        {
            return FromBytes((byte)value0, (byte)value1, (byte)value2, (byte)value3, (byte)value4, (byte)value5);
        }
        public static BitTwiddler FromBytes(sbyte value0, sbyte value1, sbyte value2, sbyte value3, sbyte value4, sbyte value5, sbyte value6)
        {
            return FromBytes((byte)value0, (byte)value1, (byte)value2, (byte)value3, (byte)value4, (byte)value5, (byte)value6);
        }
        public static BitTwiddler FromBytes(sbyte value0, sbyte value1, sbyte value2, sbyte value3, sbyte value4, sbyte value5, sbyte value6, sbyte value7)
        {
            return FromBytes((byte)value0, (byte)value1, (byte)value2, (byte)value3, (byte)value4, (byte)value5, (byte)value6, (byte)value7);
        }

        public static BitTwiddler FromBytes(sbyte[] bytes) => FromBytes(bytes, 0, bytes.Length);

        public static BitTwiddler FromBytes(sbyte[] bytes, int index, int length)
        {
            if (index < 0 || index > bytes.Length)
                throw new ArgumentOutOfRangeException("index");
            if (length < 0 || index + length > bytes.Length)
                throw new ArgumentOutOfRangeException("length");

            int byteCount = Math.Min(length, MaxByteCount);

            var result = BitTwiddler.WithByteCount(byteCount);

            for (int i = 0; i < byteCount; i++)
                result = result.SetSignedByte(bytes[i], i);

            return result;
        }

        public static BitTwiddler FromBytes(IEnumerable<sbyte> bytes, int length) => FromBytes(bytes.GetEnumerator(), length);


        public static BitTwiddler FromBytes(IEnumerator<sbyte> bytes, int length)
        {
            int byteCount = CorrectByteCount(length);

            var result = BitTwiddler.WithByteCount(byteCount);

            for (int i = 0; i < byteCount; i++)
            {
                bytes.MoveNext();

                result = result.SetSignedByte(bytes.Current);
            }

            return result;
        }

        // UShort
        public static implicit operator BitTwiddler(ushort value) => FromShort(value);

        public static BitTwiddler FromShort(ushort value)
        {
            int length = BitsPerShort * 1;

            return new BitTwiddler(value, length);
        }
        public static BitTwiddler FromShorts(ushort value0, ushort value1)
        {
            ulong value = (ulong)value0
                    | ((ulong)value1 << ShortOffset1);

            int length = BitsPerShort * 2;

            return new BitTwiddler(value, length);
        }
        public static BitTwiddler FromShorts(ushort value0, ushort value1, ushort value2)
        {
            ulong value = (ulong)value0
                    | ((ulong)value1 << ShortOffset1)
                    | ((ulong)value2 << ShortOffset2);

            int length = BitsPerShort * 3;

            return new BitTwiddler(value, length);
        }
        public static BitTwiddler FromShorts(ushort value0, ushort value1, ushort value2, ushort value3)
        {
            ulong value = (ulong)value0
                    | ((ulong)value1 << ShortOffset1)
                    | ((ulong)value2 << ShortOffset2)
                    | ((ulong)value3 << ShortOffset3);

            int length = BitsPerShort * 4;

            return new BitTwiddler(value, length);
        }

        // Short
        public static implicit operator BitTwiddler(short value) => FromShort(value);

        public static BitTwiddler FromShort(short value)
        {
            return FromShort((ushort)value);
        }
        public static BitTwiddler FromShorts(short value0, short value1)
        {
            return FromShorts((ushort)value0, (ushort)value1);
        }
        public static BitTwiddler FromShorts(short value0, short value1, short value2)
        {
            return FromShorts((ushort)value0, (ushort)value1, (ushort)value2);
        }
        public static BitTwiddler FromShorts(short value0, short value1, short value2, short value3)
        {
            return FromShorts((ushort)value0, (ushort)value1, (ushort)value2, (ushort)value3);
        }

        // Char
        public static implicit operator BitTwiddler(char value) => FromChar(value);

        public static BitTwiddler FromChar(char value)
        {
            return FromShort((ushort)value);
        }

        public static BitTwiddler FromChars(char value0, char value1)
        {
            return FromShorts((ushort)value0, (ushort)value1);
        }
        public static BitTwiddler FromChars(char value0, char value1, char value2)
        {
            return FromShorts((ushort)value0, (ushort)value1, (ushort)value2);
        }
        public static BitTwiddler FromChars(char value0, char value1, char value2, char value3)
        {
            return FromShorts((ushort)value0, (ushort)value1, (ushort)value2, (ushort)value3);
        }

        // UInt
        public static implicit operator BitTwiddler(uint value) => FromInt(value);

        public static BitTwiddler FromInt(uint value)
        {
            int length = BitsPerInt * 1;

            return new BitTwiddler(value, length);
        }

        public static BitTwiddler FromInts(uint value0, uint value1)
        {
            ulong value = (ulong)value0
                    | ((ulong)value1 << IntOffset1);

            int length = BitsPerInt * 2;

            return new BitTwiddler(value, length);
        }

        // Int
        public static implicit operator BitTwiddler(int value) => FromInt(value);

        public static BitTwiddler FromInt(int value)
        {
            return FromInt((uint)value);
        }
        public static BitTwiddler FromInts(int value0, int value1)
        {
            return FromInts((uint)value0, (uint)value1);
        }


        // ULong
        public static implicit operator BitTwiddler(ulong value) => FromLong(value);

        public static BitTwiddler FromLong(ulong value)
        {
            int length = BitsPerLong;

            return new BitTwiddler(value, length);
        }

        // Long
        public static implicit operator BitTwiddler(long value) => FromLong(value);

        public static BitTwiddler FromLong(long value)
        {
            return FromLong((ulong)value);
        }


        // Count
        public static BitTwiddler WithBitCount(int bitCount) => new BitTwiddler().ChangeBitCount(bitCount);

        public static BitTwiddler WithNibbleCount(int nibbleCount) => new BitTwiddler().ChangeNibbleCount(nibbleCount);

        public static BitTwiddler WithByteCount(int byteCount) => new BitTwiddler().ChangeByteCount(byteCount);

        public static BitTwiddler WithShortCount(int shortCount) => new BitTwiddler().ChangeShortCount(shortCount);

        public static BitTwiddler WithCharCount(int charCount) => new BitTwiddler().ChangeCharCount(charCount);

        public static BitTwiddler WithIntCount(int intCount) => new BitTwiddler().ChangeIntCount(intCount);


        #endregion


        /* Counts */
        #region Counts

        private static int CorrectBitCount(int bitCount) => (bitCount < 0) ? 0 : (bitCount > MaxBitCount) ? MaxBitCount : bitCount;

        private static int CorrectNibbleCount(int nibbleCount) => (nibbleCount < 0) ? 0 : (nibbleCount > MaxByteCount) ? MaxByteCount : nibbleCount;

        private static int CorrectByteCount(int byteCount) => (byteCount < 0) ? 0 : (byteCount > MaxByteCount) ? MaxByteCount : byteCount;

        private static int CorrectShortCount(int shortCount) => (shortCount < 0) ? 0 : (shortCount > MaxByteCount) ? MaxByteCount : shortCount;

        private static int CorrectIntCount(int intCount) => (intCount < 0) ? 0 : (intCount > MaxByteCount) ? MaxByteCount : intCount;


        public int RegionCount(int bitsPerRegion) => (bitCount + bitsPerRegion - 1) / bitsPerRegion;

        public int BitCount => bitCount;

        public int NibbleCount => (bitCount + BitsPerNibble - 1) / BitsPerNibble;

        public int ByteCount => (bitCount + BitsPerByte - 1) / BitsPerByte;

        public int ShortCount => (bitCount + BitsPerShort - 1) / BitsPerShort;

        public int CharCount => ShortCount;

        public int IntCount => (bitCount + BitsPerInt - 1) / BitsPerInt;


        public BitTwiddler ChangeBitCount(int bitCount) => new BitTwiddler(this.value, CorrectBitCount(bitCount));

        public BitTwiddler ChangeNibbleCount(int nibbleCount) => ChangeBitCount(nibbleCount * BitsPerNibble);

        public BitTwiddler ChangeByteCount(int byteCount) => ChangeBitCount(byteCount * BitsPerByte);

        public BitTwiddler ChangeShortCount(int shortCount) => ChangeBitCount(shortCount * BitsPerShort);

        public BitTwiddler ChangeCharCount(int charCount) => ChangeBitCount(charCount * BitsPerShort);

        public BitTwiddler ChangeIntCount(int intCount) => ChangeBitCount(intCount * BitsPerInt);

        #endregion


        /* Access and Modification */
        #region Access and Modification

        // Get
        private ulong GetValue(int itemIndex, int bitsPerItem, ulong itemMask)
        {
            int offset = itemIndex * bitsPerItem;

            return (value >> offset) & itemMask;
        }

        public ulong GetRegion(int bitsPerItem, int itemIndex = 0)
        {
            unchecked
            {
                ulong itemMask;

                if (bitsPerItem >= MaxBitCount)
                    itemMask = ulong.MaxValue;
                else
                    itemMask = (BitMask << bitsPerItem) - 1;

                return GetValue(itemIndex, bitsPerItem, itemMask);

            }
        }

        public bool GetBit(int bitIndex = 0) => ((value >> bitIndex) & BitMask) == 1;

        public Nibble GetNibble(int nibbleIndex = 0) => (byte)GetValue(nibbleIndex, BitsPerNibble, NibbleMask);

        public byte GetByte(int byteIndex = 0) => (byte)GetValue(byteIndex, BitsPerByte, ByteMask);

        public sbyte GetSignedByte(int byteIndex = 0) => (sbyte)GetByte(byteIndex);

        public ushort GetShort(int shortIndex = 0) => (ushort)GetValue(shortIndex, BitsPerShort, ShortMask);

        public short GetSignedShort(int shortIndex = 0) => (short)GetShort(shortIndex);

        public char GetChar(int shortIndex = 0) => (char)GetShort(shortIndex);

        public uint GetInt(int intIndex = 0) => (uint)GetValue(intIndex, BitsPerInt, IntMask);

        public int GetSignedInt(int intIndex = 0) => (int)GetInt(intIndex);

        public ulong GetLong() => value;

        public long GetSignedLong() => (long)value;


        // Set
        private BitTwiddler SetValue(int itemIndex, int bitsPerItem, ulong itemMask, ulong value)
        {
            int offset = bitsPerItem * itemIndex;

            itemMask <<= offset;

            ulong newValue = this.value & ~itemMask;

            newValue |= ((ulong)value << offset);

            return new BitTwiddler(newValue, this.bitCount);
        }

        public BitTwiddler SetRegion(ulong value, int bitsPerItem, int itemIndex = 0)
        {
            unchecked
            {
                ulong itemMask;

                if (bitsPerItem >= MaxBitCount)
                    itemMask = ulong.MaxValue;
                else
                    itemMask = (BitMask << bitsPerItem) - 1;

                return SetValue(itemIndex, bitsPerItem, itemMask, value);
            }
        }

        public BitTwiddler SetBit(bool value, int bitIndex = 0)
        {
            ulong mask = BitMask << bitIndex;

            ulong newValue;

            if (value)
                newValue = this.value | mask;
            else
                newValue = this.value & ~mask;

            return new BitTwiddler(newValue, this.bitCount);
        }

        public BitTwiddler SetNibble(Nibble value, int nibbleIndex = 0) => SetValue(nibbleIndex, BitsPerNibble, NibbleMask, value);

        public BitTwiddler SetByte(byte value, int byteIndex = 0) => SetValue(byteIndex, BitsPerByte, ByteMask, value);

        public BitTwiddler SetSignedByte(sbyte value, int byteIndex = 0) => SetByte((byte)value, byteIndex);

        public BitTwiddler SetShort(ushort value, int shortIndex = 0) => SetValue(shortIndex, BitsPerShort, ShortMask, value);

        public BitTwiddler SetSignedShort(short value, int shortIndex = 0) => SetShort((ushort)value, shortIndex);

        public BitTwiddler SetChar(char value, int charIndex = 0) => SetShort((ushort)value, charIndex);

        public BitTwiddler SetInt(uint value, int intIndex = 0) => SetValue(intIndex, BitsPerInt, IntMask, value);

        public BitTwiddler SetSignedInt(int value, int intIndex = 0) => SetInt((uint)value, intIndex);

        public BitTwiddler SetLong(ulong value) => new BitTwiddler(value, this.bitCount);

        public BitTwiddler SetSignedLong(long value) => SetLong((ulong)value);

        #endregion


        /* Operations */
        #region Operations


        // Reverse
        private BitTwiddler NaiveReverse(int itemCount, int bitsPerItem, ulong itemMask)
        {
            ulong newValue = 0;

            for (int i = 0; i < itemCount; i++)
            {
                ulong value = (this.value >> bitsPerItem * i) & itemMask;

                value <<= (bitsPerItem * (itemCount - i - 1));

                newValue |= value;
            }

            return new BitTwiddler(newValue, this.bitCount);
        }

        public BitTwiddler ReverseBits() => NaiveReverse(BitCount, 1, BitMask);

        public BitTwiddler ReverseNibbles() => NaiveReverse(NibbleCount, BitsPerNibble, NibbleMask);

        public BitTwiddler ReverseBytes() => NaiveReverse(ByteCount, BitsPerByte, ByteMask);

        public BitTwiddler ReverseShorts() => NaiveReverse(ShortCount, BitsPerShort, ShortMask);

        public BitTwiddler ReverseInts() => NaiveReverse(IntCount, BitsPerInt, IntMask);


        // Nibble
        public BitTwiddler ReverseBitsInNibbles()
        {
            const ulong mask0 = 0x1111_1111_1111_1111;
            const ulong mask1 = 0x2222_2222_2222_2222;
            const ulong mask2 = 0x4444_4444_4444_4444;
            const ulong mask3 = 0x8888_8888_8888_8888;

            ulong value0 = this.value & mask0;
            ulong value1 = this.value & mask1;
            ulong value2 = this.value & mask2;
            ulong value3 = this.value & mask3;

            ulong newValue = (value0 << 3)
                            | (value1 << 1)
                            | (value2 >> 1)
                            | (value3 >> 3);

            return new BitTwiddler(newValue, this.bitCount);
        }

        // Byte
        public BitTwiddler ReverseBitsInBytes()
        {
            return this.ReverseBitsInNibbles().ReverseNibblesInBytes();
        }
        public BitTwiddler ReverseNibblesInBytes()
        {
            const ulong mask0 = 0x0F0F_0F0F_0F0F_0F0F;
            const ulong mask1 = 0xF0F0_F0F0_F0F0_F0F0;

            ulong value0 = this.value & mask0;
            ulong value1 = this.value & mask1;

            ulong newValue = (value0 << BitsPerNibble) | (value1 >> BitsPerNibble);

            return new BitTwiddler(newValue, this.bitCount);
        }


        // Short
        public BitTwiddler ReverseBitsInShorts()
        {
            return this.ReverseBitsInNibbles().ReverseNibblesInShorts();
        }
        public BitTwiddler ReverseNibblesInShorts()
        {
            const ulong mask0 = 0x000F_000F_000F_000F;
            const ulong mask1 = 0x00F0_00F0_00F0_00F0;
            const ulong mask2 = 0x0F00_0F00_0F00_0F00;
            const ulong mask3 = 0xF000_F000_F000_F000;

            ulong value0 = this.value & mask0;
            ulong value1 = this.value & mask1;
            ulong value2 = this.value & mask2;
            ulong value3 = this.value & mask3;

            ulong newValue = (value0 << NibbleOffset3)
                            | (value1 << NibbleOffset1)
                            | (value2 >> NibbleOffset1)
                            | (value3 >> NibbleOffset3);

            return new BitTwiddler(newValue, this.bitCount);
        }
        public BitTwiddler ReverseBytesInShorts()
        {
            const ulong mask0 = 0x00FF_00FF_00FF_00FF;
            const ulong mask1 = 0xFF00_FF00_FF00_FF00;

            ulong value0 = this.value & mask0;
            ulong value1 = this.value & mask1;

            ulong newValue = (value0 << BitsPerByte) | (value1 >> BitsPerByte);

            return new BitTwiddler(newValue, this.bitCount);
        }

        // Int
        public BitTwiddler ReverseBitsInInts()
        {
            return this.ReverseBitsInBytes().ReverseBytesInInts();
        }
        public BitTwiddler ReverseNibblesInInts()
        {
            return this.ReverseNibblesInBytes().ReverseBytesInInts();
        }
        public BitTwiddler ReverseBytesInInts()
        {
            const ulong mask0 = 0x0000_00FF_0000_00FF;
            const ulong mask1 = 0x0000_FF00_0000_FF00;
            const ulong mask2 = 0x00FF_0000_00FF_0000;
            const ulong mask3 = 0xFF00_0000_FF00_0000;

            ulong value0 = this.value & mask0;
            ulong value1 = this.value & mask1;
            ulong value2 = this.value & mask2;
            ulong value3 = this.value & mask3;

            ulong newValue = (value0 << ByteOffset3)
                            | (value1 << ByteOffset1)
                            | (value2 >> ByteOffset1)
                            | (value3 >> ByteOffset3);

            return new BitTwiddler(newValue, this.bitCount);
        }
        public BitTwiddler ReverseShortsInInts()
        {
            const ulong mask0 = 0x0000_FFFF_0000_FFFF;
            const ulong mask1 = 0xFFFF_0000_FFFF_0000;

            ulong value0 = this.value & mask0;
            ulong value1 = this.value & mask1;

            ulong newValue = (value0 << BitsPerShort) | (value1 >> BitsPerShort);

            return new BitTwiddler(newValue, this.bitCount);
        }

        // Long
        public BitTwiddler ReverseBitsInLong()
        {
            return NaiveReverse(MaxBitCount, 1, BitMask);
        }
        public BitTwiddler ReverseNibblesInLong()
        {
            return NaiveReverse(MaxBitCount, BitsPerNibble, NibbleMask);
        }
        public BitTwiddler ReverseBytesInLong()
        {
            return NaiveReverse(MaxBitCount, BitsPerByte, ByteMask);
        }
        public BitTwiddler ReverseShortsInLong()
        {
            const ulong mask0 = 0x0000_0000_0000_FFFF;
            const ulong mask1 = 0x0000_0000_FFFF_0000;
            const ulong mask2 = 0x0000_FFFF_0000_0000;
            const ulong mask3 = 0xFFFF_0000_0000_0000;

            ulong value0 = this.value & mask0;
            ulong value1 = this.value & mask1;
            ulong value2 = this.value & mask2;
            ulong value3 = this.value & mask3;

            ulong newValue = (value0 << ShortOffset3)
                            | (value1 << ShortOffset1)
                            | (value2 >> ShortOffset1)
                            | (value3 >> ShortOffset3);

            return new BitTwiddler(newValue, this.bitCount);
        }
        public BitTwiddler ReverseIntsInLong()
        {
            const ulong mask0 = 0x0000_0000_FFFF_FFFF;
            const ulong mask1 = 0xFFFF_FFFF_0000_0000;

            ulong value0 = this.value & mask0;
            ulong value1 = this.value & mask1;

            ulong newValue = (value0 << BitsPerInt) | (value1 >> BitsPerInt);

            return new BitTwiddler(newValue, this.bitCount);
        }

        // Invert
        public BitTwiddler Invert()
        {
            ulong newValue = ~this.value;

            return new BitTwiddler(newValue, this.bitCount);
        }

        public static BitTwiddler operator ~(BitTwiddler value) => value.Invert();


        // Logical
        public BitTwiddler And(BitTwiddler value) => this & value;

        public static BitTwiddler operator &(BitTwiddler left, BitTwiddler right)
        {
            ulong result = left.value & right.value;

            int length = Math.Max(left.bitCount, right.bitCount);

            return new BitTwiddler(result, length);
        }

        public BitTwiddler Or(BitTwiddler value) => this | value;

        public static BitTwiddler operator |(BitTwiddler left, BitTwiddler right)
        {
            ulong result = left.value | right.value;

            int length = Math.Max(left.bitCount, right.bitCount);

            return new BitTwiddler(result, length);
        }

        public BitTwiddler Xor(BitTwiddler value) => this ^ value;

        public static BitTwiddler operator ^(BitTwiddler left, BitTwiddler right)
        {
            ulong result = left.value ^ right.value;

            int length = Math.Max(left.bitCount, right.bitCount);

            return new BitTwiddler(result, length);
        }

        // Shift
        public BitTwiddler ShiftLeft(int amount) => this >> amount;

        public static BitTwiddler operator >>(BitTwiddler value, int amount)
        {
            if (amount >= MaxBitCount)
                return new BitTwiddler(0, value.bitCount);
            else if (amount < 0)
                return value << -amount;
            else
                return new BitTwiddler(value.value >> amount, value.bitCount);
        }

        public BitTwiddler ShiftRight(int amount) => this << amount;

        public static BitTwiddler operator <<(BitTwiddler value, int amount)
        {
            if (amount >= MaxBitCount)
                return new BitTwiddler(0, value.bitCount);
            else if (amount < 0)
                return value >> -amount;
            else
                return new BitTwiddler(value.value << amount, value.bitCount);
        }


        public BitTwiddler Increment() => this++;

        public static BitTwiddler operator ++(BitTwiddler value)
        {
            unchecked
            {
                ulong newValue = value.value + 1;

                return new BitTwiddler(newValue, value.bitCount);
            }
        }

        public BitTwiddler Decrement() => this--;

        public static BitTwiddler operator --(BitTwiddler value)
        {
            unchecked
            {
                ulong newValue = value.value - 1;

                return new BitTwiddler(newValue, value.bitCount);
            }
        }

        public BitTwiddler Add(BitTwiddler value) => this + value;

        public static BitTwiddler operator +(BitTwiddler left, BitTwiddler right)
        {
            unchecked
            {
                ulong newValue = left.value + right.value;

                int newBitCount = Math.Max(left.bitCount, right.bitCount);

                return new BitTwiddler(newValue, newBitCount);
            }
        }

        public BitTwiddler Subtract(BitTwiddler value) => this - value;

        public static BitTwiddler operator -(BitTwiddler left, BitTwiddler right)
        {
            unchecked
            {
                ulong newValue = left.value - right.value;

                int newBitCount = Math.Max(left.bitCount, right.bitCount);

                return new BitTwiddler(newValue, newBitCount);
            }
        }

        public BitTwiddler MultiplyBy(BitTwiddler value) => this * value;

        public static BitTwiddler operator *(BitTwiddler left, BitTwiddler right)
        {
            unchecked
            {
                ulong newValue = left.value * right.value;

                int newBitCount = Math.Max(left.bitCount, right.bitCount);

                return new BitTwiddler(newValue, newBitCount);
            }
        }

        public BitTwiddler DivideBy(BitTwiddler value) => this / value;

        public static BitTwiddler operator /(BitTwiddler left, BitTwiddler right)
        {
            unchecked
            {
                ulong newValue = left.value / right.value;

                int newBitCount = Math.Max(left.bitCount, right.bitCount);

                return new BitTwiddler(newValue, newBitCount);
            }
        }

        public BitTwiddler Modulus(BitTwiddler value) => this % value;

        public static BitTwiddler operator %(BitTwiddler left, BitTwiddler right)
        {
            unchecked
            {
                ulong newValue = left.value % right.value;

                int newBitCount = Math.Max(left.bitCount, right.bitCount);

                return new BitTwiddler(newValue, newBitCount);
            }
        }

        // TODO: Signed arithmetic/Sign extension for arbitrarty bitregions
        #endregion


        // ToString()
        public string ToBitString(int bitsPerRegion = 4, string regionSeparator = "-", int bitCount = -1)
        {
            if (bitCount == -1)
                bitCount = this.bitCount;

            if (bitsPerRegion <= 0)
                bitsPerRegion = 64;

            StringBuilder builder = new StringBuilder();

            int bitsPassed = bitCount % bitsPerRegion;

            bool initialized = false;

            for (int i = bitCount - 1; i >= 0; i--)
            {
                if (bitsPassed == 0)
                {
                    if (initialized)
                        builder.Append(regionSeparator);

                    bitsPassed = bitsPerRegion - 1;
                }
                else
                {
                    bitsPassed--;
                }

                initialized = true;


                builder.Append(GetBit(i) ? '1' : '0');
            }

            return builder.ToString();
        }


        static readonly string[] nibbleStrings = { "0000", "0001", "0010", "0011",
                                                   "0100", "0101", "0110", "0111",
                                                   "1000", "1001", "1010", "1011",
                                                   "1100", "1101", "1110", "1111"};

        public string ToBinaryString(string nibbleSeparator = "-", string byteSeparator = "_")
        {
            StringBuilder builder = new StringBuilder();

            bool initialized = false;

            for (int i = NibbleCount - 1; i >= 0; i--)
            {
                if (initialized)
                    builder.Append(i % 2 == 1 ? byteSeparator : nibbleSeparator);
                else
                    initialized = true;

                builder.Append(nibbleStrings[GetNibble(i)]);
            }

            return builder.ToString();
        }


        public string ToHexString(string byteSeparator = "-")
        {
            StringBuilder builder = new StringBuilder();

            bool initialized = false;

            for (int i = ByteCount - 1; i >= 0; i--)
            {
                if (initialized)
                    builder.Append(byteSeparator);
                else
                    initialized = true;

                builder.Append(string.Format("{0:X2}", GetByte(i)));
            }

            return builder.ToString();
        }

        public override string ToString() => hex ? ToHexString() : ToBinaryString();

        private static bool hex = true;

        public static void DefaultAsHexString() => hex = true;

        public static bool DefaultAsBinaryString() => hex = false;
    }
}