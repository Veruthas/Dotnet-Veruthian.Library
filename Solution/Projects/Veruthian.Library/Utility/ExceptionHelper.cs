using System;

namespace Veruthian.Library.Utility
{
    public static class ExceptionHelper
    {
        public static void VerifyPositive(int value, string argument)
        {
            if (value < 0)
                throw new ArgumentException(argument);
        }

        public static void VerifyInBounds(int index, int start, int end, string argument)
        {
            if (index < start || index >= end)
                throw new ArgumentOutOfRangeException(argument);
        }

        public static void VerifyIndexInBounds(int index, int start, int end)
        {
            if (index < start || index >= end)
                throw new IndexOutOfRangeException();
        }

    }
}