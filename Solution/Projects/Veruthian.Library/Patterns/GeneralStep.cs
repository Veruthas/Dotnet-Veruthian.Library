namespace Veruthian.Library.Patterns
{
    public class GeneralStep : BaseStep
    {
        public GeneralStep() { }

        public GeneralStep(string type = null, string name = null)
        {
            this.Type = type;

            this.Name = name;
        }


        public override string Type { get; }

        public override string Name { get; }


        public IStep Shunt { get; set; }

        public IStep Down { get; set; }

        public IStep Next { get; set; }


        protected override IStep GetShunt() => Shunt;

        protected override IStep GetDown() => Down;

        protected override IStep GetNext() => Next;
    }
}