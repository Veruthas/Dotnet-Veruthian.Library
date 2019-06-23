namespace Veruthian.Library.Steps
{
    public interface IStep
    {
        IStep Shunt { get; }

        IStep Down { get; }

        IStep Next { get; }
    }
}