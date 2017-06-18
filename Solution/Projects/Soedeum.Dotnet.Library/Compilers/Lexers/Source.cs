using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    public class Source
    {
        public string Name { get; }

        public IEnumerator<char> Data { get; }


        public Source(string name, IEnumerator<char> data)
        {
            this.Name = name;
            this.Data = data;
        }
    }
}