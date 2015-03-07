using System;
using System.IO;
using BotF2.TerrainGeneration.DiamondSquare;
using BotF2.TerrainGeneration.Texture;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets
{
    public class LoadRandomPlanetTexture : MonoBehaviour
    {
        public GameObject Planet;

        // Use this for initialization
        void Start()
        {
            GeneratePlanet(new GeneratePlanetDto() { Planet = Planet, Seed = Random.Range(0, Int32.MaxValue) });
            Planet.transform.Rotate(45, 0, 0, Space.World);
        }

        public class GeneratePlanetDto
        {
            public int Seed { get; set; }
            public GameObject Planet { get; set; }
        }

        public static void GeneratePlanet(object generatePlanetDto)
        {
            var dto = (GeneratePlanetDto)generatePlanetDto;
            var heightmap = HorizontalTransientDiamondSquare.Create(new HorizontalTransientDiamondSquare.Dto()
            {
                Width = 1024,
                Height = 1024,
                Seed = dto.Seed,
                Roughness = 0.8,
                StartType = DiamondSquareInitType.Random
            }, PostProcess);
            var shader = Shader.Find("Tessellation/Bumped Specular (displacement)");

            var mat = new Material(shader);
            var textureMap = Resources.Load<Texture2D>("Textures/terrains/terran");
            var text = TextureFactory.Create(heightmap, textureMap);
            heightmap = AdjustContrast(heightmap, 18);
            mat.SetTexture("_MainTex", text);
            var nrml = TextureFactory.CreateNormalMapFromHeightMap(GetHeightData(heightmap), 100);

            mat.SetTexture("_BumpMap", nrml);
            mat.SetTexture("_ParallaxMap", nrml);
            mat.SetFloat("_Parallax", 0.1122f);

            dto.Planet.transform.GetComponent<Renderer>().material = mat;
        }

        private static float[,] GetHeightData(Texture2D heightMap)
        {
            float[,] data = new float[heightMap.width, heightMap.height];
            for (int y = 0; y < heightMap.height; y++)
            {
                for (int x = 0; x < heightMap.width; x++)
                {
                    data[x, y] = heightMap.GetPixel(x, y).r;
                }
            }
            return data;
        }

        private static float[,] PostProcess(float[,] map)
        {
            float exponent = Random.Range(0.8f, 5f);
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    var value = map[x, y];
                    map[x, y] = (float)Math.Pow(value, exponent);
                }
            }
            return map;
        }

        private static Texture2D AdjustContrast(Texture2D map, float value)
        {
            float contrast = (float)Math.Pow((100 + value) / 100, 2);

            for (int y = 0; y < map.height; y++)
            {
                for (int x = 0; x < map.width; x++)
                {
                    float col = map.GetPixel(x, y).r;
                    float adjVal = ((col - 0.5f) * contrast + 0.5f);
                    if (adjVal < 1) col = adjVal;
                    map.SetPixel(x, y, new Color(col, col, col, 0));
                }
            }
            return map;

        }

        // Update is called once per frame
        void Update()
        {
            float currentAngle = Planet.transform.rotation.eulerAngles.y;
            Planet.transform.rotation = Quaternion.AngleAxis(currentAngle + (Time.deltaTime * 10), Vector3.up);
        }
    }
}
