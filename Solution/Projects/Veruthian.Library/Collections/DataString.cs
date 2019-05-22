using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public class DataString<T> : BaseString<T, DataString<T>>
    {
        protected override DataString<T> This => this;
    }
}