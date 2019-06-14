namespace Veruthian.Library.Patterns
{
    public class GeneralStep : BaseLinkStep
    {
        public GeneralStep() { }

        public GeneralStep(string type = null, string name = null)
        {
            this.Type = type;

            this.Name = name;
        }


        public override string Type { get; }

        public override string Name { get; }
    }
}