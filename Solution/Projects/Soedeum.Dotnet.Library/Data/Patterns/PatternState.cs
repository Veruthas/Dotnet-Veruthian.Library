using System.Collections.Generic;
using System.Text;
using Soedeum.Dotnet.Library.Data.Enumeration;

namespace Soedeum.Dotnet.Library.Data.Patterns
{
    public class PatternState<T>
    {
        private int index;

        private List<PatternTransition<T>> transitions;

        private List<int> emptyTransitions;

        public PatternState(int index)
        {
            this.transitions = null;
            this.emptyTransitions = null;
            this.index = index;
        }


        public int Index => index;


        public int TransitionCount => transitions == null ? 0 : transitions.Count;

        public PatternTransition<T> GetTransition(int index) => transitions[index];


        public int EmptyTransitionCount => emptyTransitions == null ? 0 : EmptyTransitionCount;

        public int GetEmptyTransition(int index) => emptyTransitions[index];


        public IEnumerable<PatternTransition<T>> Transitions
        {
            get
            {
                if (transitions == null)
                    return EmptyEnumerable<PatternTransition<T>>.Default;
                else
                    return transitions.AsReadOnly();
            }
        }

        public IEnumerable<int> EmptyTransitions
        {
            get
            {
                if (emptyTransitions == null)
                    return EmptyEnumerable<int>.Default;
                else
                    return emptyTransitions.AsReadOnly();
            }
        }

        public void AddTransition(T on, PatternState<T> to)
        {
            if (transitions == null)
                transitions = new List<PatternTransition<T>>();

            var transition = new PatternTransition<T>(on, to.index);

            transitions.Add(transition);
        }

        public void AddTransitions(IEnumerable<T> on, PatternState<T> to)
        {
            if (transitions == null)
                transitions = new List<PatternTransition<T>>();

            foreach (var onItem in on)
            {
                var transition = new PatternTransition<T>(onItem, to.index);

                transitions.Add(transition);
            }
        }

        public void AddEmptyTransition(PatternState<T> to)
        {
            if (emptyTransitions == null)
                emptyTransitions = new List<int>();

            emptyTransitions.Add(to.index);

            emptyTransitions.Sort();
        }

        public PatternState<T> Clone(int offset)
        {
            var clone = new PatternState<T>(this.index + offset);

            if (transitions != null)
            {
                var clonedTransitions = clone.transitions = new List<PatternTransition<T>>(transitions.Count);

                for (int i = 0; i < transitions.Count; i++)
                    clonedTransitions.Add(transitions[i].Offset(offset));
            }

            if (emptyTransitions != null)
            {
                var clonedEmpty = clone.emptyTransitions = new List<int>(emptyTransitions.Count);

                for (int i = 0; i < emptyTransitions.Count; i++)
                    clonedEmpty.Add(emptyTransitions[i] + offset);
            }

            return clone;
        }


        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append('{').Append(index).Append(": ");

            bool final = true;

            if (transitions != null)
            {
                final = false;

                bool started = false;

                foreach (var transition in transitions)
                {
                    if (started)
                        builder.Append("; ");
                    else
                        started = true;

                    builder.Append('{').Append(transition.On.ToString()).Append("} -> ").Append(transition.ToIndex);
                }
            }

            if (emptyTransitions != null)
            {
                if (!final)
                    builder.Append("; ");
                else
                    final = false;

                bool started = false;

                builder.Append("<empty> -> ");

                foreach (var emptyTransition in emptyTransitions)
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
}