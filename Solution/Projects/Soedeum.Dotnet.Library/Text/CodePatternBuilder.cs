using System;
using System.Collections.Generic;
using System.Text;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Collections.Enumerators;

namespace Soedeum.Dotnet.Library.Text
{
    public class CodePatternBuilder
    {
        List<State> states = new List<State>();

        State first;

        State last;

        // Constructors
        public CodePatternBuilder() { }
        public CodePatternBuilder(CodePoint value) => Append(value);
        public CodePatternBuilder(IEnumerable<CodePoint> value) => Append(value);
        public CodePatternBuilder(CodeRange range) => Append(range);
        public CodePatternBuilder(IEnumerable<CodeRange> ranges) => Append(ranges);
        public CodePatternBuilder(CodeSet set) => Append(set);
        public CodePatternBuilder(string value) => Append(value);
        public CodePatternBuilder(IEnumerable<char> value) => Append(value);
        public CodePatternBuilder(CodePatternBuilder pattern) => Append(pattern);



        // Concatenation
        public CodePatternBuilder Append(CodePoint value)
        {
            return Append(new CodeRange(value));
        }

        public CodePatternBuilder Append(IEnumerable<CodePoint> value)
        {
            foreach (var codepoint in value)
                Append(CodeSet.Value(codepoint));

            return this;
        }

        public CodePatternBuilder Append(CodeRange range)
        {
            if (first == null)
            {
                this.first = GetNewState();

                this.last = GetNewState();

                first.AddTransition(range, last);
            }
            else
            {
                var newLast = GetNewState();

                this.last.AddTransition(range, newLast);

                this.last = newLast;
            }

            return this;
        }

        public CodePatternBuilder Append(IEnumerable<CodeRange> ranges)
        {
            foreach (var range in ranges)
                Append(range);

            return this;
        }

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

        public CodePatternBuilder Append(string value)
        {
            foreach (var codepoint in value.ToCodePoints().GetEnumerableAdapter())
                Append(codepoint);

            return this;
        }

        public CodePatternBuilder Append(IEnumerable<char> value)
        {
            foreach (var codepoint in value.ToCodePoints().GetEnumerableAdapter())
                Append(codepoint);

            return this;
        }

        public CodePatternBuilder Append(CodePatternBuilder pattern)
        {
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

        public static CodePatternBuilder operator +(CodePatternBuilder pattern, CodePoint value) => pattern.Append(value);

        public static CodePatternBuilder operator +(CodePatternBuilder pattern, IEnumerable<CodePoint> value) => pattern.Append(value);

        public static CodePatternBuilder operator +(CodePatternBuilder pattern, CodeRange range) => pattern.Append(range);

        public static CodePatternBuilder operator +(CodePatternBuilder pattern, IEnumerable<CodeRange> ranges) => pattern.Append(ranges);

        public static CodePatternBuilder operator +(CodePatternBuilder pattern, CodeSet set) => pattern.Append(set);

        public static CodePatternBuilder operator +(CodePatternBuilder pattern, string value) => pattern.Append(value);

        public static CodePatternBuilder operator +(CodePatternBuilder pattern, IEnumerable<char> value) => pattern.Append(value);

        public static CodePatternBuilder operator +(CodePatternBuilder pattern, CodePatternBuilder value) => pattern.Append(pattern);


        // Repetition
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

        public CodePatternBuilder MakeRepeating(int times)
        {
            var clone = new CodePatternBuilder(this);

            for (int i = 0; i < times - 1; i++)
                Append(clone);

            return this;
        }

        public CodePatternBuilder MakeRepeating(int min, int max)
        {
            var clone = new CodePatternBuilder(this);

            for (int i = 0; i < min - 1; i++)
                Append(clone);

            for (int i = min; i < max; i++)
                Append(new CodePatternBuilder(clone).MakeOptional());

            return this;
        }

        public static CodePatternBuilder operator *(CodePatternBuilder pattern, int times) => pattern.MakeRepeating(times);



        // Union
        public static CodePatternBuilder Union(params CodePatternBuilder[] patterns)
        {
            var newPattern = CreateUnionBase();

            foreach (var pattern in patterns)
                JoinPattern(newPattern, pattern);

            return newPattern;
        }


        public static CodePatternBuilder operator |(CodePatternBuilder left, CodePatternBuilder right) => Union(left, right);

        private static CodePatternBuilder CreateUnionBase()
        {
            var newPattern = new CodePatternBuilder();

            newPattern.first = newPattern.GetNewState();

            newPattern.last = newPattern.GetNewState();

            return newPattern;
        }

        private static void JoinPattern(CodePatternBuilder pattern, CodePatternBuilder join)
        {
            pattern.AddPatternStates(join, out var addedFirst, out var addedLast);

            pattern.first.AddEmptyTransition(addedFirst);

            addedLast.AddEmptyTransition(pattern.last);
        }

        // State creation
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


        // ToString
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("First: ").Append(first.Index).AppendLine();
            builder.Append("Last: ").Append(last.Index).AppendLine();

            foreach (var state in states)
                builder.AppendLine(state.ToString());

            return builder.ToString();
        }


        // Structures
        public class State
        {
            public State(int index)
            {
                this.Transitions = null;
                this.EmptyTransitions = null;
                this.Index = index;
            }

            public int Index { get; private set; }

            public List<Transition> Transitions { get; private set; }

            public List<int> EmptyTransitions { get; private set; }


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

                EmptyTransitions.Sort();
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

        public struct Transition
        {
            public Transition(CodeRange range, int stateIndex)
            {
                this.Range = range;

                this.StateIndex = stateIndex;
            }

            public CodeRange Range { get; private set; }

            public int StateIndex { get; private set; }


            public Transition Offset(int offset)
            {
                return new Transition(Range, StateIndex + offset);
            }
        }
    }
}