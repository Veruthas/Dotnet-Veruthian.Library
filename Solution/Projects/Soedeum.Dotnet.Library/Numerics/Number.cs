namespace Soedeum.Dotnet.Library.Numerics
{
    public struct Number
    {
        ulong value;

        int length;

        public int ByteCount => length;

        public byte GetByte(int index)
        {

        }

        public int ShortCount => (double)length / 2;
        public short GetShort(int index)
        {

        }

        public int IntCount => length  / 4;

        public int GetInt(int index)
        {

        }
    }
}