using System;

namespace Soedeum.Dotnet.Library
{
    public interface ITransformer<TSource, TTarget>
    {        
        bool TryProcess(TSource value, out TTarget result);
    }    
}