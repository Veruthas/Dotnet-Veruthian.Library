namespace Veruthian.Dotnet.Library.Text.Runes
{
    public class RuneException : System.Exception
    {
        public RuneException() { }

        public RuneException(string message) : base(message) { }

        public RuneException(string message, System.Exception inner) : base(message, inner) { }
    }
}