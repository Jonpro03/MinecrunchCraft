using System;
using System.Collections.Generic;
using System.Linq;
using minecrunch.models.Generator;
using SharpNoise.Models;
using SharpNoise.Modules;

namespace minecrunch.models
{
    public class PerlinNoise
    {
        private Perlin terrainPerlin, terrain2Perlin;
        private Perlin cavePerlin;
        private Voronoi biomePerlin;
        private Plane worldPlane;
        private int biome_size;
        private float cave_fill;
        private float cave_height;
        private float cave_stretch;
        const float MAGIC_SEED_FACTOR1 = 0.000000565f;

        WorldGenerationSettings WorldSettings;
        Module WorldGen;
        Sphere WorldBiomes;

        private static readonly Lazy<PerlinNoise> lazy = new Lazy<PerlinNoise>(() => new PerlinNoise());

        public static PerlinNoise Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        private PerlinNoise()
        {
            biome_size = 2 * 100;
            cave_fill = (20 / 100.0f * 0.70f) + 0.30f;
            cave_height = 50 / 100.0f * 0.25f;
            cave_stretch = 80 / 1000.0f * 0.8f;

            WorldSettings = new WorldGenerationSettings();
            WorldGen = new WorldGenerator(WorldSettings).CreateModule();

            terrainPerlin = new Perlin
            {
                Seed = 4, //Todo: get from settings
                Frequency = 1,
                Lacunarity = 2,
                Quality = SharpNoise.NoiseQuality.Best,
                OctaveCount = 2,
                //Persistence = 1
            };

            terrain2Perlin = new Perlin
            {
                Seed = 3, //Todo: get from settings
                Frequency = 2,
                Lacunarity = 3,
                Quality = SharpNoise.NoiseQuality.Best,
                OctaveCount = 5,
                //Persistence = 1
            };

            biomePerlin = new Voronoi
            {
                Seed = 3,
                Frequency=3
            };

            cavePerlin = new Perlin
            {
                Seed = 326507,
                Lacunarity = 0,
                Quality = SharpNoise.NoiseQuality.Fast,
                OctaveCount = 1
            };
        }

        public int Biome(int bx, int by, int bz)
        {
            double val = biomePerlin.GetValue(bx / biome_size, by / biome_size, bz / biome_size) * 2;
            return (int) val > 1 ? 1 : (int) val;
        }

        //Function that inputs the position and spits out a float value based on the perlin noise
        public double Terrain(float x, float z)
        {
            int TerrainHeight = 12;
            //Generate a value from the given position, position is divided to make the noise more frequent.
            //double perlin1 = terrainPerlin.GetValue(x * MAGIC_SEED_FACTOR1, 0, z * MAGIC_SEED_FACTOR1) * 10;
            //double perlin2 = terrain2Perlin.GetValue(x / 1000, 0, z / 1000) * 3;

            //perlin2 = perlin2 > 0.75 ? perlin2 * 2 : perlin2 * 0.5;

            //int result = (int) (perlin1 * perlin2) + TerrainHeight;
            //return result < 0 ? 0 : result > 255 ? 255 : result;
            return TerrainHeight + WorldGen.GetValue(x * MAGIC_SEED_FACTOR1, 680, z * MAGIC_SEED_FACTOR1) * 0.021625f;
        }

        public bool Cave(float x, float y, float z)
        {
            y *= cave_height;
            x *= cave_stretch;
            z *= cave_stretch;
            double val = cavePerlin.GetValue(x, y, z);
            return !(val < cave_fill && val > 0);
        }
    }
}
