using System;

namespace Soedeum.Dotnet.Library
{
    public interface ITransformer<TFrom, TTo>
    {
        TTo Result { get; }

        bool Process(TFrom value);
    }    
}