using System;
using System.Collections;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Numerics
{
    public struct ByteEnumerable : IEnumerator<byte>
    {
        ulong bytes;

        int length;

        byte current;


        public byte Current => current;

        object IEnumerator.Current => current;

        public void Dispose() { }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset() => throw new NotImplementedException();

    }
}
