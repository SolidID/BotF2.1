using System;
using System.Linq;
using System.Text;
using BotF2.Core.Extensions;
using UnityEngine;
using Random = System.Random;

namespace BotF2.TerrainGeneration.DiamondSquare
{
    public static class HorizontalTransientDiamondSquare
    {
        public class Dto
        {
            public DiamondSquareInitType StartType;
            public double Roughness;
            public int Seed;
            public int Width;
            public int Height;
            public Random Rand;
        }

        public static Texture2D Create(Dto startParams)
        {
            return Create(startParams, null);
        }

        public static Texture2D Create(Dto startParams, Func<float[,], float[,]> postProcess)
        {
            ProcessStartParameter(startParams);

            float d = 0.5f;
            int width = startParams.Width;
            int height = startParams.Height;
            //int arraySize = width * height;

            var map = new float[width, height];
            var initialValue = (float)startParams.Rand.NextDouble();

            switch (startParams.StartType)
            {
                case DiamondSquareInitType.Even:
                    for (int y = 0; y < height; y++)
                        for (int x = 0; x < width; x++)
                            map[x, y] = initialValue;
                    break;
                case DiamondSquareInitType.Random:
                    for (int y = 0; y < height; y++)
                        for (int x = 0; x < width; x++)
                            map[x, y] = (float)startParams.Rand.NextDouble();
                    break;
                case DiamondSquareInitType.EvenEdges:
                    map[0, 0] = initialValue;
                    map[width - 1, 0] = initialValue;
                    map[0, height - 1] = initialValue;
                    map[width - 1, height - 1] = initialValue;
                    break;
                case DiamondSquareInitType.RandomEdges:
                    map[0, 0] = (float)startParams.Rand.NextDouble();
                    map[width - 1, 0] = (float)startParams.Rand.NextDouble();
                    map[0, height - 1] = (float)startParams.Rand.NextDouble();
                    map[width - 1, height - 1] = (float)startParams.Rand.NextDouble();
                    break;
            }

            SquareStep(ref map, d, 0, 0, width + 1, height, startParams);
            DiamondStep(ref map, d, 0, 0, width + 1, height, startParams);

            MidPointDisplacement(ref map, d, startParams);

            if (postProcess != null)
                map = postProcess.Invoke(map);

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

        private static void MidPointDisplacement(ref float[,] map, double d, Dto data)
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

        private static void SquareStep(ref float[,] map, double d, int x, int y, int width, int height, Dto data)
        {
            if (width <= 1 || height <= 1)
                return;

            // correct the width and height as it is not zero (0) based
            width -= 1; // TODO check if this is still necessary
            height -= 1; // TODO check if this is still necessary

            // A ------ B
            // |        |
            // |    M   |
            // |        |
            // C ------ D
            int Ax = x;
            int Ay = y;

            int Bx = (x + width) % map.GetLength(0); // flip around
            int By = y;

            int Cx = x;
            int Cy = y + height;

            int Dx = Bx;
            int Dy = Cy;

            //diamond step
            //calculate index for middlepoint   
            int Mx = x + width / 2;
            int My = y + height / 2;

            //calculate height for diamond point
            map[Mx, My] = ((map[Ax, Ay] + map[Bx, By] + map[Cx, Cy] + map[Dx, Dy]) / 4.0f + (float)data.Rand.NextDouble().Lerp(-d, d)).Clamp(0, 1);
        }

        private static void DiamondStep(ref float[,] map, double d, int x, int y, int width, int height, Dto data)
        {
            if (width <= 1 || height <= 1)
                return;

            // correct the width and height as it is not zero (0) based
            width -= 1; // TODO check if this is still necessary
            height -= 1; // TODO check if this is still necessary

            int halfWidth = width / 2;
            int halfHeight = height / 2;

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
            int Ax = x;
            int Ay = y;

            int Bx = (x + width) % map.GetLength(0); // flip around
            int By = y;

            int Cx = x;
            int Cy = y + height;

            int Dx = Bx;
            int Dy = Cy;

            int Ex = x + halfWidth;
            int Ey = y + halfHeight;

            int Fx = x;
            int Fy = Ey;

            int Gx = Ex;
            int Gy = Ay;

            int Hx = Bx;
            int Hy = Fy;

            int Ix = Ex;
            int Iy = Cy;

            // we have to take care about the edgecase where E-left is outside of the image.
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
            int ELx = x - halfWidth;
            if (ELx < 0) ELx += map.GetLength(0);
            int ELy = Ey;

            // same problem is the right point
            int ERx = (Bx + halfWidth) % map.GetLength(0);
            int ERy = Ey;

            int ETx = Gx;
            int ETy = Gy - halfHeight;
            if (ETy < 0) ETy = 0;


            int EBx = Ix;
            int EBy = Iy + halfHeight;
            if (EBy > map.GetLength(1) - 1) EBy = 0;
            map[Fx, Fy] = ((map[Ax, Ay] + map[Cx, Cy] + map[Ex, Ey] + map[ELx, ELy]) / 4.0f + (float)data.Rand.NextDouble().Lerp(-d, d)).Clamp(0, 1);

            if (ETy > 0) map[Gx, Gy] = ((map[Ax, Ay] + map[Bx, By] + map[Ex, Ey] + map[ETx, ETy]) / 4.0f + (float)data.Rand.NextDouble().Lerp(-d, d)).Clamp(0, 1);
            else map[Gx, Gy] = ((map[Ax, Ay] + map[Bx, By] + map[Ex, Ey]) / 3.0f + (float)data.Rand.NextDouble().Lerp(-d, d)).Clamp(0, 1);

            map[Hx, Hy] = ((map[Bx, By] + map[Dx, Dy] + map[Ex, Ey] + map[ERx, ERy]) / 4.0f + (float)data.Rand.NextDouble().Lerp(-d, d)).Clamp(0, 1);

            if (EBy > 0) map[Ix, Iy] = ((map[Cx, Cy] + map[Dx, Dy] + map[Ex, Ey] + map[EBx, EBy]) / 4.0f + (float)data.Rand.NextDouble().Lerp(-d, d)).Clamp(0, 1);
            else map[Ix, Iy] = ((map[Cx, Cy] + map[Dx, Dy] + map[Ex, Ey]) / 3.0f + (float)data.Rand.NextDouble().Lerp(-d, d)).Clamp(0, 1);
        }

        private static Texture2D CreateBitmap(ref float[,] heightData, Dto data)
        {
            var bmp = new Texture2D(data.Width, data.Height, TextureFormat.RGBA32, false);
            float min = 0, max = 0;

            for (int y = 0; y < heightData.GetLength(1); y++)
                for (int x = 0; x < heightData.GetLength(0); x++)
                {
                    float h = heightData[x, y];
                    if (h < min)
                        min = h;
                    if (h > max)
                        max = h;
                }

            float range = max - min;

            for (int y = 0; y < heightData.GetLength(1); y++)
                for (int x = 0; x < heightData.GetLength(0); x++)
                {
                    Color col = Color.Lerp(Color.black, Color.white, ((heightData[x, y] + min) / range));
                    bmp.SetPixel(x, y, col);
                }
            bmp.Apply();
            return bmp;
        }
    }
}
