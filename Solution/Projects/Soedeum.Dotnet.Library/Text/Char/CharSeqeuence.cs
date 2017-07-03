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
            public State() { }

            public State(CharSet edge) => this.Edge = edge;



            public CharSet Edge { get; set; }

            public State Next { get; set; }

            public State Otherwise { get; set; }
        }


        private CharSequence()
        {
            start = last = end = null;
        }


        public CharSequence ToOptional()
        {
            start.Otherwise = end;

            return this;
        }

        public CharSequence ToRepetition()
        {
            start.Otherwise = end;

            last.Next = start;

            return this;
        }

        public CharSequence Append(CharSet edge)
        {
            var state = new State(edge);

            last.Next = state;

            state.Next = end;

            return this;
        }

        public CharSequence Append(CharSequence sequence)
        {
            return null;
        }

        public void Or(CharSequence sequence)
        {

        }


        public CharSequence Clone()
        {
            return null;
        }
    }
}