using System;

namespace Veruthian.Library.Utility
{
    public static class ExceptionHelper
    {
        public static void VerifyPositive(int value, string argumentName = null)
        {
            if (value < 0)
                throw new ArgumentException(argumentName);
        }

        public static void VerifyInBounds(int index, int start, int end, string argumentName = null)
        {
            if (index < start || index >= end)
                throw new ArgumentOutOfRangeException(argumentName);
        }

        public static void VerifyInRange(int index, int length, int start, int end, string indexName = null, string lengthName = null)
        {
            if (index < start || index >= end)
                throw new ArgumentOutOfRangeException(indexName);

            if (length < 0 || index + length > end)
                throw new ArgumentOutOfRangeException(lengthName);
        }

        public static void VerifyIndexInBounds(int index, int start, int end)
        {
            if (index < start || index >= end)
                throw new IndexOutOfRangeException();
        }

        public static void VerifyNotNull<S>(S value, string argumentName = null)
        {
            if (value == null)
                throw new ArgumentNullException(argumentName);
        }
    }
}