using System.Collections.Generic;

namespace Veruthian.Library.Readers
{
    public class RecollectiveReader<T, K, D> : BaseRecollectiveReader<T, K, D>
    {
        public RecollectiveReader(IEnumerator<T> enumerator, GenerateEndItem<T> generateEndItem = null)
        {
            SetData(enumerator, generateEndItem);
        }
    }

    public class RecollectiveReader<T, K> : BaseRecollectiveReader<T, K, object>
    {
        public RecollectiveReader(IEnumerator<T> enumerator, GenerateEndItem<T> generateEndItem = null)
        {
            SetData(enumerator, generateEndItem);
        }
    }
}