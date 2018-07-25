using System.Collections.Generic;

namespace Veruthian.Library.Readers
{
    public class VariableLookaheadReader<T> : BaseVariableLookaheadReader<T>
    {
        public VariableLookaheadReader(IEnumerator<T> enumerator, GenerateEndItem<T> generateEndItem = null)
        {
            SetData(enumerator, generateEndItem);
        }
    }
}