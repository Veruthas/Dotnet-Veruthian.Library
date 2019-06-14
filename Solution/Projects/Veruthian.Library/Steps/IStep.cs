namespace Veruthian.Library.Steps
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