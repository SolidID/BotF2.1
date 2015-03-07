using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BotF2.Core.Extensions;
using UnityEngine;

namespace BotF2.TerrainGeneration.Texture
{
    public static class TextureFactory
    {
        public static Texture2D Create(Texture2D heightMap, Texture2D terrain)
        {
            var returnImage = new Texture2D(heightMap.width, heightMap.height, TextureFormat.RGBA32, false);
            for (int x = 0; x < returnImage.width; x++)
            {
                for (int y = 0; y < returnImage.height; y++)
                {
                    Color col = heightMap.GetPixel(x, y);
                    returnImage.SetPixel(x, y, GetColorForHeight(terrain, col.r));

                }
            }
            returnImage.Apply();
            return returnImage;
        }

        public static Texture2D CreateNormalMap(Texture2D heightmap, float strength)
        {
            strength = Mathf.Clamp(strength, 0.0F, 100.0F);
            Texture2D result;
            float xLeft;
            float xRight;
            float yUp;
            float yDown;
            float yDelta;
            float xDelta;
            result = new Texture2D(heightmap.width, heightmap.height, TextureFormat.ARGB32, true);
            for (int by = 0; by < result.height; by++)
            {
                for (int bx = 0; bx < result.width; bx++)
                {
                    xLeft = heightmap.GetPixel(bx - 1, by).r * strength;
                    xRight = heightmap.GetPixel(bx + 1, by).r * strength;
                    yUp = heightmap.GetPixel(bx, by - 1).r * strength;
                    yDown = heightmap.GetPixel(bx, by + 1).r * strength;
                    xDelta = ((xLeft - xRight) + 1) * 0.5f;
                    yDelta = ((yUp - yDown) + 1) * 0.5f;
                    result.SetPixel(bx, by, new Color(xDelta, yDelta, 1.0f, yDelta));
                }
            }
            result.Apply();
            return result;
        }

        public static Texture2D CreateNormalMapFromHeightMap(float[,] heightData, float factor)
        {
            int width = heightData.GetLength(0);
            int height = heightData.GetLength(1);
            var result = new Texture2D(width, height);
            for (int y = 0; y <= height - 1; y++)
            {
                for (int x = 0; x <= width - 1; x++)
                {
                    var point1 = new Vector3(x, y, factor * heightData[x, y]);
                    var point2 = new Vector3(x + 1, y, factor * heightData[(x + 1) % width, y]);
                    var point3 = new Vector3(x, y + 1, factor * heightData[x, (y + 1) % height]);

                    Vector3 diffX = point2 - point1;
                    Vector3 diffY = point3 - point1;

                    Vector3 normal = Vector3.Cross(diffX, diffY);
                    normal.Normalize();
                    result.SetPixel(x, y, new Color(0.5f * normal.x + 0.5f, 0.5f * normal.y + 0.5f, 0.5f * normal.z + 0.5f, heightData[x, y]));
                }
            }
            result.Apply();
            return result;

        }

        private static Color GetColorForHeight(Texture2D texture, float value)
        {
            return texture.GetPixel((int)(255 * value), 0);
        }
    }
}
