using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Operations
{
    public abstract class SimpleOperation<TState> : Operation<TState>
    {
        public override int Count => 0;

        public override IEnumerator<IOperation<TState>> GetEnumerator()
        {
            yield break;
        }

        public override IOperation<TState> GetSubOperation(int index)
        {
            VerifyIndex(0);

            return null;
        }
    }
}