namespace Veruthian.Library.Steps
{
    public class NestedStep : BaseStep
    {
        public NestedStep(string type, IStep step)
        {
            this.Type = type;

            this.Down = step;
        }

        public NestedStep(string type, string name, IStep step)
        {
            this.Type = type;

            this.Name = name;

            this.Down = step;
        }


        public override string Type { get; }

        public override string Name { get; }

        public IStep Down { get; }


        protected override IStep GetDown() =>  Down;
    }
}