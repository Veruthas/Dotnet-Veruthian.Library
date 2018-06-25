using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public class SpeculativeReader<T> : SpeculativeReaderBase<T>
    {
        public SpeculativeReader(IEnumerator<T> enumerator, GenerateEndItem<T> generateEndItem = null)
        {
            SetData(enumerator, generateEndItem);
        }
    }
}