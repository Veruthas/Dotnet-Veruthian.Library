namespace Veruthian.Library.Steps
{
    public interface IStep
    {
        IStep Shunt { get; }

        IStep Step { get; }

        IStep Next { get; }
    }
}