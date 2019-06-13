namespace Veruthian.Library.Patterns
{
    public class Step : IStep
    {
        public Step() { }

        public Step(string name) => Name = name;


        public string Name { get; }


        public IStep Shunt { get; set; }

        public IStep Down { get; set; }

        public IStep Next { get; set; }



        public override string ToString() => Name ?? "<step>";
    }
}