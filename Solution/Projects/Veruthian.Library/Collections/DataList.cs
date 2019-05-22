using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public class DataList<T> : BaseResizableVector<T, DataList<T>>
    {
        protected override DataList<T> This => this;
    }
}