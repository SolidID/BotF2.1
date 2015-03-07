using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerrainGenerator
{
    public static class CppDiamondSquare
    {

        public static Bitmap Create(DiamondSquareInitType startType, double roughness = 1.0, int seed = 1337)
        {
            const double d = 0.5;
            const int size = GES_SIZE * GES_SIZE;

            var rnd = new Random(seed);
            var map = new double[size];
            var initialValue = rnd.NextDouble();
            switch (startType)
            {
                case DiamondSquareInitType.Even:
                    for (int i = 0; i < size; i++)
                        map[i] = initialValue;
                    break;
                case DiamondSquareInitType.Random:
                    for (int i = 0; i < size; i++)
                        map[i] = rnd.NextDouble();
                    break;
                case DiamondSquareInitType.EvenEdges:
                    map[0] = initialValue;
                    map[GES_SIZE] = initialValue;
                    map[GES_SIZE * GES_SIZE - 1] = initialValue;
                    map[GES_SIZE * GES_SIZE - 1 - GES_SIZE] = initialValue;
                    break;
                case DiamondSquareInitType.RandomEdges:
                    map[0] = rnd.NextDouble();
                    map[GES_SIZE] = rnd.NextDouble();
                    map[GES_SIZE * GES_SIZE - 1] = rnd.NextDouble();
                    map[GES_SIZE * GES_SIZE - 1 - GES_SIZE] = rnd.NextDouble();
                    break;
            }

            MidPointDisplacement(ref map, d, GES_SIZE, GES_SIZE, rnd, roughness);

            return CreateBitmap(ref map);

        }

        private const int BORDER_SIZE = 1024;
        private const int GES_SIZE = BORDER_SIZE + 1;

        private static void DiamondStep(ref double[] map, double d, int x, int y, int width, int height, Random rnd)
        {
            if (width <= 1 || height <= 1)
                return;

            // A ------ B
            // |        |
            // | middle |
            // |        |
            // C ------ D
            int A = x + y * GES_SIZE;
            int B = x + width + y * GES_SIZE;
            int C = x + (y + height) * GES_SIZE;
            int D = x + width + (y + height) * GES_SIZE;

            //diamond step
            //calculate index for middlepoint   
            int middle = x + width / 2 + (y + height / 2) * GES_SIZE;

            //calculate height for diamond point
            map[middle] = (map[A] + map[B] + map[C] + map[D]) / 4.0 + rnd.NextDouble().Lerp(-d, d);
            //if (map[middle] > 1.0) map[middle] = 1.0;
        }

        private static void SquareStep(ref double[] map, double d, int x, int y, int width, int height, Random rnd)
        {
            if (width <= 1 || height <= 1)
                return;

            //             ETop  (y)
            //               |    
            //      (x) A -- G -- B   
            //          |    |    |
            // ELeft -- F -- E -- H -- ERight
            //          |    |    |
            //          C -- I -- D
            //               |    
            //            EBottom 
            //                    
            int A = x + y * GES_SIZE;
            int B = x + width + y * GES_SIZE;
            int C = x + (y + height) * GES_SIZE;
            int D = x + width + (y + height) * GES_SIZE;

            int E = x + width / 2 + (y + height / 2) * GES_SIZE;
            int F = x + (y + height / 2) * GES_SIZE;
            int G = x + width / 2 + y * GES_SIZE;
            int H = x + width + (y + height / 2) * GES_SIZE;
            int I = x + width / 2 + (y + height) * GES_SIZE;

            int eLeft = E - width;
            if (eLeft < 0) eLeft = 0;
            int eRight = E + width;
            if (eRight >= GES_SIZE * GES_SIZE) eRight = 0;
            int eTop = E - (height * GES_SIZE);
            if (eTop < 0) eTop = 0;
            int eBottom = E + (height * GES_SIZE);
            if (eBottom >= GES_SIZE * GES_SIZE) eBottom = 0;

            if (eLeft == 0) map[F] = (map[A] + map[C] + map[E]) / 3.0 + rnd.NextDouble().Lerp(-d, d);
            else map[F] = (map[A] + map[C] + map[E] + map[eLeft]) / 4.0 + rnd.NextDouble().Lerp(-d, d);

            if (eTop == 0) map[G] = (map[A] + map[B] + map[E]) / 3.0 + rnd.NextDouble().Lerp(-d, d);
            else map[G] = (map[A] + map[B] + map[E] + map[eTop]) / 4.0 + rnd.NextDouble().Lerp(-d, d);

            if (eRight == 0) map[H] = (map[B] + map[D] + map[E]) / 3.0 + rnd.NextDouble().Lerp(-d, d);
            else map[H] = (map[B] + map[D] + map[E] + map[eRight]) / 4.0 + rnd.NextDouble().Lerp(-d, d);

            if (eBottom == 0) map[I] = (map[C] + map[D] + map[E]) / 3.0 + rnd.NextDouble().Lerp(-d, d);
            else map[I] = (map[C] + map[D] + map[E] + map[eBottom]) / 4.0 + rnd.NextDouble().Lerp(-d, d);
        }

        private static void MidPointDisplacement(ref double[] map, double d, int width, int height, Random rnd, double roughness)
        {
            if (width <= 1 || height <= 1)
                return;

            int halfWidth = width;
            int halfHeight = height;

            do
            {
                d *= Math.Pow(2, -roughness);
                halfWidth /= 2;
                halfHeight /= 2;
                if (halfHeight < 2 || halfWidth < 2) break;

                for (int j = 0; j < GES_SIZE - 1; j += halfHeight)
                    for (int i = 0; i < GES_SIZE - 1; i += halfWidth)
                        DiamondStep(ref map, d, i, j, halfWidth, halfHeight, rnd);
                for (int j = 0; j < GES_SIZE - 1; j += halfHeight)
                    for (int i = 0; i < GES_SIZE - 1; i += halfWidth)
                        SquareStep(ref map, d, i, j, halfWidth, halfHeight, rnd);
            } while (true);
        }

        private static Bitmap CreateBitmap(ref double[] map)
        {
            var bmp = new Bitmap(GES_SIZE, GES_SIZE);
            double min = 0, max = 0;

            foreach (double h in map)
            {
                if (h < min)
                    min = h;
                if (h > max)
                    max = h;
            }

            double range = max - min;

            using (var gfx = Graphics.FromImage(bmp))
            {
                var reusableBrush = new SolidBrush(Color.White);
                // filling the background
                gfx.FillRectangle(Brushes.Black, 0, 0, GES_SIZE, GES_SIZE);
                for (int y = 0; y < GES_SIZE; y++)
                    for (int x = 0; x < GES_SIZE; x++)
                    {
                        int pixelIndex = y * GES_SIZE + x;
                        int colorIndex = (int)((map[pixelIndex] + min) / range).Lerp(0, 255).Clamp(0, 255);
                        reusableBrush.Color = Color.FromArgb(colorIndex, colorIndex, colorIndex);
                        gfx.FillRectangle(reusableBrush, x, y, 1, 1);
                    }
            }
            return bmp;
        }
    }
}
