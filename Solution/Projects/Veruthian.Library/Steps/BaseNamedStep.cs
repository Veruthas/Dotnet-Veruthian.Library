namespace Veruthian.Library.Steps
{
    public abstract class BaseNamedStep : BaseNestedStep
    {
        protected BaseNamedStep(string name) => this.Name = name;

        public string Name { get; }
    }
}