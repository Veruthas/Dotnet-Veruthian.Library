namespace Veruthian.Library.Patterns
{
    public interface IStep
    {
         IStep Shunt { get; }

         IStep Down { get;  }

         IStep Next { get; }
    }
}