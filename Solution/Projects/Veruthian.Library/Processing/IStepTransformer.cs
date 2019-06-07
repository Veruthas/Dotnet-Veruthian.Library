using System;

namespace Veruthian.Library.Processing
{
    public interface IStepTransformer<TInput, TOutput>
    {
        (bool Complete, TOutput Result) Process(TInput data);
    }
}