namespace Veruthian.Library.Patterns
{
    public interface IStep
    {
         IStep Shunt { get; }

         IStep Left { get;  }

         IStep Right { get; }
    }
}