using System;

namespace Veruthian.Library.Processing
{
    public interface ITransformer<in TSource, out TTarget>
    {
        TTarget Process(TSource value);
    }
}