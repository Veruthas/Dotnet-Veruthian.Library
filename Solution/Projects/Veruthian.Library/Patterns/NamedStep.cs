namespace Veruthian.Library.Patterns
{
    public class NamedStep : Step
    {
        public NamedStep(string name) => this.Name = name;

        public string Name { get; private set; }
        

        public override string ToString() => Name;
    }
}