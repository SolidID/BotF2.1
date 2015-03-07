using System;
using System.CodeDom;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brush = System.Drawing.Brush;
using Brushes = System.Drawing.Brushes;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;
using Pen = System.Drawing.Pen;
using Point = System.Windows.Point;

namespace TerrainGenerator
{
    internal static class Generator
    {
        public static Image GenerateDiamondSquare(int size, float rough, int seed)
        {
            if (rough > 1 || rough < 0)
                throw new ArgumentException("rough is out of bound. Must b e between 0 and 1.");

            var rand = new Random(seed + DateTime.Now.Millisecond);
            float initialHeight = (float)rand.NextDouble();
            float[,] grid = new float[size, size];

            //starting edges and first step
            grid[0, 0] = initialHeight;
            grid[0, size - 1] = initialHeight;
            grid[size - 1, 0] = initialHeight;
            grid[size - 1, size - 1] = initialHeight;

            var a = new Point(0, 0);
            var b = new Point(size - 1, 0);
            var c = new Point(size - 1, size - 1);
            var d = new Point(0, size - 1);

            HandleSquareRecursive(ref grid, a, b, c, d, rough, rand);
            float max;
            float min = max = 0f;
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    float value = grid[x, y];

                    if (value < 0)
                    {
                        value = (float)-Math.Sqrt(Math.Abs(value));
                        grid[x, y] = value;
                    }

                    if (value < min)
                        min = value;

                    if (value > max)
                        max = value;

                }
            }


