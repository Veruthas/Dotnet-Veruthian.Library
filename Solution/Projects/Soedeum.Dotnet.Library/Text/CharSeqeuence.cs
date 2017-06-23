namespace Soedeum.Dotnet.Library.Text
{
    public class CharSequence
    {
        State start;

        State end;


        private class State
        {
            public CharSet Edge { get; set; }

            public State Next { get; set; }

            public State Otherwise { get; set; }
        }


        public CharSequence(CharSet edge)
        {
            this.start = new State();
            start.Edge = edge;

            this.end = new State();
            this.start.Next = end;
        }

        public void ToOptional()
        {
            start.Otherwise = end;
        }

        public void ToRepition()
        {
            start.Otherwise = end;
            start.Next = start;
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