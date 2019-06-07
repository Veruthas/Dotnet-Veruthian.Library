using System.Collections.Generic;
using System.IO;

namespace Veruthian.Library.Processing.Extensions
{
    public static class Streams
    {
        public static IEnumerable<byte> GetBytes(this Stream stream)
        {
            var item = stream.ReadByte();

            while (item != -1)
            {
                yield return (byte)item;
            }
        }

        public static IEnumerable<byte> GetBytes(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return GetBytes(stream);
            }
        }
    }
}