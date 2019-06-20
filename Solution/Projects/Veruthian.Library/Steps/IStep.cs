namespace Veruthian.Library.Steps
{
    public interface IStep
    {
        string Type { get; }

        IStep Shunt { get; }

        IStep Step { get; }

        IStep Next { get; }
    }
}