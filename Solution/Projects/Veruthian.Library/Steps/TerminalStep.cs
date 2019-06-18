namespace Veruthian.Library.Steps
{
    public class TerminalStep : BaseStep
    {
        public TerminalStep(string type = null, string name = null)
        {
            Type = type;

            Name = name;
        }


        public override string Type { get; }

        public override string Name { get; }
    }
}