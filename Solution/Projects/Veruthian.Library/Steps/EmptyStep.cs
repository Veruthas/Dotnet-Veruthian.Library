namespace Veruthian.Library.Steps
{
    public class EmptyStep : BaseStep
    {
        public EmptyStep(string type = null, string name = null)
        {
            Type = type;

            Name = name;
        }


        public override string Type { get; }

        public override string Name { get; }
    }
}