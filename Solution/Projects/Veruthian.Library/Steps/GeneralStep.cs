namespace Veruthian.Library.Steps
{
    public class GeneralStep : LinkStep
    {
        public GeneralStep() { }
        

        public GeneralStep(string type) => this.Type = type;


        public override string Type { get; }
    }
}