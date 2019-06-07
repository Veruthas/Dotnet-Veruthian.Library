namespace Veruthian.Library.Utility.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNull(this object value) => value == null;

        public static bool IsNotNull(this object value) => value != null;
    }
}