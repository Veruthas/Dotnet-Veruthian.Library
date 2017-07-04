using System;
using System.Collections;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Numerics
{
    public struct ByteEnumerable : IEnumerable<byte>
    {
        ulong bytes;

        int length;

        private ByteEnumerable(ulong bytes, int length)
        {
            this.bytes = bytes;
            
            this.length = length;            
        }


        private const int ByteOffset0 = 0;
        private const int ByteOffset1 = 8;
        private const int ByteOffset2 = 16;
        private const int ByteOffset3 = 24;
        private const int ByteOffset4 = 32;
        private const int ByteOffset5 = 40;
        private const int ByteOffset6 = 48;
        private const int ByteOffset7 = 56;


        // Bytes
        public ByteEnumerable(byte value)
        {
            length = 1;

            bytes = value;
        }
        public ByteEnumerable(byte value0, byte value1)
        {
            length = 2;

            bytes = ((ulong)value0)
                | ((ulong)value1 << ByteOffset1);
        }
        public ByteEnumerable(byte value0, byte value1, byte value2)
        {
            length = 3;

            bytes = ((ulong)value0)
                | ((ulong)value1 << ByteOffset1)
                | ((ulong)value2 << ByteOffset2);
        }
        public ByteEnumerable(byte value0, byte value1, byte value2, byte value3)
        {
            length = 4;

            bytes = ((ulong)value0)
                | ((ulong)value1 << ByteOffset1)
                | ((ulong)value2 << ByteOffset2)
                | ((ulong)value3 << ByteOffset3);
        }
        public ByteEnumerable(byte value0, byte value1, byte value2, byte value3, byte value4)
        {
            length = 5;

            bytes = ((ulong)value0)
                | ((ulong)value1 << ByteOffset1)
                | ((ulong)value2 << ByteOffset2)
                | ((ulong)value3 << ByteOffset3)
                | ((ulong)value4 << ByteOffset4);
        }
        public ByteEnumerable(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5)
        {
            length = 6;

            bytes = ((ulong)value0)
                | ((ulong)value1 << ByteOffset1)
                | ((ulong)value2 << ByteOffset2)
                | ((ulong)value3 << ByteOffset3)
                | ((ulong)value4 << ByteOffset4)
                | ((ulong)value5 << ByteOffset5);
        }
        public ByteEnumerable(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5, byte value6)
        {
            length = 7;

            bytes = ((ulong)value0)
                | ((ulong)value1 << ByteOffset1)
                | ((ulong)value2 << ByteOffset2)
                | ((ulong)value3 << ByteOffset3)
                | ((ulong)value4 << ByteOffset4)
                | ((ulong)value5 << ByteOffset5)
                | ((ulong)value6 << ByteOffset6);
        }
        public ByteEnumerable(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5, byte value6, byte value7)
        {
            length = 8;

            bytes = ((ulong)value0)
                | ((ulong)value1 << ByteOffset1)
                | ((ulong)value2 << ByteOffset2)
                | ((ulong)value3 << ByteOffset3)
                | ((ulong)value4 << ByteOffset4)
                | ((ulong)value5 << ByteOffset5)
                | ((ulong)value6 << ByteOffset6)
                | ((ulong)value7 << ByteOffset7);
        }
        public ByteEnumerable(int length, byte value0 = 0, byte value1 = 0, byte value2 = 0, byte value3 = 0, byte value4 = 0, byte value5 = 0, byte value6 = 0, byte value7 = 0)
        {
            this.length = length;

            bytes = ((ulong)value0)
                | ((ulong)value1 << ByteOffset1)
                | ((ulong)value2 << ByteOffset2)
                | ((ulong)value3 << ByteOffset3)
                | ((ulong)value4 << ByteOffset4)
                | ((ulong)value5 << ByteOffset5)
                | ((ulong)value6 << ByteOffset6)
                | ((ulong)value7 << ByteOffset7);
        }

        public ByteEnumerable(uint value)
        {

        }

        public ByteEnumerable(uint value)
        {

        }

        public ByteEnumerable(long value, ByteOrder order)
            : this((ulong)value, order) { }

        public ByteEnumerable(ulong value, ByteOrder order)
        {
            byte value0, value1, value2, value3, value4, value5, value6, value7;

            if (order == ByteOrder.LittleEndian)
            {
                value0 = (byte)((value >> ByteOffset0) & 0xFF);
                value1 = (byte)((value >> ByteOffset1) & 0xFF);
                value2 = (byte)((value >> ByteOffset2) & 0xFF);
                value3 = (byte)((value >> ByteOffset3) & 0xFF);
                value4 = (byte)((value >> ByteOffset4) & 0xFF);
                value5 = (byte)((value >> ByteOffset5) & 0xFF);
                value6 = (byte)((value >> ByteOffset6) & 0xFF);
                value7 = (byte)((value >> ByteOffset7) & 0xFF);
            }
            else
            {
                value0 = (byte)((value >> ByteOffset7) & 0xFF);
                value1 = (byte)((value >> ByteOffset6) & 0xFF);
                value2 = (byte)((value >> ByteOffset5) & 0xFF);
                value3 = (byte)((value >> ByteOffset4) & 0xFF);
                value4 = (byte)((value >> ByteOffset3) & 0xFF);
                value5 = (byte)((value >> ByteOffset2) & 0xFF);
                value6 = (byte)((value >> ByteOffset1) & 0xFF);
                value7 = (byte)((value >> ByteOffset0) & 0xFF);
            }

            this = new ByteEnumerable(value0, value1, value2, value3, value4, value5, value6, value7);
        }

        public IEnumerator<byte> GetEnumerator()
        {
            ulong bytes = this.bytes;

            for (int i = 0; i < length; i++)
            {
                byte value = (byte)(bytes & 0xFF);

                bytes >>= 8;

                yield return value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
