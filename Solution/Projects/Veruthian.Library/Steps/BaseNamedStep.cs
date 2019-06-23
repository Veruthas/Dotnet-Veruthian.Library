namespace Veruthian.Library.Steps
{
    public abstract class BaseNamedStep : BaseNestedStep
    {
        protected BaseNamedStep(string name) => this.Name = name;

        protected BaseNamedStep(string name, IStep step) : base(step) => this.Name = name;


        public string Name { get; }
    }
}