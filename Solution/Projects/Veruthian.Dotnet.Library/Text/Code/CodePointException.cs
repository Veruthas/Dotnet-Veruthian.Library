namespace Veruthian.Dotnet.Library.Text.Code
{
    public class CodePointException : System.Exception
    {
        public CodePointException() { }

        public CodePointException(string message) : base(message) { }

        public CodePointException(string message, System.Exception inner) : base(message, inner) { }
    }
}