namespace Veruthian.Library.Patterns
{
    public class Step : IStep
    {

        public Step() { }

        public Step(string name) => this.Name = name;

        public Step(string name, object data) { this.Name = name; this.Data = data; }


        public string Name { get; }

        public object Data { get; }

        public IStep Shunt { get; set; }

        public IStep Down { get; set; }

        public IStep Next { get; set; }


        public override string ToString() => Name;
    }
}