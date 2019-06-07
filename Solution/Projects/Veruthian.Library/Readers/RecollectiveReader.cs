using System.Collections.Generic;

namespace Veruthian.Library.Readers
{
    public class RecollectiveReader<T> : BaseRecollectiveReader<T>
    {
        public RecollectiveReader(IEnumerator<T> enumerator, GenerateEndItem<T> generateEndItem = null)
        {
            SetData(enumerator, generateEndItem);
        }
    }
}