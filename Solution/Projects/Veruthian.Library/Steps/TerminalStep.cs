namespace Veruthian.Library.Steps
{
    public class TerminalStep : BaseStep
    {
        public TerminalStep() { }

        public TerminalStep(string type) => Type = type;

        public override string Type { get; }
    }
}