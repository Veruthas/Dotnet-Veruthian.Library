using System;
using System.Text;

namespace Soedeum.Dotnet.Library.Text
{
    public class TextCapturer
    {
        bool isCapturing;

        TextPosition start;

        StringBuilder builder = new StringBuilder();


        public bool IsCapturing => isCapturing;


        public void Capture(TextPosition start = default(TextPosition))
        {
            // if (isCapturing)
            //     throw new InvalidOperationException("Capture already in progress.");

            Release();
            this.isCapturing = true;
            this.start = start;
        }


        public void Give(char value)
        {
            if (isCapturing)
                builder.Append(value);
        }

        public TextSpan Extract()
        {
            // Should I throw an error, or just return a default TextSpan?
            // if (!isCapturing)
            //     throw new InvalidOperationException("Cannot extract when not capturing.");


            return new TextSpan(start, builder.ToString());

        }


        public void Release()
        {
            builder.Clear();
            start = default(TextPosition);
        }

        public void Release(int amount)
        {
            if (amount > builder.Length)
                Release();
            else
                builder.Remove(builder.Length - amount, amount);
        }

        public void Release(int fromPosition, int toPosition)
        {
            Release(fromPosition - toPosition);
        }

        public void Release<S>(int fromPosition, int toPosition, S state)
        {
            Release(fromPosition, toPosition);
        }
    }
}
