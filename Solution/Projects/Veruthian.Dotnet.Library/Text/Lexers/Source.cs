using System;
using System.Collections.Generic;
using Veruthian.Dotnet.Library.Text.Code;

namespace Veruthian.Dotnet.Library.Text.Lexers
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