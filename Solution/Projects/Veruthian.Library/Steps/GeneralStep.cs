namespace Veruthian.Library.Steps
{
    public class GeneralStep : LinkStep
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