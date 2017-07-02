using System;
using System.Collections;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Numerics
{
    public struct ByteEnumerable : IEnumerable<byte>
    {
        byte value;

        public ByteEnumerable(byte value) => this.value = value;
        
        public IEnumerator<byte> GetEnumerator()
        {
            yield return value;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

public struct ShortEnumerable : IEnumerable<byte>
    {
        const int maxBytes = 2;

        uint value;

        bool littleEndian;

        int bytes;

        public ShortEnumerable(uint value, int bytes = maxBytes, bool littleEndian = true)
        {
            this.value = value;

            this.bytes = Math.Max(maxBytes, bytes);

            this.littleEndian = littleEndian;
        }

        public IEnumerator<byte> GetEnumerator()
        {
            uint mask = 0xFF;

            uint value = this.value;

            if (littleEndian)
            {
                for (int i = 0; i < bytes; i++)
                {
                    value >>= (8 * i);

                    var result = (byte)(value & mask);

                    yield return result;
                }
            }
            else
            {
                for (int i = bytes - 1; i >= 0; i--)
                {
                    value >>= (8 * i);

                    var result = (byte)(value & mask);

                    yield return result;
                }
            }

        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public struct IntEnumerable : IEnumerable<byte>
    {
        const int maxBytes = 4;
        
        uint value;

        bool littleEndian;

        int bytes;

        public IntEnumerable(uint value, int bytes = maxBytes, bool littleEndian = true)
        {
            this.value = value;

            this.bytes = Math.Max(maxBytes, bytes);

            this.littleEndian = littleEndian;
        }

        public IEnumerator<byte> GetEnumerator()
        {
            uint mask = 0xFF;

            uint value = this.value;

            if (littleEndian)
            {
                for (int i = 0; i < bytes; i++)
                {
                    value >>= (8 * i);

                    var result = (byte)(value & mask);

                    yield return result;
                }
            }
            else
            {
                for (int i = bytes - 1; i >= 0; i--)
                {
                    value >>= (8 * i);

                    var result = (byte)(value & mask);

                    yield return result;
                }
            }

        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public struct LongEnumerable : IEnumerable<byte>
    {
        const int maxBytes = 8;

        uint value;

        bool littleEndian;

        int bytes;

        public LongEnumerable(uint value, int bytes = maxBytes, bool littleEndian = true)
        {
            this.value = value;

            this.bytes = Math.Max(maxBytes, bytes);

            this.littleEndian = littleEndian;
        }

        public IEnumerator<byte> GetEnumerator()
        {
            uint mask = 0xFF;

            uint value = this.value;

            if (littleEndian)
            {
                for (int i = 0; i < bytes; i++)
                {
                    value >>= (8 * i);

                    var result = (byte)(value & mask);

                    yield return result;
                }
            }
            else
            {
                for (int i = bytes - 1; i >= 0; i--)
                {
                    value >>= (8 * i);

                    var result = (byte)(value & mask);

                    yield return result;
                }
            }

        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
