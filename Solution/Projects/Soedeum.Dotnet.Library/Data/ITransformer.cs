using System;

namespace Soedeum.Dotnet.Library.Data
{
    public interface ITransformer<TSource, TTarget>
    {
        TTarget Process(TSource value);
    }

    public static class Transformer
    {
        public static bool TryProcess<TSource, TTarget>(this ITransformer<TSource, TTarget> transformer, TSource value, out TTarget result)
        {
            try
            {
                result = transformer.Process(value);

                return true;
            }
            catch
            {
                result = default(TTarget);

                return false;
            }
        }

        public static bool TryProcess<TSource, TTarget>(this ITransformer<TSource, TTarget> transformer, TSource value, out TTarget result, out string error)
        {
            try
            {
                result = transformer.Process(value);

                error = null;

                return true;
            }
            catch (Exception ex)
            {
                result = default(TTarget);

                error = ex.Message;
                
                return false;
            }
        }
    }
}