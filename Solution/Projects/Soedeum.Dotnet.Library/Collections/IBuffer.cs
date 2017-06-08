namespace Soedeum.Dotnet.Library.Collections
{
    public interface IBuffer<in TIn, out TOut>
    {
        bool IsBuffering { get; }

        void Capture();

        void Add(TIn value);

        void Release();

        void Release(int amount);

        TOut Extract();
    }
}