using System.Collections.Generic;
using System.Text;
using Soedeum.Dotnet.Library.Collections;

namespace Soedeum.Dotnet.Library.Text
{
    public class CodePatternBuilder
    {
        List<State> states = new List<State>();

        State first;

        State last;


        public CodePatternBuilder() { }
        public CodePatternBuilder(CodeSet set) => Append(set);
        public CodePatternBuilder(IEnumerable<char> value) => Append(value);
        public CodePatternBuilder(IEnumerable<CodePoint> value) => Append(value);
        public CodePatternBuilder(CodePatternBuilder pattern) => Append(pattern);


        public CodePatternBuilder Append(CodeSet set)
        {
            if (first == null)
            {
                this.first = GetNewState();

                this.last = GetNewState();

                first.AddTransition(set, last);
            }
            else
            {
                var newLast = GetNewState();

                this.last.AddTransition(set, newLast);

                this.last = newLast;
            }

            return this;
        }

        public CodePatternBuilder Append(IEnumerable<CodePoint> value)
        {
            foreach (var codepoint in value)
                Append(CodeSet.Value(codepoint));

            return this;
        }

        public CodePatternBuilder Append(IEnumerable<char> value)
        {
            foreach (var codepoint in value.ToCodePoints().GetEnumerableAdapter())
                Append(CodeSet.Value(codepoint));

            return this;
        }

        public CodePatternBuilder Append(CodePatternBuilder pattern)
        {            
            int offset = states.Count;

            AddPatternStates(pattern, out var addedFirst, out var addedLast);

            if (first == null)
            {
                first = addedFirst;

                last = addedLast;
            }
            else
            {
                this.last.AddEmptyTransition(addedFirst);

                this.last = addedLast;
            }

            return this;
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

            var first = newPattern.first = newPattern.GetNewState();

            var last = newPattern.last = newPattern.GetNewState();

            foreach (var pattern in patterns)
            {
                newPattern.AddPatternStates(pattern, out var addedFirst, out var addedLast);

                first.AddEmptyTransition(addedFirst);

                addedLast.AddEmptyTransition(last);
            }

            return newPattern;;
        }


        private State GetNewState()
        {
            var state = new State(states.Count);

            states.Add(state);

            return state;
        }

        private void AddPatternStates(CodePatternBuilder pattern, out State first, out State last)
        {
            int offset = states.Count;

            foreach (var state in pattern.states)            
                states.Add(state.Clone(offset));

            first = states[pattern.first.Index + offset];

            last = states[pattern.last.Index + offset];
        }


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("First: ").Append(first.Index).AppendLine();
            builder.Append("Last: ").Append(last.Index).AppendLine();

            foreach (var state in states)
                builder.AppendLine(state.ToString());

            return builder.ToString();
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
                var clone = new State(this.Index + offset);

                if (Transitions != null)
                {
                    var clonedTransitions = clone.Transitions = new List<Transition>(Transitions.Count);

                    for (int i = 0; i < Transitions.Count; i++)
                        clonedTransitions.Add(Transitions[i].Offset(offset));
                }

                if (EmptyTransitions != null)
                {
                    var clonedEmpty = clone.EmptyTransitions = new List<int>(EmptyTransitions.Count);

                    for (int i = 0; i < EmptyTransitions.Count; i++)
                        clonedEmpty.Add(EmptyTransitions[i] + offset);
                }

                return clone;
            }


            public override string ToString()
            {
                var builder = new StringBuilder();

                builder.Append('{').Append(Index).Append(": ");

                bool final = true;

                if (Transitions != null)
                {
                    final = false;

                    bool started = false;

                    foreach (var transition in Transitions)
                    {
                        if (started)
                            builder.Append(" | ");
                        else
                            started = true;

                        builder.Append(transition.Range).Append(" -> ").Append(transition.StateIndex);
                    }
                }

                if (EmptyTransitions != null)
                {
                    if (!final)
                        builder.Append(" | ");
                    else
                        final = false;

                    bool started = false;

                    builder.Append("<empty> -> ");

                    foreach (var emptyTransition in EmptyTransitions)
                    {
                        if (started)
                            builder.Append(", ");
                        else
                            started = true;

                        builder.Append(emptyTransition);
                    }
                }

                if (final)
                    builder.Append("<none>");

                builder.Append("}");

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
    }
}