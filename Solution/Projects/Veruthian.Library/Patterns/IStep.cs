namespace Veruthian.Library.Patterns
{
    public interface IStep
    {
        string Type { get; }

        string Name { get; }
        
        IStep Shunt { get; }

        IStep Down { get; }

        IStep Next { get; }
    }
}