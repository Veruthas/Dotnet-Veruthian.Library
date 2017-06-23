using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Text
{
    public class CharSequence
    {
        State start;

        State last;

        State end;


        private class State
        {
            public CharSet Edge { get; set; }

            public State Next { get; set; }

            public State Otherwise { get; set; }
        }


        private CharSequence(CharSet edge)
        {
        }

        public void ToOptional()
        {
            start.Otherwise = end;
        }

        public void ToRepetition()
        {
        }

        public void Append(CharSet edge)
        {

        }

        public void Append(CharSequence sequence)
        {

        }

        public void Or(CharSequence sequence)
        {

        }
    }
}