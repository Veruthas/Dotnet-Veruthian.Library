using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    public class Source : IDisposable
    {
        public string Name { get; }

        public IEnumerator<char> Data { get; }


        public Source(string name, IEnumerator<char> data)
        {
            this.Name = name;
            this.Data = data;
        }

        public void Dispose() => Data?.Dispose();
    }
}