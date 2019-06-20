namespace Veruthian.Library.Steps
{
    public class NestedStep : BaseNestedStep
    {
        public NestedStep(string type, IStep step)
            : base(step)
        {
            this.Type = type;
        }

        public NestedStep(string type, string name, IStep step)
            : base(step)
        {
            this.Type = type;

            this.Name = name;
        }


        public override string Type { get; }

        public override string Name { get; }

        public IStep Step { get; }


        protected override IStep GetDown() => Down;
    }
}