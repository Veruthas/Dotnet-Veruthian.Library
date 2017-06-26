using System;

namespace Soedeum.Dotnet.Library.Text
{
    public interface IByteDecoder
    {
        CodePoint? Result { get; }

        bool Process(byte value);
    }


    public class Utf32Decoder : IByteDecoder
    {
        bool isLittleEndian;

        public Utf32Decoder(bool isLittleEndian = false)
        {
            this.isLittleEndian = isLittleEndian;
        }

        public CodePoint? Result => throw new NotImplementedException();

        public bool Process(byte value)
        {
            throw new NotImplementedException();
        }
    }
}