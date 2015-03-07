using System;

namespace Assets.Scripts
{
    public static class Globals
    {
        public static float Radius = 4f;
        public static float Height = 2 * Radius;
        public static float RowHeight = 1.5f * Radius;
        public static float HalfWidth = (float)Math.Sqrt((Radius * Radius) - ((Radius / 2f) * (Radius / 2f)));
        public static float Width = 2 * HalfWidth;
        public static float ExtraHeight = Height - RowHeight;
        public static float Edge = RowHeight - ExtraHeight;
    }
}