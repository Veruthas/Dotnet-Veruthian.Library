namespace Veruthian.Library.Numeric
{
    public interface IShiftable<T, N>
        where T : IShiftable<T, N>
    {
        T ShiftLeft(T value, N amount);

        T ShiftRight(T value, N amount);

        T RotateRight(T value, N amount);

        T RotateLeft(T value, N amount);
    }
}