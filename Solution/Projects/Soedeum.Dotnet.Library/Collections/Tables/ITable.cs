namespace Soedeum.Dotnet.Library.Collections
{
    public interface ITable<TKey, TValue>
    {
        bool Define(TKey key, TValue value, bool canOverwrite = true);

        bool IsDefined(TKey key);


        TValue Resolve(TKey key, TValue defaultValue = default(TValue));

        bool TryResolve(TKey key, out TValue result, TValue defaultValue = default(TValue));

        bool TryResolve<TCast>(TKey key, out TCast result, TCast defaultValue = default(TCast))
            where TCast : TValue;
    }
}