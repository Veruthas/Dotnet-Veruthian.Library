using System;

namespace Veruthian.Dotnet.Library.Text.Encodings
{
    public class EncodingException : Exception
    {
        public EncodingException() { }

        public EncodingException(string message) : base(message) { }

        public EncodingException(string message, System.Exception inner) : base(message, inner) { }
    }
}