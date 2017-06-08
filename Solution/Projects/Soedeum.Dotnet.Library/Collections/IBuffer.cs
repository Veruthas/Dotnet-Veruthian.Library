namespace Soedeum.Dotnet.Library.Collections
{
    public interface IBuffer<in TIn, out TOut, in TStart>
    {
        bool IsBuffering { get; }

        void Start(TStart start = default(TStart));

        void Add(TIn value);

        void Release();

        void Release(int amount);

        TOut Extract();
    }
}