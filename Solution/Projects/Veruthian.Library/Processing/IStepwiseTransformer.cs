using System;

namespace Veruthian.Library.Processing
{
    public interface IStepwiseTransformer<TInput, TOutput>
    {
        (bool Complete, TOutput Result) Process(TInput data);
    }
}