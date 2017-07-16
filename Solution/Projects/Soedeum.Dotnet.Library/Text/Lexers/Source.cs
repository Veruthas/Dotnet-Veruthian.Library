using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Text.Lexers
{
    public class Source : IDisposable
    {
        public string Name { get; }

        public IEnumerator<CodePoint> Data { get; }


        public Source(string name, IEnumerator<CodePoint> data)
        {
            this.Name = name;
            this.Data = data;
        }

        public void Dispose() => Data?.Dispose();
    }
}