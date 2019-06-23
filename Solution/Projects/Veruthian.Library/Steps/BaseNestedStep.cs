namespace Veruthian.Library.Steps
{
    public abstract class BaseNestedStep : BaseStep
    {
        public IStep Down { get; set; }

        protected override IStep GetDown() => Down;
    }
}