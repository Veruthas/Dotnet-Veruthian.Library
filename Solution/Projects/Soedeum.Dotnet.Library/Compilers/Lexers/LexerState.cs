using System.Collections.Generic;
using Soedeum.Dotnet.Library.Text;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    public class LexerState<T>
    {
        private struct State
        {
            public CharRange Range;

            public LexerState<T> Next;
        }

        List<State> states;

        LexerState<T> endState;

        T type;


        private LexerState()
        {
            this.states = new List<State>();
        }

        private LexerState(T type)
        {
            this.type = type;
        }


        public LexerState<T> GetNextState(char value)
        {            
        }


        public bool IsEndState => this.states == null;

        public T Type => this.type;


        public static LexerState<T> EndState(T type)
        {
            return new LexerState<T>(type);
        }
    }
}