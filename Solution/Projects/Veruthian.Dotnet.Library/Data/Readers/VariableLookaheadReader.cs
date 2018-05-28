using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Readers
{
    public class VariableLookaheadReader<T> : VariableLookaheadReaderBase<T>
    {
        public VariableLookaheadReader(IEnumerator<T> enumerator, GenerateEndItem<T> generateEndItem = null)
        {
            SetData(enumerator, generateEndItem);
        }
    }
}