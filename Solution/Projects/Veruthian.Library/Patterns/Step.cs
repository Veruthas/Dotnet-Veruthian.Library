namespace Veruthian.Library.Patterns
{
    public class Step : IStep
    {
        public IStep Shunt { get; set; }

        public IStep Down { get; set; }

        public IStep Next { get; set; }

        public override string ToString() => $"step<{GetHashCode()}>";
    }
}