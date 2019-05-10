using System;
using Veruthian.Library.Collections;
using Veruthian.Library.Collections.Extensions;

namespace Veruthian.Library.Steps
{
    public class MatchSetStep<T> : MatchSimpleStep<T>
    {
        IContainer<T> set;

        Func<IContainer<T>, string> toString;

        public MatchSetStep(IContainer<T> set, Func<IContainer<T>, string> toString = null)
        {
            Utility.ExceptionHelper.VerifyNotNull(set, nameof(set));

            this.set = set;

            this.toString = toString ?? ((s) => s.ToString());
        }

        public override string Description => $"match-set<{toString(set)}>";

        protected override bool Match(T value) => set.Contains(value);
    }
}