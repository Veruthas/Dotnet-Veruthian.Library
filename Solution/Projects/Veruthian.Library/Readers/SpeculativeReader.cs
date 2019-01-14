using System.Collections.Generic;

namespace Veruthian.Library.Readers
{
    public class SpeculativeReader<T> : BaseSpeculativeReader<T>
    {
        public SpeculativeReader(IEnumerator<T> enumerator, GenerateEndItem<T> generateEndItem = null)
        {
            SetData(enumerator, generateEndItem);
        }
    }
}