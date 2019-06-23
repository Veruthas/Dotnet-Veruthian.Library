namespace Veruthian.Library.Steps
{
    public abstract class BaseLinkStep : BaseNestedStep
    {
        public IStep Shunt { get; set; }    

        public IStep Next { get; set; }


        protected override IStep GetShunt() => Shunt;

        protected override IStep GetNext() => Next;
    }
}