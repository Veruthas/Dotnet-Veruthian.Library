namespace Veruthian.Library.Patterns
{
    public class GeneralStep : Step
    {
        public GeneralStep() { }

        public GeneralStep(string type = null, string label = null)
        {
            this.Type = type;

            this.Label = label;
        }

        public override string Type { get; }

        public override string Label { get; }


        public IStep Shunt { get; set; }

        public IStep Down { get; set; }

        public IStep Next { get; set; }


        protected override IStep GetShunt() => Shunt;

        protected override IStep GetDown() => Down;

        protected override IStep GetNext() => Next;
    }
}