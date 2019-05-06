using Veruthian.Library.Processing;
using Veruthian.Library.Types;

namespace Veruthian.Library.Operations
{
    public interface ISpeculativeBuilder<TState>:IBuilder<TState>
        where TState : Has<ISpeculative>
    {
        //Speculate
        SpeculativeOperation<TState> Speculate(IOperation<TState> operation);
    }
}