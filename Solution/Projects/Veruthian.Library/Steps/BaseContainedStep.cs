namespace Veruthian.Library.Steps
{
    public abstract class BaseContainedStep : BaseStep
    {
        public abstract IStep Step { get; set; }
    }
}