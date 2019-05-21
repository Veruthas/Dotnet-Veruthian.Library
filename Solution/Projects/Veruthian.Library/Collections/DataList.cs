using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public class DataList<T> : BaseResizableVector<Number, T, DataList<T>>, IResizableVector<T>
    {
        public override Number Start => Number.Zero;

        protected override int VerifiedAddressToIndex(Number address) => (int)address;

        protected override Number OffsetStartAddress(Number offset) => offset;

        public override bool IsValidAddress(Number address) => address < Count;

    }
}