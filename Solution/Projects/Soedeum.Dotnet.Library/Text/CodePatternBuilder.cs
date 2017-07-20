using System.Collections.Generic;
using System.Text;

namespace Soedeum.Dotnet.Library.Text
{
    public class CodePatternBuilder
    {
        List<State> states = new List<State>();

        State first;

        State last;


        private CodePatternBuilder() { }

        public CodePatternBuilder(CodePatternBuilder pattern)
        {

        }

        public CodePatternBuilder(CodeSet set)
        {
            this.first = GetNewState();

            this.last = GetNewState();

            first.AddTransition(set, last);
        }

        private State GetNewState()
        {
            var state = new State(states.Count);

            states.Add(state);

            return state;
        }

        private class State
        {
            public State(int index)
            {
                this.Index = index;
            }

            public int Index { get; set; }

            public List<Transition> Transitions { get; set; }

            public List<int> EmptyTransitions { get; set; }


            public void AddTransition(CodeRange range, State to)
            {
                if (Transitions == null)
                    Transitions = new List<Transition>();

                var transition = new Transition(range, to.Index);

                Transitions.Add(transition);
            }

            public void AddTransition(CodeSet set, State to)
            {
                if (Transitions == null)
                    Transitions = new List<Transition>();

                foreach (var range in set.Ranges)
                {
                    var transition = new Transition(range, to.Index);

                    Transitions.Add(transition);
                }
            }

            public void AddEmptyTransition(State to)
            {
                if (EmptyTransitions == null)
                    EmptyTransitions = new List<int>();

                EmptyTransitions.Add(to.Index);
            }

            public State Clone(int offset)
            {
                var clone = new State(this.Index);

                if (clone.Transitions != null)
                {
                    var clonedTransitions = clone.Transitions = new List<Transition>(Transitions.Count);

                    for (int i = 0; i < Transitions.Count; i++)
                        clonedTransitions[i] = Transitions[i].Offset(offset);
                }

                if (clone.EmptyTransitions != null)
                {
                    var clonedEmpty = clone.EmptyTransitions = new List<int>(EmptyTransitions.Count);

                    for (int i = 0; i < EmptyTransitions.Count; i++)
                        clonedEmpty[i] = clonedEmpty[i] + offset;
                }

                return clone;
            }


            public override string ToString()
            {
                var builder = new StringBuilder();

                if (Transitions != null)
                {
                    foreach (var transition in Transitions)
                    {
                        builder.Append(transition.Range).Append(" -> ").Append(transition.StateIndex).Append("; ");
                    }
                }

                if (EmptyTransitions != null)
                {
                    builder.Append("Empty: ");

                    foreach (var empty in EmptyTransitions)
                    {
                        builder.Append(empty).Append("; ");
                    }
                }

                return builder.ToString();
            }
        }

        private struct Transition
        {
            public Transition(CodeRange range, int stateIndex)
            {
                this.Range = range;

                this.StateIndex = stateIndex;
            }

            public CodeRange Range { get; set; }

            public int StateIndex { get; set; }


            public Transition Offset(int offset)
            {
                return new Transition(Range, StateIndex = offset);
            }
        }


        public CodePatternBuilder Append(CodeSet set)
        {
            var newLast = GetNewState();

            this.last.AddTransition(set, newLast);

            this.last = newLast;

            return this;
        }

        public CodePatternBuilder Append(CodePatternBuilder pattern)
        {
            return null;
        }

        public CodePatternBuilder MakeOptional()
        {
            this.first.AddEmptyTransition(this.last);

            return this;
        }

        public CodePatternBuilder MakeRepeating()
        {
            this.last.AddEmptyTransition(this.first);

            return this;
        }


        public static CodePatternBuilder Or(params CodePatternBuilder[] patterns)
        {
            var newPattern = new CodePatternBuilder();

            var first = newPattern.GetNewState();
            var last = newPattern.GetNewState();
        }
    }
}