namespace Veruthian.Library.Steps
{
    public abstract class BaseLinkStep : BaseStep
    {
        public IStep Shunt { get; set; }    

        public IStep Down { get; set; }

        public IStep Next { get; set; }


        protected override IStep GetShunt() => Shunt;

        protected override IStep GetDown() => Down;

        protected override IStep GetNext() => Next;
    }
}