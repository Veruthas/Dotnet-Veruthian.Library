using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Soedeum.Dotnet.Library.Data.Patterns
{
    public class PatternBuilder<T> : IEnumerable<State<T>>
    {
        List<State<T>> states = new List<State<T>>();

        State<T> start;

        State<T> end;

        // Constructors
        public PatternBuilder() { }
        public PatternBuilder(T value) => Append(value);
        public PatternBuilder(params T[] values) => Append((IEnumerable<T>)values);
        public PatternBuilder(IEnumerable<T> values) => Append(values);
        public PatternBuilder(PatternBuilder<T> pattern) => Append(pattern);


        public State<T> this[int index] => states[index];

        public int Count => states.Count;


        public int StartIndex => start.Index;

        public int EndIndex => end.Index;


        // Concatenation
        public PatternBuilder<T> Append(T value)
        {
            State<T> oldEnd, newEnd;

            if (start == null)
            {
                this.start = oldEnd = GetNewState();

                this.end = newEnd = GetNewState();
            }
            else
            {
                oldEnd = this.end;

                this.end = newEnd = GetNewState();
            }

            oldEnd.AddTransition(value, newEnd);

            return this;
        }

        public PatternBuilder<T> Append(params T[] values) => Append((IEnumerable<T>)values);

        public PatternBuilder<T> Append(IEnumerable<T> values)
        {
            State<T> oldEnd, newEnd;

            if (start == null)
            {
                this.start = oldEnd = GetNewState();

                this.end = newEnd = GetNewState();
            }
            else
            {
                oldEnd = this.end;

                this.end = newEnd = GetNewState();
            }

            oldEnd.AddTransitions(values, newEnd);

            return this;
        }


        public PatternBuilder<T> Append(PatternBuilder<T> pattern)
        {
            AddPatternStates(pattern, out var addedStart, out var addedEnd);

            if (start == null)
            {
                start = addedStart;

                end = addedEnd;
            }
            else
            {
                this.end.AddEmptyTransition(addedStart);

                this.end = addedEnd;
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
            this.start.AddEmptyTransition(this.end);

            return this;
        }

        public PatternBuilder<T> MakeRepeating()
        {
            this.end.AddEmptyTransition(this.start);

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

            newPattern.start = newPattern.GetNewState();

            newPattern.end = newPattern.GetNewState();

            return newPattern;
        }

        private static void JoinPattern(PatternBuilder<T> pattern, PatternBuilder<T> join)
        {
            pattern.AddPatternStates(join, out var addedFirst, out var addedLast);

            pattern.start.AddEmptyTransition(addedFirst);

            addedLast.AddEmptyTransition(pattern.end);
        }

        // State creation
        private State<T> GetNewState()
        {
            var state = new State<T>(states.Count);

            states.Add(state);

            return state;
        }

        private void AddPatternStates(PatternBuilder<T> pattern, out State<T> start, out State<T> end)
        {
            int offset = states.Count;

            foreach (var state in pattern.states)
                states.Add(state.Clone(offset));

            start = states[pattern.start.Index + offset];

            end = states[pattern.end.Index + offset];
        }


        // ToString
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("First: ").Append(start.Index).AppendLine();
            builder.Append("Last: ").Append(end.Index).AppendLine();

            foreach (var state in states)
                builder.AppendLine(state.ToString());

            return builder.ToString();
        }

        public IEnumerator<State<T>> GetEnumerator() => states.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}