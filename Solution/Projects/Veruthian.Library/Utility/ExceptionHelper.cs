using System;

namespace Veruthian.Library.Utility
{
    public static class ExceptionHelper
    {
        public static void VerifyPositive(int value, string argumentName = null)
            => VerifyAtLeast(value, 0, argumentName);

        public static void VerifyNegative(int value, string argumentName = null)
            => VerifyAtMost(value, -1, argumentName);

        public static void VerifyAtLeast(int value, int atLeast, string argumentName = null)
        {
            if (value < atLeast)
                throw new ArgumentOutOfRangeException(argumentName, $"{argumentName ?? "value"} must at least {atLeast}");
        }

        public static void VerifyAtMost(int value, int atMost, string argumentName = null)
        {
            if (value > atMost)
                throw new ArgumentOutOfRangeException(argumentName, $"{argumentName ?? "value"} must at most {atMost}");
        }

        public static void VerifyInBetween(int value, int start, int end, string argumentName = null)
        {
            if (value <= start || value >= end)
                throw new ArgumentOutOfRangeException(argumentName, $"{argumentName ?? "value"} must be between {start} and {end}");
        }

        public static void VerifyBetween(int value, int start, int end, string argumentName = null)
        {
            if (value < start || value > end)
                throw new ArgumentOutOfRangeException(argumentName, $"{argumentName ?? "value"} must be between {start} and {end}");
        }

        public static void VerifyInBounds(int value, int length, int start, int end, string indexName = null, string lengthName = null)
        {
            if (value < start || value >= end)
                throw new ArgumentOutOfRangeException(indexName);

            if (length < 0 || value + length > end)
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