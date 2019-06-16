namespace Veruthian.Library.Steps
{
    public class EmptyStep : BaseStep
    {
        public EmptyStep(string name = null) => Name = name;


        public override string Type => "Empty";

        public override string Name { get; }
    }
}