            return CreateBitmapFromArray(grid, min, max);
        }

        private static void HandleSquareRecursive(ref float[,] grid, Point a, Point b, Point c, Point d, float rough, Random rand)
        {
            //some valuues
            int xHalf = (int)((a.X + b.X) / 2);
            int yHalf = (int)((a.Y + d.Y) / 2);

            if (xHalf == (int)a.X || yHalf == (int)a.Y)
                return;

            // generating new points
            var top = new Point(xHalf, a.Y);
            var right = new Point(b.X, yHalf);
            var bottom = new Point(xHalf, c.Y);
            var left = new Point(a.X, yHalf);

            var mid = new Point(xHalf, yHalf);
            Point upperMid;
            Point rightMid;
            Point bottomMid;
            Point leftMid;

            upperMid = a.Y - yHalf > 0 ? new Point(xHalf, a.Y - yHalf) : a;
            rightMid = b.X + xHalf < grid.GetLength(0) ? new Point(b.X + xHalf, yHalf) : b;
            bottomMid = c.Y + yHalf < grid.GetLength(1) ? new Point(xHalf, c.Y + yHalf) : c;
            leftMid = a.X - xHalf > 0 ? new Point(a.X - xHalf, yHalf) : d;

            // setting & calculating new hights of the point;
            grid[(int)mid.X, (int)mid.Y] = (grid[(int)a.X, (int)a.Y]
                                     + grid[(int)b.X, (int)b.Y]
                                     + grid[(int)c.X, (int)c.Y]
                                     + grid[(int)d.X, (int)d.Y])
                                    / 4 + rough * (2f * (float)rand.NextDouble() - 1);

            grid[(int)top.X, (int)top.Y] = (grid[(int)a.X, (int)a.Y]
                                              + grid[(int)b.X, (int)b.Y]
                                              + grid[(int)mid.X, (int)mid.Y]
                                              + grid[(int)upperMid.X, (int)upperMid.Y]) / 4
                                              + rough * (2f * (float)rand.NextDouble() - 1);

            grid[(int)right.X, (int)right.Y] = (grid[(int)b.X, (int)b.Y]
                                              + grid[(int)c.X, (int)c.Y]
                                              + grid[(int)mid.X, (int)mid.Y]
                                              + grid[(int)rightMid.X, (int)rightMid.Y]) / 4
                                              + rough * (2f * (float)rand.NextDouble() - 1);

            grid[(int)bottom.X, (int)bottom.Y] = (grid[(int)c.X, (int)c.Y]
                                              + grid[(int)d.X, (int)d.Y]
                                              + grid[(int)mid.X, (int)mid.Y]
                                              + grid[(int)bottomMid.X, (int)bottomMid.Y]) / 4
                                              + rough * (2f * (float)rand.NextDouble() - 1);

            grid[(int)left.X, (int)left.Y] = (grid[(int)d.X, (int)d.Y]
                                              + grid[(int)a.X, (int)a.Y]
                                              + grid[(int)mid.X, (int)mid.Y]
                                              + grid[(int)leftMid.X, (int)leftMid.Y]) / 4
                                              + rough * (2f * (float)rand.NextDouble() - 1);


            var newRough = rough / 2;
            HandleSquareRecursive(ref grid, a, top, mid, left, newRough, rand);
            HandleSquareRecursive(ref grid, top, b, right, mid, newRough, rand);
            HandleSquareRecursive(ref grid, mid, right, c, bottom, newRough, rand);
            HandleSquareRecursive(ref grid, left, mid, bottom, d, newRough, rand);
        }

        private static void ExternalCode(int size, float rough, int seed)
        {
            var rand = new Random(seed + DateTime.Now.Millisecond);
            int depth = size - 1;
            float initialHeight = (float)rand.NextDouble();
            float[,] grid = new float[size, size];

            //starting edges
            grid[0, 0] = initialHeight;
            grid[0, size - 1] = initialHeight;
            grid[size - 1, 0] = initialHeight;
            grid[size - 1, size - 1] = initialHeight;

            int iteration;
            bool putX, putZ;
            while (depth > -1)
            {
                iteration = (1 << depth);
                putX = false;
                for (int i = 0; i < size; i += iteration)
                {
                    putZ = false;
                    for (int j = 0; j < size; j += iteration)
                    {
                        if (putX && putZ)
                        {
                            //put diamond
                            grid[i, j] = (grid[i - iteration, j - iteration]
                                        + grid[i + iteration, j - iteration]
                                        + grid[i - iteration, j + iteration]
                                        + grid[i + iteration, j + iteration]
                                        ) / 4 + rough * (float)rand.NextDouble();
                        }
                        if (putX != putZ)
                        {
                            // put squares
                            if (putX)
                            {
                                grid[i, j] = (grid[i - iteration, j]
                                              + grid[i + iteration, j]
                                    ) / 2 + rough * (float)rand.NextDouble();
                            }
                            else
                            {
                                grid[i, j] = (grid[i, j - iteration]
                                              + grid[i, j + iteration]
                                    ) / 2 + rough * (float)rand.NextDouble();
                            }
                        }
                        putZ = !putZ;
                    }
                    putX = !putX;
                }
                rough /= 2;
                depth--;
            }
        }

        private static Bitmap CreateBitmapFromArray(float[,] grid, float min, float max)
        {
            int width = grid.GetLength(0);
            int height = grid.GetLength(1);
            var bmp = new Bitmap(width, height);
            // some correction necessary to have no negative values
            float correction = (max - min);

            using (var gfx = Graphics.FromImage(bmp))
            {
                gfx.FillRectangle(Brushes.Black, 0, 0, width, height);
                var myPen = new Pen(Color.White, 1f);
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        var colorIndex = (int)((grid[x, y] / correction).Clamp(0, 1).Lerp(0, 255));
                        myPen.Color = Color.FromArgb(colorIndex, colorIndex, colorIndex);
                        gfx.DrawLine(myPen, x, y, x + 1, y + 1);
                        //Brush bru = new SolidBrush(Color.FromArgb(colorIndex, colorIndex, colorIndex));
                        //gfx.FillRectangle(bru, x, y, 1, 1);
                    }
                }
            }
            return bmp;
        }


        public static Bitmap GetDS(uint size)
        {
            var rand = new Random(1337 + DateTime.Now.Millisecond);
            float initialHeight = (float)rand.NextDouble();
            float[,] grid = new float[size, size];

            //starting edges and first step
            grid[0, 0] = initialHeight;
            grid[0, size - 1] = initialHeight;
            grid[size - 1, 0] = initialHeight;
            grid[size - 1, size - 1] = initialHeight;
            DiamondSquare(0, 0, size, size, 5, size / 4, ref grid, rand);
            return CreateBitmapFromArray(grid, 0, 1);
        }
        private static void DiamondSquare(uint x1, uint y1, uint x2, uint y2, float range, uint level, ref float[,] grid, Random rnd)
        {
            if (rnd == null)
                rnd = new Random(1337 + DateTime.Now.Millisecond);

            if (level < 1) return;

            // diamonds
            for (uint i = x1 + level; i < x2; i += level)
                for (uint j = y1 + level; j < y2; j += level)
                {
                    float a = grid[i - level, j - level];
                    float b = grid[i, j - level];
                    float c = grid[i - level, j];
                    float d = grid[i, j];
                    grid[i - level / 2, j - level / 2] = (a + b + c + d) / 4 + (float)rnd.NextDouble() * range;
                }

            // squares
            for (uint i = x1 + 2 * level; i < x2; i += level)
                for (uint j = y1 + 2 * level; j < y2; j += level)
                {
                    float a = grid[i - level, j - level];
                    float b = grid[i, j - level];
                    float c = grid[i - level, j];
                    float d = grid[i, j];
                    float e = grid[i - level / 2, j - level / 2];

                    grid[i - level, j - level / 2] = (a + c + e + grid[i - 3 * level / 2, j - level / 2]) / 4 + (float)rnd.NextDouble() * range;
                    grid[i - level / 2, j - level] = (a + b + e + grid[i - level / 2, j - 3 * level / 2]) / 4 + (float)rnd.NextDouble() * range;
                }

            DiamondSquare(x1, y1, x2, y2, range / 2, level / 2, ref grid, rnd);
        }
    }
}