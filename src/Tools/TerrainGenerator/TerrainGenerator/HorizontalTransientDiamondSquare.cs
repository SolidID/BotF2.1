using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace TerrainGenerator
{
    public static class HorizontalTransientDiamondSquare
    {
        public class Dto
        {
            public DiamondSquareInitType StartType;
            public double Roughness;
            public int Seed;
            //public int Size;
            public int Width;
            public int Height;
            public Random Rand;
        }

        public static Bitmap Create(Dto startParams)
        {
            ProcessStartParameter(startParams);

            const double d = 0.5;
            int width = startParams.Width;
            int height = startParams.Height;
            int arraySize = width * height;

            var map = new double[arraySize];
            var initialValue = startParams.Rand.NextDouble();
            switch (startParams.StartType)
            {
                case DiamondSquareInitType.Even:
                    for (int i = 0; i < arraySize; i++)
                        map[i] = initialValue;
                    break;
                case DiamondSquareInitType.Random:
                    for (int i = 0; i < arraySize; i++)
                        map[i] = startParams.Rand.NextDouble();
                    break;
                case DiamondSquareInitType.EvenEdges:
                    map[0] = initialValue;
                    map[width - 1] = initialValue;
                    map[arraySize - 1] = initialValue;
                    map[arraySize - width] = initialValue;
                    break;
                case DiamondSquareInitType.RandomEdges:
                    map[0] = startParams.Rand.NextDouble();
                    map[width - 1] = startParams.Rand.NextDouble();
                    map[arraySize - 1] = startParams.Rand.NextDouble();
                    map[arraySize - width] = startParams.Rand.NextDouble();
                    break;
            }

            SquareStep(ref map, d, 0, 0, width + 1, height, startParams);
            DiamondStep(ref map, d, 0, 0, width + 1, height, startParams);

            MidPointDisplacement(ref map, d, startParams);

            return CreateBitmap(ref map, startParams);

        }

        private static void ProcessStartParameter(Dto startParams)
        {
            // a result of the diamond square algorythm must match the formular
            // h = 2^i + 1 for the height and 
            // w = 2^i for the width (as it should flip on the edge)
            // that means that i have to check if the size matches this formular
            startParams.Height = GetSize(GetRootOfSize(startParams.Height));
            startParams.Width = GetSize(GetRootOfSize(startParams.Width)) - 1;

            if (startParams.Rand == null)
                startParams.Rand = new Random(startParams.Seed);
        }

        private static int GetRootOfSize(int size)
        {
            int level = 0;
            if (size % 2 != 0) size--;
            decimal mySize = size;
            while (mySize > 2)
            {
                mySize /= 2;
                level++;
            }

            return level;
        }

        private static int GetSize(int x)
        {
            // 2^x+1 = (2 << X) + 1
            return (2 << x) + 1;
        }

        private static void MidPointDisplacement(ref double[] map, double d, Dto data)
        {
            int width = data.Height; // this is absicht!
            int height = data.Height;

            if (width <= 1 || height <= 1)
                return;

            int halfWidth = width; // TODO float?
            int halfHeight = height; // TODO float?

            do
            {
                d *= Math.Pow(2, -data.Roughness);
                halfWidth = halfWidth / 2;
                halfHeight = halfHeight / 2;
                if (halfHeight < 2 || halfWidth < 2) break;

                // at first do all squares (well calculate the middle)
                for (int y = 0; y < data.Height - 1; y += halfHeight)
                    for (int x = 0; x < data.Width - 1; x += halfWidth)
                        SquareStep(ref map, d, x, y, halfWidth + 1, halfHeight + 1, data);

                // than do the diamonds
                for (int y = 0; y < data.Height - 1; y += halfHeight)
                    for (int x = 0; x < data.Width - 1; x += halfWidth)
                        DiamondStep(ref map, d, x, y, halfWidth + 1, halfHeight + 1, data);

            } while (true);
        }

        private static void SquareStep(ref double[] map, double d, int x, int y, int width, int height, Dto data)
        {
            if (width <= 1 || height <= 1)
                return;

            // correct the width and height as it is not zero (0) based
            width -= 1;
            height -= 1;
            int globalSize = data.Height - 1;

            // A ------ B
            // |        |
            // | middle |
            // |        |
            // C ------ D
            int A = x + y * globalSize;
            int B = A + width;
            if (B % globalSize == 0)
                B -= globalSize;

            int C = x + (y + height) * globalSize;
            int D = C + width;
            if (D % globalSize == 0)
                D -= globalSize;

            //diamond step
            //calculate index for middlepoint   
            int middle = x + width / 2 + (y + height / 2) * globalSize;

            //calculate height for diamond point
            map[middle] = (map[A] + map[B] + map[C] + map[D]) / 4.0 + data.Rand.NextDouble().Lerp(-d, d);
        }

        private static void DiamondStep(ref double[] map, double d, int x, int y, int width, int height, Dto data)
        {
            if (width <= 1 || height <= 1)
                return;

            // correct the width and height as it is not zero (0) based
            width -= 1;
            height -= 1;
            int globalSize = data.Height - 1;

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
            int A = x + y * globalSize;
            int B = A + width;
            if (B % globalSize == 0)
                B -= globalSize;

            int C = x + (y + height) * globalSize;
            int D = C + width;
            if (D % globalSize == 0)
                D -= globalSize;

            int E = x + width / 2 + (y + height / 2) * globalSize;
            int F = x + (y + height / 2) * globalSize;
            int G = x + width / 2 + y * globalSize;
            int H = x + width + (y + height / 2) * globalSize;
            if (H % globalSize == 0)
                H -= globalSize;

            int I = x + width / 2 + (y + height) * globalSize;

            // we have to take care about the adgecase where E-left is outside of the image.
            // becase of we are generating a spheric texture the eLeft should be on the other side of the texture
            //
            //             ETop  (y)
            //               |    
            //      (x) A -- G -- B -- X -- X  
            //          |    |    |    |    |
            //    EL -- F -- E -- H -- ER - X
            //          |    |    |    |    |
            //          C -- I -- D -- X -- X
            //          |    |    |    |    |
            //          X -- EB - X -- X -- X
            //          |    |    |    |    |
            //          X -- X -- X -- X -- X
            //
            // Here ETop and ELeft are out of the boundaries of the image. So we flip the side for them so that EL should be the same location as ER
            // ETop should be EB.
            // First we have to check if the calculated coordinates for ETop and EL are out of bounds. Well for ETop it is easy. When the coordinate 
            // is lower than 0 it is out of bounds. 
            // if EL is not in the same line as E than it is out of bounds.
            int lineOfE = E / globalSize;
            int eLeft = E - width;
            int lineOfELeft = eLeft / globalSize;
            if (lineOfELeft < lineOfE)
                eLeft += globalSize;

            // same problem is the right point
            int eRight = E + width;
            int lineOfERight = eRight / globalSize;
            if (lineOfERight > lineOfE)
                eRight -= globalSize;

            int eTop = E - (height) * globalSize;
            if (eTop < 0) eTop = 0;


            int eBottom = E + (height) * globalSize;
            if (eBottom >= data.Width * data.Height) eBottom = 0;

            map[F] = (map[A] + map[C] + map[E] + map[eLeft]) / 4.0 + data.Rand.NextDouble().Lerp(-d, d);

            if (eTop > 0) map[G] = (map[A] + map[B] + map[E] + map[eTop]) / 4.0 + data.Rand.NextDouble().Lerp(-d, d);
            else map[G] = (map[A] + map[B] + map[E]) / 3.0 + data.Rand.NextDouble().Lerp(-d, d);

            map[H] = (map[B] + map[D] + map[E] + map[eRight]) / 4.0 + data.Rand.NextDouble().Lerp(-d, d);

            if (eBottom > 0) map[I] = (map[C] + map[D] + map[E] + map[eBottom]) / 4.0 + data.Rand.NextDouble().Lerp(-d, d);
            else map[I] = (map[C] + map[D] + map[E]) / 3.0 + data.Rand.NextDouble().Lerp(-d, d);
        }

        private static Bitmap CreateBitmap(ref double[] map, Dto data)
        {
            var bmp = new Bitmap(data.Width, data.Height);
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
                gfx.FillRectangle(Brushes.Black, 0, 0, data.Width, data.Height);
                for (int y = 0; y < data.Height; y++)
                    for (int x = 0; x < data.Width; x++)
                    {
                        int pixelIndex = y * data.Width + x;
                        int colorIndex = (int)((map[pixelIndex] + min) / range).Lerp(0, 255).Clamp(0, 255);
                        reusableBrush.Color = Color.FromArgb(colorIndex, colorIndex, colorIndex);
                        gfx.FillRectangle(reusableBrush, x, y, 1, 1);
                    }
            }
            return bmp;
        }
    }
}
