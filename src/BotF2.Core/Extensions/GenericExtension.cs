using System;

namespace BotF2.Core.Extensions
{
    public static class GenericExtension
    {
        public static T Clamp<T>(this T value, T min, T max)
            where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
                return min;
            if (value.CompareTo(max) > 0)
                return max;

            return value;

        }

        public static float Lerp(this float amount, float min, float max)
        {
            return min + (max - min) * amount;
        }

        public static double Lerp(this double amount, double min, double max)
        {
            return min + (max - min) * amount;
        }

    }
}