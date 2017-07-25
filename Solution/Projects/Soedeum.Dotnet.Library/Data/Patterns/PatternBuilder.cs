using System.Collections.Generic;
using System.Text;

namespace Soedeum.Dotnet.Library.Data.Patterns
{
    public class PatternBuilder<T>
    {
        List<State> states = new List<State>();

        State first;

        State last;

        // Constructors
        public PatternBuilder() { }
        public PatternBuilder(T value) => Append(value);
        public PatternBuilder(params T[] values) => Append((IEnumerable<T>)values);
        public PatternBuilder(IEnumerable<T> values) => Append(values);
        public PatternBuilder(PatternBuilder<T> pattern) => Append(pattern);



        // Concatenation
        public PatternBuilder<T> Append(T value)
        {
            State oldLast, newLast;

            if (first == null)
            {
                this.first = oldLast = GetNewState();

                this.last = newLast = GetNewState();
            }
            else
            {
                oldLast = this.last;

                this.last = newLast = GetNewState();
            }

            oldLast.AddTransition(value, newLast);

            return this;
        }

        public PatternBuilder<T> Append(params T[] values) => Append((IEnumerable<T>)values);

        public PatternBuilder<T> Append(IEnumerable<T> values)
        {
            State oldLast, newLast;

            if (first == null)
            {
                this.first = oldLast = GetNewState();

                this.last = newLast = GetNewState();
            }
            else
            {
                oldLast = this.last;

                this.last = newLast = GetNewState();
            }

            oldLast.AddTransitions(values, newLast);

            return this;
        }


        public PatternBuilder<T> Append(PatternBuilder<T> pattern)
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

        public PatternBuilder<T> AppendSequence(params T[] values) => AppendSequence((IEnumerable<T>)values);

        public PatternBuilder<T> AppendSequence(IEnumerable<T> values)
        {
            foreach (var value in values)
                Append(value);

            return this;
        }


        public static PatternBuilder<T> operator +(PatternBuilder<T> pattern, T value) => pattern.Append(value);

        public static PatternBuilder<T> operator +(PatternBuilder<T> pattern, IEnumerable<T> values) => pattern.Append(values);

        public static PatternBuilder<T> operator +(PatternBuilder<T> leading, PatternBuilder<T> following) => leading.Append(following);


        // Repetition
        public PatternBuilder<T> MakeOptional()
        {
            this.first.AddEmptyTransition(this.last);

            return this;
        }

        public PatternBuilder<T> MakeRepeating()
        {
            this.last.AddEmptyTransition(this.first);

            return this;
        }

        public PatternBuilder<T> MakeRepeating(int times)
        {
            var clone = new PatternBuilder<T>(this);

            for (int i = 0; i < times - 1; i++)
                Append(clone);

            return this;
        }

        public PatternBuilder<T> MakeRepeating(int min, int max)
        {
            var clone = new PatternBuilder<T>(this);

            for (int i = 0; i < min - 1; i++)
                Append(clone);

            for (int i = min; i < max; i++)
                Append(new PatternBuilder<T>(clone).MakeOptional());

            return this;
        }

        public static PatternBuilder<T> operator *(PatternBuilder<T> pattern, int times) => pattern.MakeRepeating(times);



        // Union
        public static PatternBuilder<T> Union(params PatternBuilder<T>[] patterns)
        {
            var newPattern = CreateUnionBase();

            foreach (var pattern in patterns)
                JoinPattern(newPattern, pattern);

            return newPattern;
        }


        public static PatternBuilder<T> operator |(PatternBuilder<T> left, PatternBuilder<T> right) => Union(left, right);

        private static PatternBuilder<T> CreateUnionBase()
        {
            var newPattern = new PatternBuilder<T>();

            newPattern.first = newPattern.GetNewState();

            newPattern.last = newPattern.GetNewState();

            return newPattern;
        }

        private static void JoinPattern(PatternBuilder<T> pattern, PatternBuilder<T> join)
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

        private void AddPatternStates(PatternBuilder<T> pattern, out State first, out State last)
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


            public void AddTransition(T on, State to)
            {
                if (Transitions == null)
                    Transitions = new List<Transition>();

                var transition = new Transition(on, to.Index);

                Transitions.Add(transition);
            }

            public void AddTransitions(IEnumerable<T> on, State to)
            {
                if (Transitions == null)
                    Transitions = new List<Transition>();

                foreach (var onItem in on)
                {
                    var transition = new Transition(onItem, to.Index);

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
                            builder.Append("; ");
                        else
                            started = true;

                        builder.Append('{').Append(transition.On.ToString()).Append("} -> ").Append(transition.ToIndex);
                    }
                }

                if (EmptyTransitions != null)
                {
                    if (!final)
                        builder.Append("; ");
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
            public Transition(T on, int toIndex)
            {
                this.On = on;

                this.ToIndex = toIndex;
            }

            public T On { get; private set; }

            public int ToIndex { get; private set; }


            public Transition Offset(int offset)
            {
                return new Transition(On, toIndex: +offset);
            }
        }
    }
}