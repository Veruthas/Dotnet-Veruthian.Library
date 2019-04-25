using System;
using Veruthian.Library.Readers;
using Veruthian.Library.Types;
using Veruthian.Library.Utility;

namespace Veruthian.Library.Operations
{
    public class MatchOperation<T, TState> : BaseSimpleOperation< TState>
        where TState : Has<IReader<T>>
        where T : IEquatable<T>
    {
        T item;

        public MatchOperation(T value)
        {
            ExceptionHelper.VerifyNotNull(value);

            this.item = value;
        }

        public T Item => item;
        
        public override string Description => $"Match({item.ToString()})";

        protected override bool DoAction(TState state, ITracer<TState> tracer = null)
        {
            state.Get(out IReader<T> reader);

            if (!reader.IsEnd)
            {
                var value = reader.Peek();

                if (item.Equals(value))
                {
                    reader.Read();

                    return true;
                }
            }

            return false;
        }
    }
}